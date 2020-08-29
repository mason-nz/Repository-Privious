using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XYAuto.BUOC.ChiTuData2017.Entities.Distribute;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    //物料分发明细表（物料ID以天为单位存储）
    public partial class MaterielDistributeDetailed : DataBase
    {
        public static readonly MaterielDistributeDetailed Instance = new MaterielDistributeDetailed();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Distribute.MaterielDistributeDetailed entity)
        {
            var strSql = new StringBuilder();

            strSql.AppendFormat($@"
IF(NOT EXISTS(SELECT 1 FROM DBO.Materiel_DistributeDetailed WHERE MaterielId = {entity.MaterielId}
        AND Source = {entity.Source} AND Date = '{entity.Date.ToString("yyyy-MM-dd")}'))
BEGIN
");
            strSql.Append("insert into Materiel_DistributeDetailed(");
            strSql.Append("MaterielId,Date,PV,UV,OnLineAvgTime,BrowsePageAvg,JumpProportion,InquiryNumber,SessionNumber,TelConnectNumber,Source,Status,CreateTime,CreateUserId,DistributeUrl,DistributeDetailType");
            strSql.Append(") values (");
            strSql.Append("@MaterielId,@Date,@PV,@UV,@OnLineAvgTime,@BrowsePageAvg,@JumpProportion,@InquiryNumber,@SessionNumber,@TelConnectNumber,@Source,@Status,@CreateTime,@CreateUserId,@DistributeUrl,@DistributeDetailType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");

            strSql.AppendLine("");
            strSql.AppendLine(@"END");

            var parameters = new SqlParameter[]{
                        new SqlParameter("@MaterielId",entity.MaterielId),
                        new SqlParameter("@Date",entity.Date),
                        new SqlParameter("@PV",entity.PV),
                        new SqlParameter("@UV",entity.UV),
                        new SqlParameter("@OnLineAvgTime",entity.OnLineAvgTime),
                        new SqlParameter("@BrowsePageAvg",entity.BrowsePageAvg),
                        new SqlParameter("@JumpProportion",entity.JumpProportion),
                        new SqlParameter("@InquiryNumber",entity.InquiryNumber),
                        new SqlParameter("@SessionNumber",entity.SessionNumber),
                        new SqlParameter("@TelConnectNumber",entity.TelConnectNumber),
                        new SqlParameter("@Source",entity.Source),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@DistributeUrl",entity.DistributeUrl),
                        new SqlParameter("@CreateUserId",entity.CreateUserId),
                        new SqlParameter("@DistributeDetailType",entity.DistributeDetailType),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public List<Entities.Distribute.MaterielDistributeDetailed> GetList(DistributeQuery<Entities.Distribute.MaterielDistributeDetailed> query)
        {
            var sql = @" SELECT A.*,ForwardNumber FROM (
                        SELECT  DD.*
                        FROM    dbo.Materiel_DistributeDetailed AS DD WITH ( NOLOCK )
                        WHERE   DD.Status = 0
                        ";
            var parameters = new List<SqlParameter>();
            if (query.MaterielId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.MaterielId = @MaterielId";
                parameters.Add(new SqlParameter("@MaterielId", query.MaterielId));
            }
            if (!string.IsNullOrWhiteSpace(query.Date))
            {
                sql += $" AND DD.Date = @Date";
                parameters.Add(new SqlParameter("@Date", query.Date));
            }
            if (!string.IsNullOrWhiteSpace(query.StartDate))
            {
                sql += $" AND DD.Date >= @StartDate";
                parameters.Add(new SqlParameter("@StartDate", query.StartDate));
            }
            if (!string.IsNullOrWhiteSpace(query.EndDate))
            {
                sql += $" AND DD.Date <= @EndDate";
                parameters.Add(new SqlParameter("@EndDate", query.EndDate));
            }
            if (query.Source != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += $" AND DD.Source = @Source";
                parameters.Add(new SqlParameter("@Source", query.Source));
            }
            sql += " ) A LEFT JOIN ";
            sql += "  ( SELECT B.Date,SUM(CASE  WHEN ForwardNumber <=-1 THEN 0 WHEN ForwardNumber IS NULL THEN 0 ELSE ForwardNumber END)AS ForwardNumber  FROM dbo.[Materiel_DetailedStatistics] A WITH ( NOLOCK ) INNER JOIN [Materiel_DistributeDetailed] B WITH ( NOLOCK ) ON A.DistributeId = B.DistributeId WHERE B.MaterielId = " + query.MaterielId + "  GROUP BY B.Date) B ON A.Date = B.Date";
            sql += " ORDER BY A.Date ASC";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Distribute.MaterielDistributeDetailed>(data.Tables[0]);
        }

        public Entities.Distribute.MaterielInfo GetMaterielInfo(int materielId, int distributeType)
        {
            var sql = string.Format(@"
CREATE TABLE #Temp_UserInfo_Materiel
(
	UserID INT,
	SysName VARCHAR(200)
)

--用户临时表
INSERT INTO #Temp_UserInfo_Materiel
        ( UserID, SysName )
SELECT UserID,SysName FROM Chitunion2017.DBO.v_UserInfo

SELECT TOP 1
ME.MaterielID ,ME.Title,ME.MaterielUrl AS Url,ME.Name ,
                            DistributeUrl=(
							    SELECT TOP 1  DD1.DistributeUrl FROM DBO.Materiel_DistributeDetailed AS DD1 WITH(NOLOCK)
							    WHERE DD1.MaterielId = ME.MaterielID AND DD1.DistributeUrl <>'' AND DD1.Source = {0}
							),
	                        SI.SceneName,
	                        CS.SerialID ,
	                        CS.ShowName AS SerialName,
	                        CB.Name AS BrandName,
                            Channel = ( SELECT  STUFF(( SELECT  '、'
                                                                + ISNULL(DictName,'')
                                                                + '-' + ISNULL(MC.MediaTypeName,'')
																+'-' + ISNULL(MC.MediaNumber,'')
                                                        FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC WITH ( NOLOCK )
																LEFT JOIN DBO.DictInfo ON MC.ChannelType = DictId
                                                        WHERE   MC.MaterielID = ME.MaterielID
                                                      FOR
                                                        XML PATH('')
                                                      ), 1, 1, '')
                                      ),
                            IpList = ( SELECT   STUFF(( SELECT  '|'
                                            + CAST(MLP.Type AS VARCHAR(15))
                                            + ',' + ISNULL(MLP.TitleName, '')
                                    FROM    Chitunion_OP2017.dbo.MaterielIpLableInfo
                                            AS MLP WITH ( NOLOCK )
                                    WHERE   MLP.MaterielID = ME.MaterielID
                                  FOR
                                    XML PATH('')
                                  ), 1, 1, '')
                            ),
							ME.CreateTime AS AssembleTime,
                            UI.SysName AS AssembleUser,
							UI1.SysName AS DistributeUserQwy,
							UI2.SysName AS DistributeUserAgent,
							CH.DistributeDateAgent,
							CH1.DistributeDateQWY
                    FROM    Chitunion_OP2017.dbo.MaterielExtend AS ME WITH ( NOLOCK )
                            LEFT JOIN Chitunion_OP2017.dbo.SceneInfo AS SI WITH ( NOLOCK ) ON SI.SceneID = ME.SceneID
                            LEFT JOIN BaseData2017.dbo.CarSerial AS CS WITH ( NOLOCK ) ON CS.SerialID = ME.SerialID
                            LEFT JOIN BaseData2017.dbo.CarBrand AS CB WITH ( NOLOCK ) ON CB.BrandID = CS.BrandID
                            LEFT JOIN #Temp_UserInfo_Materiel AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
							LEFT JOIN(
									SELECT  CH.DistributeDateAgent ,
											DNAT1.MaterielId ,
											DNAT1.CreateUserId AS DistributeUserId
									FROM    dbo.Materiel_DistributeQingNiaoAgent AS DNAT1 WITH ( NOLOCK )
											INNER JOIN ( SELECT DNAT.MaterielId ,
																MIN(DNAT.DistributeDate) AS DistributeDateAgent
														 FROM   dbo.Materiel_DistributeQingNiaoAgent AS DNAT
																WITH ( NOLOCK )
														 GROUP BY DNAT.MaterielId
													   ) AS CH ON CH.MaterielId = DNAT1.MaterielId
																  AND CH.DistributeDateAgent = DNAT1.DistributeDate
									WHERE 1= 1 AND DNAT1.[Type] = 1
									GROUP BY CH.DistributeDateAgent ,
											DNAT1.MaterielId ,
											DNAT1.CreateUserId
							)AS CH ON CH.MaterielId = ME.MaterielID
							LEFT JOIN(
								SELECT  CH.DistributeDateQwy ,
										MC1.MaterielID ,
										MC1.CreateUserID AS DistributeUserId
								FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC1
										INNER JOIN ( SELECT MC.MaterielID ,
															MIN(MC.CreateTime) AS DistributeDateQwy
													 FROM   Chitunion_OP2017.dbo.MaterielChannel AS MC WITH ( NOLOCK )
													 WHERE MC.ChannelType IN (SELECT DictId FROM DBO.DictInfo WITH(NOLOCK) WHERE DictType = {0})
													 GROUP BY MC.MaterielID
												   ) AS CH ON MC1.MaterielID = CH.MaterielID
															  AND MC1.CreateTime = CH.DistributeDateQWY
								GROUP BY MC1.MaterielID ,
										CH.DistributeDateQwy ,
										MC1.CreateUserID
							) AS CH1 ON CH1.MaterielID = ME.MaterielID
							LEFT JOIN #Temp_UserInfo_Materiel AS UI1 WITH(NOLOCK) ON UI1.UserID = CH1.DistributeUserId
							LEFT JOIN #Temp_UserInfo_Materiel AS UI2 WITH(NOLOCK) ON UI2.UserID = CH.DistributeUserId
                    WHERE   ME.MaterielID = @MaterielID
                ", distributeType);
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@MaterielID",materielId)
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, parameters.ToArray());
            return DataTableToEntity<Entities.Distribute.MaterielInfo>(data.Tables[0]);
        }

        public List<MaterielDistributeTotal> GetDistributeTotals(string startDate, string endDate, string sqlWhere)
        {
            string[] where = sqlWhere.Split('#');
            var sbSql = new StringBuilder();

            sbSql.AppendFormat(@"
                        SELECT TotalSessionNumber,TotalInquiryNumber,TotalTelConnectNumber,TotalForwardNumber,TotalMateriel,TotalDistribute
                         FROM (
                        SELECT SUM(SessionNumber) AS TotalSessionNumber,
		                SUM(InquiryNumber) AS TotalInquiryNumber,
		                SUM(TelConnectNumber) AS TotalTelConnectNumber,
                        SUM(ForwardNumber) AS TotalForwardNumber
					 FROM
                --全网域的（多个）
               (SELECT
						ME.MaterielID,BrowsePageAvg,DistributeDate,DistributeType,DistributeUserId,
						ForwardNumber,InquiryNumber,JumpProportion,
				OnLineAvgTime,PV,SessionNumber,QuanWang.[Source],TelConnectNumber,UV,
		                UI.SysName AS AssembleUser ,
		                UI1.SysName AS DistributeUser ,
		                DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) LEFT JOIN (
					SELECT
	                T.DistributeDate ,
	                T.DistributeUserId,73001 AS DistributeType,D.PV,UV,OnLineAvgTime,JumpProportion,
					BrowsePageAvg,InquiryNumber,SessionNumber, TelConnectNumber,
					D.MaterielId,D.Source,ForwardNumber
	                FROM
	                (
	                --全网域分发+统计
	                SELECT  CH.DistributeDate ,
			                MC1.MaterielID ,
			                MC1.CreateUserID AS DistributeUserId
	                FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC1
			                INNER JOIN
							( SELECT MC.MaterielID ,
								     MIN(MC.CreateTime) AS DistributeDate
						       FROM   Chitunion_OP2017.dbo.MaterielChannel AS MC WITH (NOLOCK)
						       INNER JOIN  Chitunion_OP2017.DBO.DictInfo DC  WITH ( NOLOCK ) ON MC.ChannelType = DC.DictId
							   WHERE DictType = {0}
						       GROUP BY MC.MaterielID
					            ) AS CH ON MC1.MaterielID = CH.MaterielID AND MC1.CreateTime = CH.DistributeDate
	                GROUP BY MC1.MaterielID,CH.DistributeDate,MC1.CreateUserID
	                )AS T
	                INNER JOIN ( SELECT SUM(CASE  WHEN DD.PV <=-1 THEN 0 ELSE DD.PV END) AS PV ,
						                SUM(CASE  WHEN DD.UV <=-1 THEN 0 ELSE DD.UV END) AS UV ,
						                SUM(CASE  WHEN DD.OnLineAvgTime <=-1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
						                SUM(CASE  WHEN DD.JumpProportion <=-1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
						                SUM(CASE  WHEN DD.BrowsePageAvg <=-1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg ,
						                SUM(CASE  WHEN DD.InquiryNumber <=-1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
						                SUM(CASE  WHEN DD.SessionNumber <=-1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
						                SUM(CASE  WHEN DD.TelConnectNumber <=-1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
						                DD.MaterielId ,
						                DD.Source
				                FROM    dbo.Materiel_DistributeDetailed AS DD WITH (NOLOCK)
								WHERE  DD.DistributeUrl IS NOT NULL
				                GROUP BY DD.MaterielId,DD.Source
				                ) AS D ON D.MaterielId = T.MaterielID AND D.Source = {0} --全网域()
							LEFT JOIN
							(SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <=-1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber FROM
							[dbo].[Materiel_DetailedStatistics] MDS WITH ( NOLOCK )
							INNER JOIN   dbo.Materiel_DistributeDetailed MDD ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
							WHERE  MDD.Source = {0}
							GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId) AS QuanWang ON ME.MaterielID = QuanWang.MaterielId
							LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK) ON DI.DictId = QuanWang.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = QuanWang.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {2}
                UNION
                        SELECT
                        ME.MaterielID,BrowsePageAvg, DistributeDate, DistributeType, DistributeUserId,
                        ForwardNumber, InquiryNumber, JumpProportion,
                OnLineAvgTime, PV, SessionNumber, JingJiRen.[Source], TelConnectNumber, UV,
                        UI.SysName AS AssembleUser,
                        UI1.SysName AS DistributeUser,
                        DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) LEFT JOIN(
                    SELECT
                    T.DistributeDate,
                    T.DistributeUserId, 73002 AS DistributeType, D.PV, UV, OnLineAvgTime, JumpProportion,
                    BrowsePageAvg, InquiryNumber, SessionNumber, TelConnectNumber,
                    D.MaterielId, D.Source, ForwardNumber
                    FROM
                    (
                        SELECT  CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId AS DistributeUserId
                        FROM    dbo.Materiel_DistributeQingNiaoAgent AS DNAT1 WITH(NOLOCK)
                                INNER JOIN(SELECT DNAT.MaterielId,
                                                    MIN(DNAT.DistributeDate) AS DistributeDate
                                             FROM   dbo.Materiel_DistributeQingNiaoAgent AS DNAT WITH(NOLOCK)
                                             GROUP BY DNAT.MaterielId
                                           ) AS CH ON CH.MaterielId = DNAT1.MaterielId  AND CH.DistributeDate = DNAT1.DistributeDate
                        WHERE DNAT1.[Type] = 1
                        GROUP BY CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId
                    )AS T
                    INNER JOIN(SELECT SUM(CASE  WHEN DD.PV <= -1 THEN 0 ELSE DD.PV END) AS PV,
                                        SUM(CASE  WHEN DD.UV <= -1 THEN 0 ELSE DD.UV END) AS UV,
                                        SUM(CASE  WHEN DD.OnLineAvgTime <= -1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
                                        SUM(CASE  WHEN DD.JumpProportion <= -1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
                                        SUM(CASE  WHEN DD.BrowsePageAvg <= -1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg,
                                        SUM(CASE  WHEN DD.InquiryNumber <= -1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
                                        SUM(CASE  WHEN DD.SessionNumber <= -1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
                                        SUM(CASE  WHEN DD.TelConnectNumber <= -1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
                                        DD.MaterielId,
                                        DD.Source
                                FROM    dbo.Materiel_DistributeDetailed AS DD WITH(NOLOCK)
                                WHERE  DD.DistributeUrl IS NOT NULL
                                GROUP BY DD.MaterielId, DD.Source
                                ) AS D ON D.MaterielId = T.MaterielId AND D.Source = {1}--经纪人
                                LEFT JOIN
                            (SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <= -1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber
                            FROM[dbo].[Materiel_DetailedStatistics] MDS WITH(NOLOCK)
                            INNER JOIN   dbo.Materiel_DistributeDetailed MDD ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
                            WHERE  MDD.Source = {1}
                            GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId ) AS JingJiRen ON ME.MaterielID = JingJiRen.MaterielId
                            LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK)ON DI.DictId = JingJiRen.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = JingJiRen.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {2} ) T ) AA,
               (
                 SELECT COUNT(DISTINCT(MaterielID)) AS TotalMateriel FROM
                --全网域的（多个）
               (SELECT
						ME.MaterielID, BrowsePageAvg,DistributeDate,DistributeType,DistributeUserId,
						ForwardNumber,InquiryNumber,JumpProportion,
				OnLineAvgTime,PV,SessionNumber,QuanWang.[Source],TelConnectNumber,UV,
		                UI.SysName AS AssembleUser ,
		                UI1.SysName AS DistributeUser ,
		                DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) LEFT JOIN (
					SELECT
	                T.DistributeDate ,
	                T.DistributeUserId,73001 AS DistributeType,D.PV,UV,OnLineAvgTime,JumpProportion,
					BrowsePageAvg,InquiryNumber,SessionNumber, TelConnectNumber,
					D.MaterielId,D.Source,ForwardNumber
	                FROM
	                (
	                --全网域分发+统计
	                SELECT  CH.DistributeDate ,
			                MC1.MaterielID ,
			                MC1.CreateUserID AS DistributeUserId
	                FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC1
			                INNER JOIN
							( SELECT MC.MaterielID ,
								     MIN(MC.CreateTime) AS DistributeDate
						       FROM   Chitunion_OP2017.dbo.MaterielChannel AS MC WITH (NOLOCK)
						       INNER JOIN  Chitunion_OP2017.DBO.DictInfo DC  WITH ( NOLOCK ) ON MC.ChannelType = DC.DictId
							   WHERE DictType = {0}
						       GROUP BY MC.MaterielID
					            ) AS CH ON MC1.MaterielID = CH.MaterielID AND MC1.CreateTime = CH.DistributeDate
	                GROUP BY MC1.MaterielID,CH.DistributeDate,MC1.CreateUserID
	                )AS T
	                INNER JOIN ( SELECT SUM(CASE  WHEN DD.PV <=-1 THEN 0 ELSE DD.PV END) AS PV ,
						                SUM(CASE  WHEN DD.UV <=-1 THEN 0 ELSE DD.UV END) AS UV ,
						                SUM(CASE  WHEN DD.OnLineAvgTime <=-1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
						                SUM(CASE  WHEN DD.JumpProportion <=-1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
						                SUM(CASE  WHEN DD.BrowsePageAvg <=-1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg ,
						                SUM(CASE  WHEN DD.InquiryNumber <=-1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
						                SUM(CASE  WHEN DD.SessionNumber <=-1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
						                SUM(CASE  WHEN DD.TelConnectNumber <=-1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
						                DD.MaterielId ,
						                DD.Source
				                FROM    dbo.Materiel_DistributeDetailed AS DD WITH (NOLOCK)
								WHERE  DD.DistributeUrl IS NOT NULL
				                GROUP BY DD.MaterielId,DD.Source
				                ) AS D ON D.MaterielId = T.MaterielID AND D.Source = {0} --全网域()
							LEFT JOIN
							(SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <=-1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber FROM
							[dbo].[Materiel_DetailedStatistics] MDS WITH ( NOLOCK )
							INNER JOIN   dbo.Materiel_DistributeDetailed MDD ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
							WHERE  MDD.Source = {0}
							GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId) AS QuanWang ON ME.MaterielID = QuanWang.MaterielId
							LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK) ON DI.DictId = QuanWang.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = QuanWang.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {2}
                UNION
                        SELECT
                        ME.MaterielID, BrowsePageAvg, DistributeDate, DistributeType, DistributeUserId,
                        ForwardNumber, InquiryNumber, JumpProportion,
                OnLineAvgTime, PV, SessionNumber, JingJiRen.[Source], TelConnectNumber, UV,
                        UI.SysName AS AssembleUser,
                        UI1.SysName AS DistributeUser,
                        DI.DictName AS DistributeTypeName
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK) LEFT JOIN(
                    SELECT
                    T.DistributeDate,
                    T.DistributeUserId, 73002 AS DistributeType, D.PV, UV, OnLineAvgTime, JumpProportion,
                    BrowsePageAvg, InquiryNumber, SessionNumber, TelConnectNumber,
                    D.MaterielId, D.Source, ForwardNumber
                    FROM
                    (
                        SELECT  CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId AS DistributeUserId
                        FROM    dbo.Materiel_DistributeQingNiaoAgent AS DNAT1 WITH(NOLOCK)
                                INNER JOIN(SELECT DNAT.MaterielId,
                                                    MIN(DNAT.DistributeDate) AS DistributeDate
                                             FROM   dbo.Materiel_DistributeQingNiaoAgent AS DNAT WITH(NOLOCK)
                                             GROUP BY DNAT.MaterielId
                                           ) AS CH ON CH.MaterielId = DNAT1.MaterielId  AND CH.DistributeDate = DNAT1.DistributeDate
                        WHERE DNAT1.[Type] = 1
                        GROUP BY CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId
                    )AS T
                    INNER JOIN(SELECT SUM(CASE  WHEN DD.PV <= -1 THEN 0 ELSE DD.PV END) AS PV,
                                        SUM(CASE  WHEN DD.UV <= -1 THEN 0 ELSE DD.UV END) AS UV,
                                        SUM(CASE  WHEN DD.OnLineAvgTime <= -1 THEN 0 ELSE DD.OnLineAvgTime END) AS OnLineAvgTime,
                                        SUM(CASE  WHEN DD.JumpProportion <= -1 THEN 0 ELSE DD.JumpProportion END)AS JumpProportion,
                                        SUM(CASE  WHEN DD.BrowsePageAvg <= -1 THEN 0 ELSE DD.BrowsePageAvg END) AS BrowsePageAvg,
                                        SUM(CASE  WHEN DD.InquiryNumber <= -1 THEN 0 ELSE DD.InquiryNumber END) AS InquiryNumber,
                                        SUM(CASE  WHEN DD.SessionNumber <= -1 THEN 0 ELSE DD.SessionNumber END) AS SessionNumber,
                                        SUM(CASE  WHEN DD.TelConnectNumber <= -1 THEN 0 ELSE DD.TelConnectNumber END)AS TelConnectNumber,
                                        DD.MaterielId,
                                        DD.Source
                                FROM    dbo.Materiel_DistributeDetailed AS DD WITH(NOLOCK)
                                WHERE  DD.DistributeUrl IS NOT NULL
                                GROUP BY DD.MaterielId, DD.Source
                                ) AS D ON D.MaterielId = T.MaterielId AND D.Source = {1}--经纪人
                                LEFT JOIN
                            (SELECT MDS.MaterielId, SUM(CASE  WHEN ForwardNumber <= -1 THEN 0 ELSE ForwardNumber END)AS ForwardNumber
                            FROM[dbo].[Materiel_DetailedStatistics] MDS WITH(NOLOCK)
                            INNER JOIN   dbo.Materiel_DistributeDetailed MDD ON MDS.MaterielId = MDD.MaterielId
                            AND MDS.DistributeId = MDD.DistributeId
                            WHERE  MDD.Source = {1}
                            GROUP BY MDS.MaterielId
							) MD ON D.MaterielId = MD.MaterielId ) AS JingJiRen ON ME.MaterielID = JingJiRen.MaterielId
                            LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK)ON DI.DictId = JingJiRen.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = JingJiRen.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {3}
) B ) BB,        (
                  SELECT COUNT(DISTINCT(MaterielID)) AS TotalDistribute FROM
                --全网域的（多个）
               (SELECT
						ME.MaterielID, DistributeType
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK)
INNER JOIN (
					SELECT
	                T.DistributeDate ,
	                T.DistributeUserId,73001 AS DistributeType,MaterielID
	                FROM
	                (
	                --全网域分发+统计
	                SELECT  CH.DistributeDate ,
			                MC1.MaterielID ,
			                MC1.CreateUserID AS DistributeUserId
	                FROM    Chitunion_OP2017.dbo.MaterielChannel AS MC1
			                INNER JOIN
							( SELECT MC.MaterielID ,
								     MIN(MC.CreateTime) AS DistributeDate
						       FROM   Chitunion_OP2017.dbo.MaterielChannel AS MC WITH (NOLOCK)
						       INNER JOIN  Chitunion_OP2017.DBO.DictInfo DC  WITH ( NOLOCK ) ON MC.ChannelType = DC.DictId
							   WHERE DictType = {0}
						       GROUP BY MC.MaterielID
					            ) AS CH ON MC1.MaterielID = CH.MaterielID AND MC1.CreateTime = CH.DistributeDate
	                GROUP BY MC1.MaterielID,CH.DistributeDate,MC1.CreateUserID
	                )AS T ) QuanWang ON ME.MaterielId = QuanWang.MaterielId
				LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK) ON DI.DictId = QuanWang.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = QuanWang.DistributeUserId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {2}
                UNION
                        SELECT
                       ME.MaterielID, DistributeType
                FROM Chitunion_OP2017.dbo.MaterielExtend AS ME WITH(NOLOCK)
INNER JOIN(
                    SELECT
                    T.DistributeDate,
                    T.DistributeUserId, 73002 AS DistributeType, MaterielID
                    FROM
                    (
                        SELECT  CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId AS DistributeUserId
                        FROM    dbo.Materiel_DistributeQingNiaoAgent AS DNAT1 WITH(NOLOCK)
                                INNER JOIN(SELECT DNAT.MaterielId,
                                                    MIN(DNAT.DistributeDate) AS DistributeDate
                                             FROM   dbo.Materiel_DistributeQingNiaoAgent AS DNAT WITH(NOLOCK)
                                             GROUP BY DNAT.MaterielId
                                           ) AS CH ON CH.MaterielId = DNAT1.MaterielId  AND CH.DistributeDate = DNAT1.DistributeDate
                        WHERE DNAT1.[Type] = 1
                        GROUP BY CH.DistributeDate,
                                DNAT1.MaterielId,
                                DNAT1.CreateUserId
                    )AS T ) AS JingJiRen ON ME.MaterielID = JingJiRen.MaterielId
                LEFT JOIN Chitunion_OP2017.dbo.DictInfo AS DI WITH(NOLOCK)ON DI.DictId = JingJiRen.DistributeType
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI WITH(NOLOCK) ON UI.UserID = ME.CreateUserID
                LEFT JOIN Chitunion2017.dbo.v_UserInfo AS UI1 WITH(NOLOCK) ON UI1.UserID = JingJiRen.DistributeUserId
                INNER JOIN  dbo.Materiel_DistributeQingNiaoAgent aa on ME.MaterielID = aa.MaterielId
                WHERE ME.Status = 0 AND ME.ArticleFrom <> 69001 {2}
                GROUP BY ME.MaterielID, DistributeType
) C ) CC
            ", (int)DistributeTypeEnum.QuanWangYu, (int)DistributeTypeEnum.QingNiaoAgent, where[0], where[1]);
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToList<MaterielDistributeTotal>(data.Tables[0]);
        }

        public List<Entities.Distribute.MaterielDistributeDetailed> GetInfoByPullDataVerifyMaterile(int offestSize, int materielId, int source)
        {
            var sql = $@"
                        SELECT TOP {offestSize} * FROM dbo.Materiel_DistributeDetailed AS DD WITH(NOLOCK)
                        WHERE DD.MaterielId = {materielId} AND DD.Source = {source}
                        ORDER BY DD.Date ASC
                    ";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.Distribute.MaterielDistributeDetailed>(data.Tables[0]);
        }
    }
}