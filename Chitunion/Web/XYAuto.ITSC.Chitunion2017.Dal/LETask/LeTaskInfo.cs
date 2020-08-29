using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.LETask;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.LETask
{
    //任务表

    //任务表
    public partial class LeTaskInfo : DataBase
    {
        public static readonly LeTaskInfo Instance = new LeTaskInfo();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.LETask.LeTaskInfo entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into LE_TaskInfo(");
            strSql.Append("TaskName,BillingRuleName,MaterialUrl,MaterialID,TaskType,RuleCount,TakeCount,BeginTime,EndTime,Status,TaskAmount,CPCPrice,CPLPrice,CreateTime,ImgUrl,Synopsis,CategoryID,CPCLimitPrice,CPLLimitPrice");
            strSql.Append(") values (");
            strSql.Append("@TaskName,@BillingRuleName,@MaterialUrl,@MaterialID,@TaskType,@RuleCount,@TakeCount,@BeginTime,@EndTime,@Status,@TaskAmount,@CPCPrice,@CPLPrice,@CreateTime,@ImgUrl,@Synopsis,@CategoryID,@CPCLimitPrice,@CPLLimitPrice");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@TaskName",entity.TaskName),
                        new SqlParameter("@BillingRuleName",entity.BillingRuleName),
                        new SqlParameter("@MaterialUrl",entity.MaterialUrl),
                        new SqlParameter("@MaterialID",entity.MaterialID),
                        new SqlParameter("@TaskType",entity.TaskType),
                        new SqlParameter("@RuleCount",entity.RuleCount),
                        new SqlParameter("@TakeCount",entity.TakeCount),
                        new SqlParameter("@BeginTime",entity.BeginTime),
                        new SqlParameter("@EndTime",entity.EndTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@TaskAmount",entity.TaskAmount),
                        new SqlParameter("@CPCPrice",entity.CPCPrice),
                        new SqlParameter("@CPLPrice",entity.CPLPrice),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@ImgUrl",entity.ImgUrl),
                        new SqlParameter("@Synopsis",entity.Synopsis),
                        new SqlParameter("@CategoryID",entity.CategoryID),
                        new SqlParameter("@CPCLimitPrice",entity.CPCLimitPrice),
                        new SqlParameter("@CPLLimitPrice",entity.CPLLimitPrice),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo> VerifyTaskReceviceByDistribute(int taskId, int userId, int channelId)
        {
            var sql = $@"

                    --判断用户是否领取过任务
                    DECLARE @TaskId INT = {taskId},
		                    @UserID INT = {userId},
                            @ChannelID INT = {channelId},
		                    @TaskStatus INT = 0,@TaskRuleCount INT,@TaskTakeCount INT

                    --将数据存储
                    SELECT T.*
	                       INTO #Temp_TaskInfo
                    FROM dbo.LE_TaskInfo T WITH(NOLOCK)
                    WHERE T.RecID = @TaskId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_TaskInfo))
                    BEGIN
	                    SELECT 1001 AS ResultCode--任务信息不存在
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT @TaskStatus = T.Status ,@TaskRuleCount = T.RuleCount,@TaskTakeCount = T.TakeCount
                    FROM #Temp_TaskInfo AS T WITH(NOLOCK)
                    IF(@TaskStatus != {(int)LeTaskStatusEnum.Ing})
                    BEGIN
	                    SELECT 1002 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    IF(@TaskTakeCount >= @TaskRuleCount)
                    BEGIN
	                    SELECT 1003 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    IF(EXISTS( SELECT 1 FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.TaskID = @TaskId
                        AND AO.UserID = @UserID))
                    BEGIN
	                    SELECT 1004 AS ResultCode --不满足条件,您已领取过该任务
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    --同一个用户1小时之内，只能领取60个不同的任务
                    DECLARE @UserTaskCount INT = 0
                    SELECT @UserTaskCount = COUNT(*) FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.UserID = @UserID
                        AND AO.ChannelID = @ChannelID
                        AND AO.CreateTime BETWEEN DATEADD(MINUTE,-60,'{DateTime.Now}') AND '{DateTime.Now}'

                    IF(@UserTaskCount >=60)
                    BEGIN
	                    SELECT 1005 AS ResultCode--同一个用户1小时之内，只能领取60个不同的任务
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT  0 AS ResultCode;--验证通过
                    SELECT  * FROM  #Temp_TaskInfo;
                    ";

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo>(
                DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.LeTaskInfo>(obj.Tables[1]));
        }

        public Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo> VerifyTaskReceviceByDistribute(int taskId, string userIdentity, int channelId)
        {
            var sql = $@"

                    --判断用户是否领取过任务
                    DECLARE @TaskId INT = {taskId},
                            @UserIdentity varchar(50) = '{userIdentity}',
                            @ChannelID INT = {channelId},
		                    @TaskStatus INT = 0,@TaskRuleCount INT,@TaskTakeCount INT

                    --将数据存储
                    SELECT T.*
	                       INTO #Temp_TaskInfo
                    FROM dbo.LE_TaskInfo T WITH(NOLOCK)
                    WHERE T.RecID = @TaskId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_TaskInfo))
                    BEGIN
	                    SELECT 1001 AS ResultCode--任务信息不存在
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT @TaskStatus = T.Status ,@TaskRuleCount = T.RuleCount,@TaskTakeCount = T.TakeCount
                    FROM #Temp_TaskInfo AS T WITH(NOLOCK)
                    IF(@TaskStatus != {(int)LeTaskStatusEnum.Ing})
                    BEGIN
	                    SELECT 1002 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    IF(@TaskTakeCount >= @TaskRuleCount)
                    BEGIN
	                    SELECT 1003 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    IF(EXISTS( SELECT 1 FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.TaskID = @TaskId
                    AND AO.UserIdentity = @UserIdentity AND AO.ChannelID = @ChannelID))
                    BEGIN
	                    SELECT 1004 AS ResultCode --不满足条件,您已领取过该任务
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    --同一个用户1小时之内，只能领取60个不同的任务
                    DECLARE @UserTaskCount INT = 0
                    SELECT @UserTaskCount = COUNT(*) FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.UserIdentity = @UserIdentity AND AO.ChannelID = @ChannelID
                        AND AO.CreateTime BETWEEN DATEADD(MINUTE,-60,'{DateTime.Now}') AND '{DateTime.Now}'

                    IF(@UserTaskCount >=60)
                    BEGIN
	                    SELECT 1005 AS ResultCode--同一个用户1小时之内，只能领取60个不同的任务
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT  0 AS ResultCode;--验证通过
                    SELECT  * FROM  #Temp_TaskInfo;
                    ";

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo>(
                DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.LeTaskInfo>(obj.Tables[1]));
        }

        public Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo> VerifyTaskReceviceByCoverImage(int taskId, int userId, int channelId)
        {
            var sql = $@"

                    --判断用户是否领取过任务
                    DECLARE @TaskId INT = {taskId},
		                    @UserID INT = {userId},
                            @ChannelID INT = {channelId},
		                    @TaskStatus INT = 0,@TaskRuleCount INT,@TaskTakeCount INT

                    --将数据存储
                    SELECT T.*
	                       INTO #Temp_TaskInfo
                    FROM dbo.LE_TaskInfo T WITH(NOLOCK)
                    WHERE T.RecID = @TaskId

                    IF(NOT EXISTS(SELECT 1 FROM #Temp_TaskInfo))
                    BEGIN
	                    SELECT 1001 AS ResultCode--任务信息不存在
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT @TaskStatus = T.Status ,@TaskRuleCount = T.RuleCount,@TaskTakeCount = T.TakeCount
                    FROM #Temp_TaskInfo AS T WITH(NOLOCK)
                    IF(@TaskStatus != {(int)LeTaskStatusEnum.Ing})
                    BEGIN
	                    SELECT 1002 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    IF(@TaskTakeCount >= @TaskRuleCount)
                    BEGIN
	                    SELECT 1003 AS ResultCode--该任务已结束，不可领取
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    --IF(EXISTS( SELECT 1 FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    --WHERE AO.TaskID = @TaskId
                    --AND AO.UserID = @UserID))
                    --BEGIN
	                --    SELECT 1004 AS ResultCode --不满足条件,您已领取过该任务
	                --    SELECT * FROM #Temp_TaskInfo
	                --    RETURN
                    --END

                    --同一个用户1小时之内，只能领取60个不同的任务
                    DECLARE @UserTaskCount INT = 0
                    SELECT @UserTaskCount = COUNT(*) FROM DBO.LE_ADOrderInfo AS AO WITH(NOLOCK)
                    WHERE AO.UserID = @UserID
                        AND AO.ChannelID = @ChannelID
                        AND AO.CreateTime BETWEEN DATEADD(MINUTE,-60,'{DateTime.Now}') AND '{DateTime.Now}'

                    IF(@UserTaskCount >=60)
                    BEGIN
	                    SELECT 1005 AS ResultCode--同一个用户1小时之内，只能领取60个不同的任务
	                    SELECT * FROM #Temp_TaskInfo
	                    RETURN
                    END

                    SELECT  0 AS ResultCode;--验证通过
                    SELECT  * FROM  #Temp_TaskInfo;
                    ";

            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return new Tuple<VerifyResultCode, Entities.LETask.LeTaskInfo>(
                DataTableToEntity<Entities.LETask.VerifyResultCode>(obj.Tables[0]),
                DataTableToEntity<Entities.LETask.LeTaskInfo>(obj.Tables[1]));
        }

        public Entities.LETask.LeTaskInfo GetInfo(int taskId)
        {
            var sql = $@"

                    SELECT  T.RecID ,
                            T.TaskName ,
                            T.BillingRuleName ,
                            T.MaterialUrl ,
                            T.MaterialID ,
                            T.TaskType ,
                            T.RuleCount ,
                            T.TakeCount ,
                            T.BeginTime ,
                            T.EndTime ,
                            T.Status ,
                            T.TaskAmount ,
                            T.CPCPrice ,
                            T.CPLPrice ,
                            T.CreateTime ,
                            T.ImgUrl ,
                            T.Synopsis ,
                            T.CategoryID ,
                            T.CPCLimitPrice ,
                            T.CPLLimitPrice
                    FROM    dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                    WHERE   T.RecID = {taskId};
                    ";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.LETask.LeTaskInfo>(obj.Tables[0]);
        }

        public int UpdateTakeCount(int taskId)
        {
            var sql = $@"
                    UPDATE  dbo.LE_TaskInfo
                    SET     TakeCount = TakeCount + 1
                    WHERE   RecID = {taskId};

                    DECLARE @TaskRuleCount INT = 0,@TaskTakeCount INT = 0
                    SELECT
                            @TaskRuleCount = T.RuleCount ,
                            @TaskTakeCount = T.TakeCount
                    FROM    dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                    WHERE   RecID =  {taskId};

                    IF ( @TaskTakeCount >= @TaskRuleCount )
                        BEGIN
                           UPDATE  dbo.LE_TaskInfo SET Status = {(int)LeTaskStatusEnum.Finished} WHERE RecID = {taskId};
                        END
                    ";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public int VerifyTaskStorage(int materielId, int taskType)
        {
            var sql = $@"

                         SELECT COUNT(*)
                         FROM   dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                         WHERE  T.TaskType = {taskType}
                                AND T.MaterialID = {materielId};
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public int UpdateTaskStatus(int materielId, LeTaskStatusEnum taskStatusEnum = LeTaskStatusEnum.Finished)
        {
            var sql = $@"
                        UPDATE dbo.LE_TaskInfo SET Status = {(int)taskStatusEnum}
                        WHERE MaterialID = {materielId}
                    ";

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }

        public List<Entities.LETask.LeTaskCategory> GeTaskCategories()
        {
            var sql = $@"
                SELECT  COUNT(*) AS CC ,
                        SI.SceneID AS CategoryId,
                        SI.SceneName AS CategoryName
                FROM    dbo.DictScene AS SI WITH ( NOLOCK )
                        LEFT JOIN ( SELECT  T.RecID ,
                                            T.CategoryID
                                    FROM    Chitunion2017.dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                                    WHERE   T.Status = {(int)LeTaskStatusEnum.Ing}
                                  ) AS TT ON SI.SceneID = TT.CategoryID
                WHERE   TT.RecID > 0
                GROUP BY SI.SceneID ,
                        SI.SceneName
                ORDER BY CC DESC;
            ";
            var obj = SqlHelper.ExecuteDataset(ConnectChitunionOP2017, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.LeTaskCategory>(obj.Tables[0]);
        }

        public List<Entities.LETask.LeTaskCategory> GetCategoriesByMaterielId(int materielId)
        {
            var sql = $@"
                SELECT  AI.CategoryID AS CategoryId,
                        AI.CategoryNew CategoryName
                FROM    Chitunion_OP2017.dbo.MaterielExtend ME
                        JOIN BaseData2017.dbo.ArticleInfo AI ON AI.RecID = ME.ArticleID
                WHERE   ME.MaterielID = {materielId}
                ";
            var obj = SqlHelper.ExecuteDataset(ConnectChitunionOP2017, CommandType.Text, sql);
            return DataTableToList<Entities.LETask.LeTaskCategory>(obj.Tables[0]);
        }

        public int VerifyTaskArticleId(int materielId, int taskType)
        {
            var sql = $@"

                SELECT  COUNT(ME.ArticleID)
                FROM    dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                        LEFT JOIN Chitunion_OP2017.dbo.MaterielExtend AS ME WITH ( NOLOCK ) ON ME.MaterielID = T.MaterialID
                WHERE   T.MaterialID = {materielId}
                AND T.TaskType = {taskType}
                    ";

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public bool VerifyPromotionChannel(long promotionChannelId)
        {
            var sql = @"
	                SELECT COUNT(*) FROM dbo.LE_PromotionChannel_Dict WHERE DictID = @DictID
                ";
            SqlParameter[] parameters = {
                    new SqlParameter("@DictID", promotionChannelId)
            };
            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql, parameters);
            if (obj == null)
                return false;

            return Convert.ToInt32(obj) > 0;
        }

        #region V2.3

        public Entities.DTO.V2_3.TaskRspDTO GetDataByPage(Entities.DTO.V2_3.TaskResDTO res, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT TI.[RecID] AS TaskId,TaskName,MaterialUrl,TI.EndTime, TI.CPCPrice, [BillingRuleName],[Synopsis],[ImgUrl],TaskAmount AS TotalPrice,ISNULL(TotalAmount,0) AS TotalAmount");
            sql.AppendFormat(" ,CASE WHEN (SELECT COUNT(OI.RecID) FROM [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK) WHERE OI.TaskID = TI.RecID AND OI.UserID={0})>0 THEN 1 ELSE 0 END AS IsForward", res.UserID);
            sql.Append(" YanFaFROM [dbo].[LE_TaskInfo] TI WITH(NOLOCK) LEFT JOIN (");
            sql.Append(" SELECT OI.TaskID,OI.UserID,SUM(TotalMoney) AS TotalAmount FROM [dbo].[LE_ADOrderInfo] OI WITH(NOLOCK) ");

            sql.AppendFormat(" INNER JOIN [dbo].[LE_AccountBalance] AB WITH(NOLOCK) ON OI.RecID = AB.OrderID WHERE OI.UserID = {0} ", res.UserID);
            sql.Append($@" GROUP BY OI.TaskID,OI.UserID) P ON TI.RecID = P.TaskID WHERE TI.Status=194001 AND TI.TaskType=192001");
            if (res.SceneID > 0)
            {
                sql.AppendFormat(" AND [CategoryID]={0}", res.SceneID);
            }
            string strOrder = " TI.RecID DESC ";
            var outParam = new SqlParameter("@TotalRecorder", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var outParam2 = new SqlParameter("@TaskID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var sqlParams = new SqlParameter[]{
                outParam,
                outParam2,
                new SqlParameter("@SQL",sql.ToString()),
                new SqlParameter("@CurPage",res.PageIndex),
                new SqlParameter("@PageRows",res.PageSize),
                new SqlParameter("@Order",strOrder)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_Page2", sqlParams);
            totalCount = Convert.IsDBNull(sqlParams[0].Value) ? 0 : Convert.ToInt32(sqlParams[0].Value);
            int taskID = Convert.IsDBNull(sqlParams[1].Value) ? 0 : Convert.ToInt32(sqlParams[1].Value);
            List<Entities.DTO.V2_3.TaskInfo> list = DataTableToList<Entities.DTO.V2_3.TaskInfo>(data.Tables[0]);
            return new Entities.DTO.V2_3.TaskRspDTO() { TaskID = taskID, TotalCount = totalCount, TaskInfo = list };
        }


        public int GetTaskIdByMaterialID(int MaterialID)
        {
            var sql = @"SELECT [RecID] AS TaskId
                      FROM [Chitunion2017].[dbo].[LE_TaskInfo]
                      WHERE [MaterialID] = @MaterialID";
            SqlParameter[] parameters = {
                    new SqlParameter("@MaterialID", SqlDbType.Int,4)
            };
            parameters[0].Value = MaterialID;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql.ToString(), parameters);
            return Convert.ToInt32(obj == DBNull.Value ? -2 : obj);
        }

        #endregion
        #region V2.5
        public Entities.DTO.V2_3.TaskRspDTO GetDataByPageV2_5(Entities.DTO.V2_3.TaskResDTO res, string randomNumberConfig)
        {
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            var sbSql = new StringBuilder();
            sbSql.AppendLine($@"SELECT TOP {res.PageSize}
                                        TI.[RecID] AS TaskId ,
                                        TaskName ,
                                        MaterialUrl ,
                                        TI.EndTime ,
                                        TI.CPCPrice ,
                                        TI.MaterialID ,
                                        [BillingRuleName] ,
                                        [Synopsis] ,
                                        TaskAmount AS TotalPrice ,
                                        ISNULL(TotalAmount, 0) AS TotalAmount ,
                                        CASE WHEN ( SELECT  COUNT(OI.RecID)
                                                    FROM    [dbo].[LE_ADOrderInfo] OI WITH ( NOLOCK )
                                                    WHERE   OI.TaskID = TI.RecID
                                                            AND OI.UserID = {res.UserID}
                                                  ) > 0 THEN 1
                                             ELSE 0
                                        END AS IsForward ,
                                        ISNULL(TI.ImgUrl, AA.HeadImg) ImgUrl ,
                                        --AA.HeadImg2 ,
                                        --AA.HeadImg3 ,
                                        CASE WHEN ME.MaterielType = 7007 THEN AIN.HeadImgNew2
                                             ELSE AA.HeadImg2
                                        END AS HeadImg2 ,
                                        CASE WHEN ME.MaterielType = 7007 THEN AIN.HeadImgNew3
                                             ELSE AA.HeadImg3
                                        END AS HeadImg3 ,
                                        ME.CreateTime
                                        INTO #temp
                                FROM    [dbo].[LE_TaskInfo] TI WITH ( NOLOCK )
                                        LEFT JOIN ( SELECT  OI.TaskID ,
                                                            OI.UserID ,
                                                            SUM(TotalMoney) AS TotalAmount
                                                    FROM    [dbo].[LE_ADOrderInfo] OI WITH ( NOLOCK )
                                                            INNER JOIN [dbo].[LE_AccountBalance] AB WITH ( NOLOCK ) ON OI.RecID = AB.OrderID
                                                    WHERE   1 = 1
                                                            AND OI.UserID = {res.UserID}
                                                    GROUP BY OI.TaskID ,
                                                            OI.UserID
                                                  ) P ON TI.RecID = P.TaskID
                                        LEFT JOIN Chitunion_OP2017.dbo.MaterielExtend ME ON ME.MaterielID = TI.MaterialID
                                        LEFT JOIN Chitunion_OP2017.dbo.AccountArticle AA ON AA.ArticleID = ME.ArticleID
                                        LEFT JOIN BaseData2017.dbo.ArticleInfoNew AIN ON AIN.RecID = ME.ArticleID
                                WHERE   TI.Status = 194001
                                        AND TI.TaskType = 192001
                                    ");

            if (res.PageIndex > 0)
            {
                if (res.isFirstIdOnPage)
                    sbSql.AppendLine($@" AND TI.RecID <= {res.PageIndex} ");
                else
                    sbSql.AppendLine($@" AND TI.RecID < {res.PageIndex} ");
            } 
            if (res.SceneID > 0)
                sbSql.AppendLine($@" AND TI.CategoryID = {res.SceneID} ");

            if (!string.IsNullOrWhiteSpace(res.BeginTime) && res.IsNewUser)
            {
                sbSql.AppendLine($@" AND TI.BeginTime >= '{res.BeginTime}' ");
            }
            sbSql.AppendLine($@" ORDER BY TI.RecID DESC; ");

            sbSql.AppendLine($@"SELECT * , ReadCount = (SELECT Chitunion2017.dbo.[f_GenArticleReadNum](CreateTime, { randomNumberConfig})) FROM #temp ORDER BY TaskId DESC;");
            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            ITSC.Chitunion2017.Common.Loger.Log4Net.Info($"GetDataByPageV2_5->读取数据库耗时：{ts1.Subtract(ts2).Duration().TotalMilliseconds.ToString()}毫秒");
            List<Entities.DTO.V2_3.TaskInfo> list = DataTableToList<Entities.DTO.V2_3.TaskInfo>(ds.Tables[0]);
            TimeSpan ts3 = new TimeSpan(DateTime.Now.Ticks);
            ITSC.Chitunion2017.Common.Loger.Log4Net.Info($"GetDataByPageV2_5->表专集合耗时：{ts2.Subtract(ts3).Duration().TotalMilliseconds.ToString()}毫秒");
            return new Entities.DTO.V2_3.TaskRspDTO() { TaskInfo = list };
        }
        #endregion
    }
}