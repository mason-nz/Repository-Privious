using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.BatchMedia;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.BatchMedia
{
    public class BatchMedia : DataBase
    {
        public static readonly BatchMedia Instance = new BatchMedia();
        public DataTable GetListByMedia(int MediaType, string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT TOP 2
                                    VUI.SysName UserName ,
                                    BM.CreateTime
                            FROM    dbo.BatchMedia BM
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.CreateUserID
                            WHERE   BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                    AND BM.MediaType = {MediaType}");
            switch (MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND BM.MediaNumber='{NumberOrName}'");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    if (!string.IsNullOrEmpty(NumberOrName))
                        sbSql.Append($"AND BM.MediaName='{NumberOrName}'");
                    break;
                default:
                    break;
            }
            sbSql.Append($" ORDER BY BM.CreateTime DESC ");
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public DataTable GetListByCar(int brandID, int serialID)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT TOP 50
                                    VUI.SysName UserName ,
                                    BM.CreateTime
                            FROM    dbo.BatchMedia BM
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI ON VUI.UserID = BM.CreateUserID
                            WHERE   BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                    AND BM.BrandID = {brandID}");

            //if (serialID != -2)
                sbSql.Append($"AND BM.SerialID = {serialID}");

            sbSql.Append($" ORDER BY BM.CreateTime DESC ");
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public DataTable GetModelByMedia(Entities.BatchMedia.QueryBatchMedia query)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  *
                            FROM    dbo.BatchMedia BM
                            WHERE BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体} ");

            if (query.MediaType != -2)
                sbSql.Append($" AND BM.MediaType = {query.MediaType} ");

            if (query.MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信)
            {
                if (!string.IsNullOrEmpty(query.MediaNumber))
                    sbSql.Append($" AND BM.MediaNumber = '{query.MediaNumber}' ");
            }
            else
            {
                if (!string.IsNullOrEmpty(query.MediaName))
                    sbSql.Append($" AND BM.MediaName = '{query.MediaName}' ");
            }
            
            if (query.Status != -2)
                sbSql.Append($" AND BM.Status = {query.Status} ");

            if (query.CurrentUserID != -2)
                sbSql.Append($" AND BM.CreateUserID = {query.CurrentUserID} ");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public DataTable GetModelByCar(Entities.BatchMedia.QueryBatchMedia query)
        {
            var sbSql = new StringBuilder();
            if (query.BrandID != -2 && query.SerialID == -2)
            {
                sbSql.Append($@"
                            SELECT  *
                            FROM    dbo.BatchMedia BM
                            WHERE BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.子品牌} 
                                  AND BM.BrandID = {query.BrandID} ");
            }
            else
            {
                sbSql.Append($@"
                            SELECT  *
                            FROM    dbo.BatchMedia BM
                            WHERE BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.车型} 
                                  AND BM.BrandID = {query.BrandID} 
                                  AND BM.SerialID = {query.SerialID} ");
            }


            if (query.Status != -2)
                sbSql.Append($" AND BM.Status = {query.Status} ");

            if (query.CurrentUserID != -2)
                sbSql.Append($" AND BM.CreateUserID = {query.CurrentUserID} ");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        public Entities.BatchMedia.BatchMedia GetModelByRecID(int batchMediaID)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  *
                            FROM    dbo.BatchMedia BM
                            WHERE 1=1 ");

            if (batchMediaID != -2)
                sbSql.Append($" AND BM.BatchMediaID = {batchMediaID} ");

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<Entities.BatchMedia.BatchMedia>(data.Tables[0]);
        }

        public int Insert(Entities.BatchMedia.BatchMedia entity)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"INSERT dbo.BatchMedia
                                ( TaskType ,
                                  BatchAuditID ,
                                  MediaType ,
                                  MediaName ,
                                  MediaID ,
                                  BrandID ,
                                  SerialID ,
                                  Status ,
                                  CreateTime ,
                                  CreateUserID ,
                                  MediaNumber ,
                                  HeadImg ,
                                  IsSelfDo ,
                                  HomeUrl
                                )
                        VALUES  ( @TaskType , -- TaskType - int
                                  @BatchAuditID , -- BatchAuditID - int
                                  @MediaType , -- MediaType - int
                                  @MediaName , -- MediaName - varchar(200)
                                  @MediaID , -- MediaID - int
                                  @BrandID , -- BrandID - int
                                  @SerialID , -- SerialID - int
                                  @Status , -- Status - int
                                  GETDATE() , -- CreateTime - datetime
                                  @CreateUserID , -- CreateUserID - int
                                  @MediaNumber , -- MediaNumber - varchar(100)
                                  @HeadImg , -- HeadImg - varchar(200)
                                  @IsSelfDo , -- IsSelfDo - bit
                                  @HomeUrl  -- HomeUrl - varchar(200)
                                )");
            sbSql.Append(";SELECT SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@TaskType",entity.TaskType),
                        new SqlParameter("@BatchAuditID",entity.BatchAuditID),
                        new SqlParameter("@MediaType",entity.MediaType),
                        new SqlParameter("@MediaName",entity.MediaName),
                        new SqlParameter("@MediaID",entity.MediaID),
                        new SqlParameter("@BrandID",entity.BrandID),
                        new SqlParameter("@SerialID",entity.SerialID),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@CreateUserID",entity.CreateUserID),
                        new SqlParameter("@MediaNumber",entity.MediaNumber),
                        new SqlParameter("@HeadImg",entity.HeadImg),
                        new SqlParameter("@IsSelfDo",entity.IsSelfDo),
                        new SqlParameter("@HomeUrl",entity.HomeUrl)
                        };


            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        #region 获取媒体信息
        public MediaModel GetMediaModel(int MediaType, string NumberOrName)
        {
            var sbSql = new StringBuilder();
            switch (MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    sbSql.Append(GetWeiXinSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                    sbSql.Append(GetAPPSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                    sbSql.Append(GetVideoSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                    sbSql.Append(GetBroadcastSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                    sbSql.Append(GetWeiboSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                    sbSql.Append(GetTouTiaoSQL(NumberOrName));
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    sbSql.Append(GetSouHuSQL(NumberOrName));
                    break;
            }
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return DataTableToEntity<MediaModel>(data.Tables[0]);
        }
        protected string GetWeiXinSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.微信} MediaType ,
                                        WOA.NickName Name ,
                                        WOA.HeadImg ,
                                        WOA.WxNumber Number ,
                                        WOA.CreateTime ,
                                        WOA.FansCount StatisticsCount ,
                                        ( SELECT    AVG(VWAI.ReadNum)
                                            FROM      BaseData2017.dbo.v_Weixin_ArticleInfo VWAI
                                            WHERE     VWAI.Location = 1
                                                    AND VWAI.WxNum = WOA.WxNumber
                                        ) ReadCount ,
                                        CAST(( CASE ( SELECT  COUNT(1)
                                                        FROM    Chitunion2017.dbo.Media_Weixin MW
                                                                JOIN Chitunion2017.dbo.UserInfo UI ON MW.CreateUserID = UI.UserID
                                                        WHERE   MW.Status = 0
                                                                AND MW.AuditStatus = 43002
                                                                AND MW.Number = WOA.WxNumber
                                                                AND UI.Source = 3001
                                                    )
                                                    WHEN 0 THEN 0
                                                    ELSE 1
                                                END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Weixin_OAuth WOA
                                WHERE   1 = 1
                            ");
            sbSql.AppendFormat($@"AND WOA.WxNumber= '{NumberOrName}'");
            return sbSql.ToString();
        }
        protected string GetAPPSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.APP} MediaType ,
                                        MBPC.Name ,
                                        MBPC.HeadIconURL HeadImg ,
                                        MBPC.CreateTime ,
                                        MBPC.DailyLive StatisticsCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.Media_PCAPP MP
                                                                JOIN Chitunion2017.dbo.UserInfo UI ON MP.CreateUserID = UI.UserID
                                                      WHERE     MP.Status = 0
                                                                AND MP.AuditStatus = 48002
                                                                AND MP.Name = MBPC.Name
                                                                AND UI.Source = 3001
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_BasePCAPP MBPC
                                WHERE   1 = 1 
                            ");
            sbSql.AppendFormat($@" AND MBPC.Name= '{NumberOrName}'");

            return sbSql.ToString();
        }
        protected string GetVideoSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.视频} MediaType ,
                                        MV.Name ,
                                        MV.HeadIconURL HeadImg ,
                                        MV.CreateTime ,
                                        MV.FansCount StatisticsCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.UserInfo UI
                                                      WHERE     UI.Source = 3001
                                                                AND UI.UserID = MV.CreateUserID
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_Video MV
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MV.Status = 0
                            ");
            sbSql.AppendFormat($@"AND MV.Name = '{NumberOrName}'");

            return sbSql.ToString();
        }
        protected string GetBroadcastSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.直播} MediaType ,
                                        MB.Name ,
                                        MB.HeadIconURL HeadImg ,
                                        MB.CreateTime ,
                                        MB.FansCount StatisticsCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.UserInfo UI
                                                      WHERE     UI.Source = 3001
                                                                AND UI.UserID = MB.CreateUserID
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_Broadcast MB
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MB.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MB.Status = 0
                            ");
            sbSql.AppendFormat($@"AND MB.Name = '{NumberOrName}'");
            return sbSql.ToString();
        }
        protected string GetWeiboSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博} MediaType ,
                                        MV.Name ,
                                        MV.HeadIconURL HeadImg ,
                                        MV.CreateTime ,
                                        MV.FansCount StatisticsCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.UserInfo UI
                                                      WHERE     UI.Source = 3001
                                                                AND UI.UserID = MV.CreateUserID
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_Weibo MV
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MV.Status = 0
                            ");
            sbSql.AppendFormat($@"AND MV.Name = '{NumberOrName}'");

            return sbSql.ToString();
        }
        protected string GetTouTiaoSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.头条} MediaType ,
                                        MT.UserName Name ,
                                        MT.HeadImg ,
                                        MT.Url HomeUrl ,
                                        MT.CreateTime ,
                                        MT.FansCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.UserInfo UI
                                                      WHERE     UI.Source = 3001
                                                                AND UI.UserID = MT.CreateUserID
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_TouTiao MT
                                WHERE   1 = 1
                            ");
            sbSql.AppendFormat($@"AND MT.UserName = '{NumberOrName}'");

            return sbSql.ToString();
        }
        protected string GetSouHuSQL(string NumberOrName)
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} MediaType ,
                                        MT.UserName Name ,
                                        MT.HeadImg ,
                                        MT.Url HomeUrl ,
                                        MT.CreateTime ,
                                        MT.FansCount ,
                                        CAST(( CASE ( SELECT    COUNT(1)
                                                      FROM      Chitunion2017.dbo.UserInfo UI
                                                      WHERE     UI.Source = 3001
                                                                AND UI.UserID = MT.CreateUserID
                                                    )
                                                 WHEN 0 THEN 0
                                                 ELSE 1
                                               END ) AS BIT) IsSelfDo
                                FROM    Chitunion2017.dbo.Media_SouHu MT
                                WHERE   1 = 1
                            ");
            sbSql.AppendFormat($@"AND MT.UserName = '{NumberOrName}'");

            return sbSql.ToString();
        }
        #endregion

        #region 更新待审状态，提交人
        public int UpdatePendingSubmitTime(int batchMediaID, int batchAuditID)
        {
            int retval = 0;
            string sqlstr = $@"
                                UPDATE  dbo.BatchMedia
                                SET     Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审} ,
                                        SubmitTime = GETDATE() ,
                                        BatchAuditID = {batchAuditID}
                                WHERE   BatchMediaID = {batchMediaID}";
            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);

            return retval;
        }
        #endregion
    }
}
