using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.MediaLabel
{
    public class MediaLabel : DataBase
    {
        public static readonly MediaLabel Instance = new MediaLabel();
        protected string GetMediaSql(int MediaType, string NumberOrName)
        {
            var sbSql = new StringBuilder();
            switch (MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.微信} MediaType ,
                                            WOA.NickName Name ,
                                            WOA.WxNumber Number ,
                                            WOA.HeadImg ,
                                            WOA.CreateTime
                                    FROM    Chitunion2017.dbo.Weixin_OAuth WOA
                                    WHERE WOA.WxNumber='{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.APP} MediaType ,
                                            MBPC.Name ,
                                            MBPC.Name Number ,
                                            MBPC.HeadIconURL HeadImg ,
                                            MBPC.CreateTime
                                    FROM    Chitunion2017.dbo.Media_BasePCAPP MBPC
                                    WHERE   MBPC.Name = '{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.视频} MediaType ,
                                            MV.Name ,
                                            MV.Number ,
                                            MV.HeadIconURL HeadImg ,
                                            MV.CreateTime
                                    FROM    Chitunion2017.dbo.Media_Video MV
                                            JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                    WHERE   UR.RoleID = 'SYS001RL00005'
                                            AND MV.Status = 0
                                            AND MV.Name = '{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.直播} MediaType ,
                                            MB.Name ,
                                            MB.Number ,
                                            MB.HeadIconURL HeadImg ,
                                            MB.CreateTime
                                    FROM    Chitunion2017.dbo.Media_Broadcast MB
                                            JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MB.CreateUserID
                                    WHERE   UR.RoleID = 'SYS001RL00005'
                                            AND MB.Status = 0
                                            AND MB.Name = '{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博} MediaType ,
                                            MV.Name ,
                                            MV.Number ,
                                            MV.HeadIconURL HeadImg ,
                                            MV.CreateTime
                                    FROM    Chitunion2017.dbo.Media_Weibo MV
                                            JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                    WHERE   UR.RoleID = 'SYS001RL00005'
                                            AND MV.Status = 0
                                            AND MV.Name = '{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.头条} MediaType ,
                                            MT.UserName Name ,
                                            MT.HeadImg ,
                                            MT.Url HomeUrl ,
                                            MT.CreateTime
                                    FROM    Chitunion2017.dbo.Media_TouTiao MT
                                    WHERE   1 = 1
                                            AND MT.UserName = '{NumberOrName}'; ");
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    sbSql.Append($@"
                                    SELECT TOP 1
                                            {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} MediaType ,
                                            MT.UserName Name ,
                                            MT.HeadImg ,
                                            MT.Url HomeUrl ,
                                            MT.CreateTime
                                    FROM    Chitunion2017.dbo.Media_SouHu MT
                                    WHERE   1 = 1
                                            AND MT.UserName = '{NumberOrName}'; ");
                    break;
                default:
                    break;
            }
            return sbSql.ToString();
        }
        public string GetLabelSql(int dictType)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  TBI.TitleID DictId,
                                    TBI.Name DictName,
                                    TBI.Type
                            FROM    dbo.TitleBasicInfo TBI
                            WHERE   TBI.Type = {dictType};");
            return sbSql.ToString();
        }
        public string GetLabelSql(int dictType, int mediaType, string numberORname)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  TBI.TitleID DictId,
                                    TBI.Name DictName,
                                    TBI.Type
                            FROM    dbo.TitleBasicInfo TBI
                            WHERE   TBI.Type = {dictType} ");
            if ((int)Entities.ENUM.ENUM.EnumMediaType.微信 == mediaType)
            {
                sbSql.Append($@"
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   dbo.MediaLabelResult MLR
                                                 WHERE  MLR.Status = 0
                                                        AND MLR.MediaType = {mediaType}
                                                        AND MLR.MediaNumber = '{numberORname}' ) ");
            }
            else
            {
                sbSql.Append($@"
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   dbo.MediaLabelResult MLR
                                                 WHERE  MLR.Status = 0
                                                        AND MLR.MediaType = {mediaType}
                                                        AND MLR.MediaName = '{numberORname}' ) ");
            }
            return sbSql.ToString();
        }
        public bool IsExistsLabelByMedia(int mediaType, string numberORname)
        {
            var sbSql = new StringBuilder();
            if ((int)Entities.ENUM.ENUM.EnumMediaType.微信 == mediaType)
            {
                sbSql.Append($@"
                                SELECT  COUNT(1)
                                FROM    dbo.MediaLabelResult MLR
                                WHERE   MLR.MediaType = {mediaType}
                                        AND MLR.MediaNumber = '{numberORname}'");
            }
            else
            {
                sbSql.Append($@"
                                SELECT  COUNT(1)
                                FROM    dbo.MediaLabelResult MLR
                                WHERE   MLR.MediaType = {mediaType}
                                        AND MLR.MediaName = '{numberORname}' ");
            }
            var data = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return (int)data > 0;
        }
        public Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable> RenderBatchMedia(int MediaType, string NumberOrName, int batchMediaID)
        {
            var sbSql = new StringBuilder();
            //媒体信息
            sbSql.Append(GetMediaSql(MediaType, NumberOrName));
            //固定标签项项
            sbSql.Append(GetLabelSql((int)Entities.ENUM.ENUM.EnumLabelType.分类, MediaType, NumberOrName));
            sbSql.Append(GetLabelSql((int)Entities.ENUM.ENUM.EnumLabelType.市场场景, MediaType, NumberOrName));
            sbSql.Append(GetLabelSql((int)Entities.ENUM.ENUM.EnumLabelType.分发场景, MediaType, NumberOrName));
            sbSql.Append(GetLabelSql((int)Entities.ENUM.ENUM.EnumLabelType.IP));


            if (
                (MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信
                || MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.头条
                || MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.搜狐)
                && batchMediaID != -2
                )
            {
                sbSql.Append($@"
                                SELECT  STUFF(( SELECT  ',' + CAST(BMA.ArticleID AS VARCHAR(50))
                                                FROM    dbo.BatchMediaArticle BMA
                                                WHERE   BMA.BatchMediaID = {batchMediaID}
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '')
                                ;");
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            DataTable dt5 = null;
            if (data.Tables.Count == 6)
                dt5 = data.Tables[5];
            return new Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable>(data.Tables[0], data.Tables[1], data.Tables[2], data.Tables[3], data.Tables[4], dt5);
        }
        public Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, Tuple<DataTable>> ViewBatchMedia(int batchMediaID, int MediaType, int batchAuditID)
        {
            var sbSql = new StringBuilder();
            //媒体信息item1
            sbSql.Append($@"SELECT  BM.MediaType ,
                                    BM.MediaName Name ,
                                    BM.MediaNumber Number ,
                                    BM.HeadImg ,
                                    BM.HomeUrl ,
                                    BM.SubmitTime CreateTime ,
                                    VUI1.SysName OperateInfoUserName ,
                                    VUI2.SysName AuditInfoUserName ,
                                    BM.AuditTime
                            FROM    dbo.BatchMedia BM
                                    JOIN Chitunion2017.dbo.v_UserInfo VUI1 ON VUI1.UserID = BM.CreateUserID
                                    LEFT JOIN Chitunion2017.dbo.v_UserInfo VUI2 ON VUI2.UserID = BM.AuditUserID
                            WHERE   BM.BatchMediaID = {batchMediaID};");
            //打标签项item2
            sbSql.Append($@"SELECT  BLH.Type ,
                                    BLH.TitleID DictId,
                                    BLH.LabelID ,
                                    BLH.Name DictName
                            FROM    dbo.BatchLabelHistory BLH
                            WHERE   BLH.BatchMediaID = {batchMediaID};");

            //审标签项item3
            sbSql.Append($@"SELECT  BAP.TitleID DictId,
                                    BAP.Name DictName,
                                    BAP.AuditLabelID LabelID ,
                                    BAP.Type
                            FROM    dbo.BatchAuditPassed BAP
                                    JOIN dbo.BatchMediaAudit BMA ON BAP.BatchAuditID = BMA.BatchAuditID
                                    JOIN dbo.BatchMedia BM ON BM.BatchAuditID = BMA.BatchAuditID
                            WHERE   BM.BatchMediaID = {batchMediaID};");

            //子IP项item4
            sbSql.Append($@"SELECT  IPSUB.TitleID DictId,
                                    IPSUB.LabelID ,
                                    IPSUB.SubIPID ,
                                    TBI.Name DictName
                            FROM    dbo.IPSubLabel IPSUB
                                    JOIN dbo.TitleBasicInfo TBI ON TBI.TitleID = IPSUB.TitleID
                            WHERE   IPSUB.BatchMediaID = {batchMediaID};");

            //子IP下的标签项item5
            sbSql.Append($@"SELECT  SON.SubIPID ,
                                    SON.Name DictName,
                                    SON.BatchMediaID
                            FROM    dbo.SonIPLabel SON
                            WHERE   SON.BatchMediaID = {batchMediaID};");

            //子IP项（审核）item6
            sbSql.Append($@"SELECT  IPSUBAudit.TitleID DictId,
                                    IPSUBAudit.AuditLabelID LabelID,
                                    IPSUBAudit.AuditSubIPID SubIPID,
                                    TBI.Name DictName
                            FROM    dbo.IPSubLabelAuditPassed IPSUBAudit
                                    JOIN dbo.TitleBasicInfo TBI ON TBI.TitleID = IPSUBAudit.TitleID
                            WHERE   IPSUBAudit.BatchAuditID = {batchAuditID};");

            //子IP下的标签项（审核）item7
            sbSql.Append($@"SELECT  SONAudit.AuditSubIPID SubIPID,
                                    SONAudit.Name DictName,
                                    SONAudit.BatchAuditID
                            FROM    dbo.SonIPLabelAuditPassed SONAudit
                            WHERE   SONAudit.BatchAuditID = {batchAuditID};");

            if (
                (MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信
                || MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.头条
                || MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.搜狐)
                && batchMediaID != -2
                )
            {
                sbSql.Append($@"
                                SELECT  STUFF(( SELECT  ',' + CAST(BMA.ArticleID AS VARCHAR(50))
                                                FROM    dbo.BatchMediaArticle BMA
                                                WHERE   BMA.BatchMediaID = {batchMediaID}
                                              FOR
                                                XML PATH('')
                                              ), 1, 1, '')
                                ;");
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            DataTable dt7 = null;
            if (data.Tables.Count == 8)
                dt7 = data.Tables[7];
            return new Tuple<DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, DataTable, Tuple<DataTable>>(data.Tables[0], data.Tables[1], data.Tables[2], data.Tables[3], data.Tables[4], data.Tables[5], data.Tables[6], new Tuple<DataTable>(dt7));
        }
        #region 根据母IP获取子IP
        public DataTable GetSubIPByPid(int pid)
        {
            string sql = $@"SELECT  *
                            FROM    dbo.IPSubLabel IPSub
                            WHERE   IPSub.LabelID = {pid}";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return data.Tables[0];
        }
        #endregion
        public int ArticleReceive(string articleIds, DateTime startDate, DateTime endDate, int articleCount, int resourceType, string number, int currentUserID, int batchMediaID)
        {
            int mediaType = -2;
            switch (resourceType)
            {
                case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.微信;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.头条;
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    mediaType = (int)Entities.ENUM.ENUM.EnumMediaType.搜狐;
                    break;
            }
            var sbSql = new StringBuilder();
            if (!string.IsNullOrEmpty(articleIds))
            {
                sbSql.Append(GetInsertSql_BatchMediaArticle(articleIds, mediaType, number, currentUserID, batchMediaID));
            }
            else
            {
                #region 过期不用
                //string strResource = string.Empty;
                //string strData = string.Empty;
                //string strMediaType = string.Empty;
                //switch (resourceType)
                //{
                //    case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                //        strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.微信} ";
                //        strData = $@" AND AI.DataId = '{number}' ";
                //        strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信} ";
                //        break;
                //    case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                //        strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.今日头条} ";
                //        strData = $@" AND AI.DataName = '{number}' ";
                //        strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条} ";
                //        break;
                //    case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                //        strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.搜狐} ";
                //        strData = $@" AND AI.DataName = '{number}' ";
                //        strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} ";
                //        break;
                //}
                //string strBegin = string.Empty;
                //string strEnd = string.Empty;
                //if (startDate == new DateTime(1900, 1, 1) && endDate == new DateTime(1900, 1, 1))
                //{
                //    //如果发布日期未选择则直接查询发布日期默认为最近7天的文章的按发布时间倒序的前30篇文章
                //    strBegin = string.Format(@" AND AI.PublishTime >= '{0}'", DateTime.Now.Date.AddDays(-7));
                //    strEnd = string.Format(@" AND AI.PublishTime < '{0}'", DateTime.Now.Date);
                //}

                //if (startDate != new DateTime(1900, 1, 1))
                //    strBegin = string.Format(@" AND AI.PublishTime >= '{0}'", startDate.Date);

                //if (endDate != new DateTime(1900, 1, 1))
                //    strEnd = string.Format(@" AND AI.PublishTime < '{0}'", endDate.Date.AddDays(1));

                //string strRUM = string.Empty;
                //if (articleCount == 0)//说明要领取查询出的所有文章
                //{ }
                //else
                //    strRUM = $@" AND T1.RNUM <= {articleCount}";
                //sbSql.AppendFormat($@"
                //                    SELECT  STUFF(( SELECT  ',' + CAST(T1.ArticleId AS VARCHAR(50))
                //                                    FROM    ( SELECT    AI.RecID ArticleId ,
                //                                                        AI.Url ,
                //                                                        AI.Title ,
                //                                                        AI.ReadNum ,
                //                                                        AI.LikeNum ,
                //                                                        AI.ComNum ,
                //                                                        AI.PublishTime ,
                //                                                        ROW_NUMBER() OVER ( ORDER BY AI.PublishTime DESC ) RNUM
                //                                              FROM      BaseData2017.dbo.ArticleInfo AI
                //                                              WHERE     1 = 1 
                //                                                        {strResource} 
                //                                                        {strData}
                //                                                        {strBegin}		
                //                                                        {strEnd}
                //                                                        AND AI.RecID NOT IN (
                //                                                        SELECT  BMAI.ArticleID
                //                                                        FROM    dbo.BatchMediaArticle BMAI
                //                                                        WHERE   1 = 1
                //                                                                AND BMAI.CreateUserID = {currentUserID} 
                //                                                                AND BMAI.MediaNumber = '{number}' 
                //                                                                {strMediaType}
                //                             )
                //                                            ) T1
                //                                    WHERE   1=1 
                //                                            --AND T1.RNUM <= {articleCount}
                //                                            {strRUM}
                //                                  FOR
                //                                    XML PATH('')
                //                                  ), 1, 1, '');
                //            ");
                #endregion
                string sql = string.Empty;
                switch (resourceType)
                {
                    case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                        sql = GetWeiXinSQL_ArticleReceive(startDate, endDate, articleCount, number, currentUserID);
                        break;
                    case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                        sql = GetTouTiaoSQL_ArticleReceive(startDate, endDate, articleCount, number, currentUserID);
                        break;
                    case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                        sql = GetSouHuSQL_ArticleReceive(startDate, endDate, articleCount, number, currentUserID);
                        break;
                }
                var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
                articleIds = data.Tables[0].Rows[0][0].ToString();
                sbSql = new StringBuilder();
                sbSql.Append(GetInsertSql_BatchMediaArticle(articleIds, mediaType, number, currentUserID, batchMediaID));
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
        }
        protected string GetWeiXinSQL_ArticleReceive(DateTime startDate, DateTime endDate, int articleCount, string number, int currentUserID)
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.WxNum = '{number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信} ";
            string strBegin = string.Empty;
            string strEnd = string.Empty;

            if (startDate != new DateTime(1900, 1, 1))
                strBegin = string.Format(@" AND AI.PubTime >= '{0}'", startDate.Date);

            if (endDate != new DateTime(1900, 1, 1))
                strEnd = string.Format(@" AND AI.PubTime < '{0}'", endDate.Date.AddDays(1));

            string strRUM = string.Empty;
            if (articleCount == 0)//说明要领取查询出的所有文章
            { }
            else
                strRUM = $@" AND T1.RNUM <= {articleCount}";
            sbSql.AppendFormat($@"
                                SELECT  STUFF(( SELECT  ',' + CAST(T1.ArticleId AS VARCHAR(50))
                                                FROM    ( SELECT    AI.RecID ArticleId ,                                   
                                                                    AI.PubTime ,
                                                                    ROW_NUMBER() OVER ( ORDER BY AI.PubTime DESC ) RNUM
                                                            FROM      BaseData2017.dbo.Weixin_ArticleInfo AI 
									  
                                                            WHERE     1 = 1 
                                                                    {strData}
                                                                    {strBegin}		
                                                                    {strEnd}
                                                                    AND AI.RecID NOT IN (
                                                                    SELECT  BMAI.ArticleID
                                                                    FROM    dbo.BatchMediaArticle BMAI
                                                                    WHERE   1 = 1
                                                                            AND BMAI.CreateUserID = {currentUserID} 
                                                                            AND BMAI.MediaNumber = '{number}' 
                                                                            {strMediaType}
                                            )
                                                        ) T1
                                                WHERE   1=1 
                                                        --AND T1.RNUM <= {articleCount}
                                                        {strRUM}
                                                FOR
                                                XML PATH('')
                                                ), 1, 1, '');
                            ");

            return sbSql.ToString();
        }
        protected string GetTouTiaoSQL_ArticleReceive(DateTime startDate, DateTime endDate, int articleCount, string number, int currentUserID)
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.UserName = '{number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条} ";
            string strBegin = string.Empty;
            string strEnd = string.Empty;

            if (startDate != new DateTime(1900, 1, 1))
                strBegin = string.Format(@" AND AI.PublishTime >= '{0}'", startDate.Date);

            if (endDate != new DateTime(1900, 1, 1))
                strEnd = string.Format(@" AND AI.PublishTime < '{0}'", endDate.Date.AddDays(1));

            string strRUM = string.Empty;
            if (articleCount == 0)//说明要领取查询出的所有文章
            { }
            else
                strRUM = $@" AND T1.RNUM <= {articleCount}";
            sbSql.AppendFormat($@"
                                SELECT  STUFF(( SELECT  ',' + CAST(T1.ArticleId AS VARCHAR(50))
                                                FROM    ( SELECT    AI.Id ArticleId ,                                   
                                                                    AI.PublishTime ,
                                                                    ROW_NUMBER() OVER ( ORDER BY AI.PublishTime DESC ) RNUM
                                                            FROM      BaseData2017.dbo.TouTiaoArticleInfo AI 
									  
                                                            WHERE     1 = 1 
                                                                    {strData}
                                                                    {strBegin}		
                                                                    {strEnd}
                                                                    AND AI.Id NOT IN (
                                                                    SELECT  BMAI.ArticleID
                                                                    FROM    dbo.BatchMediaArticle BMAI
                                                                    WHERE   1 = 1
                                                                            AND BMAI.CreateUserID = {currentUserID} 
                                                                            AND BMAI.MediaNumber = '{number}' 
                                                                            {strMediaType}
                                            )
                                                        ) T1
                                                WHERE   1=1 
                                                        {strRUM}
                                                FOR
                                                XML PATH('')
                                                ), 1, 1, '');
                            ");

            return sbSql.ToString();
        }
        protected string GetSouHuSQL_ArticleReceive(DateTime startDate, DateTime endDate, int articleCount, string number, int currentUserID)
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.UserName = '{number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} ";
            string strBegin = string.Empty;
            string strEnd = string.Empty;

            if (startDate != new DateTime(1900, 1, 1))
                strBegin = string.Format(@" AND AI.PublishTime >= '{0}'", startDate.Date);

            if (endDate != new DateTime(1900, 1, 1))
                strEnd = string.Format(@" AND AI.PublishTime < '{0}'", endDate.Date.AddDays(1));

            string strRUM = string.Empty;
            if (articleCount == 0)//说明要领取查询出的所有文章
            { }
            else
                strRUM = $@" AND T1.RNUM <= {articleCount}";
            sbSql.AppendFormat($@"
                                SELECT  STUFF(( SELECT  ',' + CAST(T1.ArticleId AS VARCHAR(50))
                                                FROM    ( SELECT    AI.RecID ArticleId ,                                   
                                                                    AI.PublishTime ,
                                                                    ROW_NUMBER() OVER ( ORDER BY AI.PublishTime DESC ) RNUM
                                                            FROM      BaseData2017.dbo.SouHuArticleInfo AI 
									  
                                                            WHERE     1 = 1 
                                                                    {strData}
                                                                    {strBegin}		
                                                                    {strEnd}
                                                                    AND AI.RecID NOT IN (
                                                                    SELECT  BMAI.ArticleID
                                                                    FROM    dbo.BatchMediaArticle BMAI
                                                                    WHERE   1 = 1
                                                                            AND BMAI.CreateUserID = {currentUserID} 
                                                                            AND BMAI.MediaNumber = '{number}' 
                                                                            {strMediaType}
                                            )
                                                        ) T1
                                                WHERE   1=1 
                                                        {strRUM}
                                                FOR
                                                XML PATH('')
                                                ), 1, 1, '');
                            ");

            return sbSql.ToString();
        }
        protected string GetInsertSql_BatchMediaArticle(string articleIds, int mediaType, string number, int currentUserID, int batchMediaID)
        {
            var sbSql = new StringBuilder();
            foreach (var id in articleIds.Split(',').ToList().Distinct())
            {
                sbSql.Append($@"
                            INSERT dbo.BatchMediaArticle
                                    ( BatchMediaID ,
                                      MediaType ,
                                      MediaName ,
                                      ArticleID ,
                                      CreateTime ,
                                      CreateUserID ,
                                      MediaNumber
                                    )
                            VALUES  ( {batchMediaID} , -- BatchMediaID - int
                                      {mediaType} , -- MediaType - int
                                      '{number}' , -- MediaName - varchar(200)
                                      {id} , -- ArticleID - int
                                      GETDATE() , -- CreateTime - datetime
                                      {currentUserID} , -- CreateUserID - int
                                      '{number}'  -- MediaNumber - varchar(100)
                                    )");
            }

            return sbSql.ToString();
        }
        /// <summary>
        /// ZLB 2017-11-28
        /// 查询打标签的文章数量
        /// </summary>
        /// <param name="BatchMediaID"></param>
        /// <returns></returns>
        public int SelectArticleCount(int BatchMediaID)
        {
            string strSql = $" SELECT COUNT (1) FROM BatchMediaArticle M  WHERE M.BatchMediaID={BatchMediaID}";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

    }
}
