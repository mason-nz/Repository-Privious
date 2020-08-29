using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.Query;
using XYAuto.BUOC.IP2017.Entities.Query;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4
{
    public class InputListMediaQuery : DataListQueryClient<ReqInputListMediaDto, ResInputListMediaDto>
    {
        protected string GetWeiXinSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.微信} MediaType ,
                                        WOA.RecID ,
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
                                        ( SELECT --TOP 1
                                                    BM.BatchMediaID
                                          FROM      dbo.BatchMedia BM
                                          WHERE     BM.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                                    AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体}
                                                    AND BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信}
                                                    AND BM.MediaNumber = WOA.WxNumber
                                                    AND BM.CreateUserID = {RequestQuery.CurrentUserID}
                                        ) BatchMediaID
                                FROM    Chitunion2017.dbo.Weixin_OAuth WOA
								LEFT JOIN (SELECT  MLR.MediaNumber
                                                        FROM    dbo.MediaLabelResult MLR
                                                        WHERE   MLR.MediaType = 14001
                                                                AND MLR.Status = 0
                                                                GROUP BY MLR.MediaNumber)MLR ON MLR.MediaNumber=WOA.WxNumber
								LEFT JOIN (SELECT  BM.MediaNumber
                                                            FROM    dbo.BatchMedia BM
                                                            WHERE   BM.MediaType = 14001
                                                                    AND BM.Status > 1001
                                                                    GROUP BY BM.MediaNumber ) BM ON BM.MediaNumber=WOA.WxNumber
                                WHERE   1 = 1
                                        AND WOA.Status = 0
                                        AND WOA.WxNumber <> ''
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"         
                                  AND WOA.RecID IN ( SELECT    MC.WxID
                                                     FROM      Chitunion2017.dbo.MediaCategory MC
                                                     WHERE     MC.MediaType = 14001
                                                               AND MC.CategoryID = {RequestQuery.DictId} )");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
            {
                sbSql.AppendFormat($@"AND (WOA.NickName LIKE '%{RequestQuery.Name}%' OR WOA.WxNumber LIKE '%{RequestQuery.Name}%')");
            }

            #region V2.0.3已录未录标签 
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT  1
            //                                            FROM    dbo.BatchMedia BM
            //                                            WHERE   BM.MediaType = 14001
            //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
            //                                                    AND BM.MediaNumber = WOA.WxNumber )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                     FROM   dbo.BatchMedia BM
            //                                     WHERE  BM.MediaType = 14001
            //                                            AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                            AND BM.MediaNumber = WOA.WxNumber )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@" AND MLR.MediaNumber IS NULL AND BM.MediaNumber IS NULL ");
                //sbSql.AppendFormat($@"
                //                    AND NOT EXISTS ( SELECT  1
                //                                        FROM    dbo.MediaLabelResult MLR
                //                                        WHERE   MLR.MediaType = 14001
                //                                                AND MLR.Status = 0
                //                                                AND MLR.MediaNumber = WOA.WxNumber  )");
                //sbSql.AppendFormat($@"
                //                        AND NOT EXISTS ( SELECT  1
                //                                            FROM    dbo.BatchMedia BM
                //                                            WHERE   BM.MediaType = 14001
                //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                //                                                    AND BM.MediaNumber = WOA.WxNumber )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@" AND (MLR.MediaNumber IS NOT NULL OR BM.MediaNumber IS NOT NULL) ");
                //    sbSql.AppendFormat($@"
                //                        AND (EXISTS ( SELECT 1 
                //                                         FROM    dbo.MediaLabelResult MLR
                //                                        WHERE   MLR.MediaType = 14001
                //                                                    AND MLR.Status = 0
                //                                                AND MLR.MediaNumber = WOA.WxNumber  )");

                //    sbSql.AppendFormat($@"
                //                              OR  EXISTS ( SELECT 1
                //                                         FROM   dbo.BatchMedia BM
                //                                         WHERE  BM.MediaType = 14001
                //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                //                                                AND BM.MediaNumber = WOA.WxNumber ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   Chitunion2017.dbo.Media_Weixin MW
                                                            JOIN Chitunion2017.dbo.UserInfo UI ON MW.CreateUserID = UI.UserID
                                                     WHERE  MW.Status = 0
                                                            AND MW.AuditStatus = 43002
                                                            AND MW.Number = WOA.WxNumber
                                                            AND UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND EXISTS ( SELECT 1
                                                 FROM   Chitunion2017.dbo.Media_Weixin MW
                                                        JOIN Chitunion2017.dbo.UserInfo UI ON MW.CreateUserID = UI.UserID
                                                 WHERE  MW.Status = 0
                                                        AND MW.AuditStatus = 43002
                                                        AND MW.Number = WOA.WxNumber
                                                        AND UI.Source = 3001 )");

            if (RequestQuery.HasArticleType != -2)
            {
                switch (RequestQuery.HasArticleType)
                {
                    case (int)Enum.EnumHasArticleType.最近30天有文章:
                        sbSql.AppendFormat($@"
                                    AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
                                                         WHERE  AI.WxNum = WOA.WxNumber
                                                                AND AI.PubTime >= '{DateTime.Now.Date.AddDays(-29)}'
                                                                AND AI.PubTime < '{DateTime.Now.Date.AddDays(1)}'  )");
                        break;
                    case (int)Enum.EnumHasArticleType.最近7天有文章:
                        sbSql.AppendFormat($@"
                                    AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
                                                         WHERE  AI.WxNum = WOA.WxNumber
                                                                AND AI.PubTime >= '{DateTime.Now.Date.AddDays(-6)}'
                                                                AND AI.PubTime < '{DateTime.Now.Date.AddDays(1)}'  )");
                        break;
                    case (int)Enum.EnumHasArticleType.无文章:
                        sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
                                                         WHERE  AI.WxNum = WOA.WxNumber)");
                        break;
                    default:
                        break;
                }
            }

            return sbSql.ToString();
        }
        #region 注释
        //protected string GetWeiXinSQL()
        //{
        //    var sbSql = new StringBuilder();
        //    sbSql.AppendFormat($@"
        //                        SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.微信} MediaType ,
        //                                WOA.RecID ,
        //                                WOA.NickName Name ,
        //                                WOA.HeadImg , 
        //                                WOA.WxNumber Number ,
        //                                WOA.CreateTime ,
        //                                WOA.FansCount StatisticsCount ,
        //                                ( SELECT    AVG(VWAI.ReadNum)
        //                                  FROM      BaseData2017.dbo.v_Weixin_ArticleInfo VWAI
        //                                  WHERE     VWAI.Location = 1
        //                                            AND VWAI.WxNum = WOA.WxNumber
        //                                ) ReadCount ,
        //                                ( SELECT --TOP 1
        //                                            BM.BatchMediaID
        //                                  FROM      dbo.BatchMedia BM
        //                                  WHERE     BM.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
        //                                            AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体}
        //                                            AND BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信}
        //                                            AND BM.MediaNumber = WOA.WxNumber
        //                                            AND BM.CreateUserID = {RequestQuery.CurrentUserID}
        //                                ) BatchMediaID
        //                        FROM    Chitunion2017.dbo.Weixin_OAuth WOA
        //                        WHERE   1 = 1
        //                                AND WOA.Status = 0
        //                                AND WOA.WxNumber <> ''
        //                    ");
        //    if (RequestQuery.DictId != -2)
        //        sbSql.AppendFormat($@"         
        //                          AND WOA.RecID IN ( SELECT    MC.WxID
        //                                             FROM      Chitunion2017.dbo.MediaCategory MC
        //                                             WHERE     MC.MediaType = 14001
        //                                                       AND MC.CategoryID = {RequestQuery.DictId} )");

        //    if (!string.IsNullOrEmpty(RequestQuery.Name))
        //    {
        //        sbSql.AppendFormat($@"AND (WOA.NickName LIKE '%{RequestQuery.Name}%' OR WOA.WxNumber LIKE '%{RequestQuery.Name}%')");
        //    }

        //    #region V2.0.3已录未录标签 
        //    //if (RequestQuery.LabelStatus == 0)//未录标签
        //    //    sbSql.AppendFormat($@"
        //    //                        AND NOT EXISTS ( SELECT  1
        //    //                                            FROM    dbo.BatchMedia BM
        //    //                                            WHERE   BM.MediaType = 14001
        //    //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
        //    //                                                    AND BM.MediaNumber = WOA.WxNumber )");
        //    //else if (RequestQuery.LabelStatus == 1)//已录标签
        //    //    sbSql.AppendFormat($@"
        //    //                        AND EXISTS ( SELECT 1
        //    //                                     FROM   dbo.BatchMedia BM
        //    //                                     WHERE  BM.MediaType = 14001
        //    //                                            AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
        //    //                                            AND BM.MediaNumber = WOA.WxNumber )");
        //    #endregion
        //    if (RequestQuery.LabelStatus == 0)//未录标签
        //    {
        //        sbSql.AppendFormat($@"
        //                            AND NOT EXISTS ( SELECT  1
        //                                                FROM    dbo.MediaLabelResult MLR
        //                                                WHERE   MLR.MediaType = 14001
        //                                                        AND MLR.Status = 0
        //                                                        AND MLR.MediaNumber = WOA.WxNumber  )");
        //        sbSql.AppendFormat($@"
        //                                AND NOT EXISTS ( SELECT  1
        //                                                    FROM    dbo.BatchMedia BM
        //                                                    WHERE   BM.MediaType = 14001
        //                                                            AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
        //                                                            AND BM.MediaNumber = WOA.WxNumber )");
        //    }
        //    else if (RequestQuery.LabelStatus == 1)//已录标签
        //    { 
        //        sbSql.AppendFormat($@"
        //                            AND (EXISTS ( SELECT 1 
        //                                             FROM    dbo.MediaLabelResult MLR
        //                                            WHERE   MLR.MediaType = 14001
        //                                                        AND MLR.Status = 0
        //                                                    AND MLR.MediaNumber = WOA.WxNumber  )");

        //        sbSql.AppendFormat($@"
        //                                  OR  EXISTS ( SELECT 1
        //                                             FROM   dbo.BatchMedia BM
        //                                             WHERE  BM.MediaType = 14001
        //                                                    AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
        //                                                    AND BM.MediaNumber = WOA.WxNumber ))");
        //    }

        //    if (RequestQuery.SelfDoBusiness == 0)//非自营
        //        sbSql.AppendFormat(@"
        //                            AND NOT EXISTS ( SELECT 1
        //                                             FROM   Chitunion2017.dbo.Media_Weixin MW
        //                                                    JOIN Chitunion2017.dbo.UserInfo UI ON MW.CreateUserID = UI.UserID
        //                                             WHERE  MW.Status = 0
        //                                                    AND MW.AuditStatus = 43002
        //                                                    AND MW.Number = WOA.WxNumber
        //                                                    AND UI.Source = 3001 )");
        //    else if (RequestQuery.SelfDoBusiness == 1)//自营
        //        sbSql.AppendFormat(@"
        //                            AND EXISTS ( SELECT 1
        //                                         FROM   Chitunion2017.dbo.Media_Weixin MW
        //                                                JOIN Chitunion2017.dbo.UserInfo UI ON MW.CreateUserID = UI.UserID
        //                                         WHERE  MW.Status = 0
        //                                                AND MW.AuditStatus = 43002
        //                                                AND MW.Number = WOA.WxNumber
        //                                                AND UI.Source = 3001 )");

        //    if (RequestQuery.HasArticleType != -2)
        //    {
        //        switch (RequestQuery.HasArticleType)
        //        {
        //            case (int)Enum.EnumHasArticleType.最近30天有文章:
        //                sbSql.AppendFormat($@"
        //                            AND EXISTS ( SELECT 1
        //                                                 FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
        //                                                 WHERE  AI.WxNum = WOA.WxNumber
        //                                                        AND AI.PubTime >= '{DateTime.Now.Date.AddDays(-29)}'
        //                                                        AND AI.PubTime < '{DateTime.Now.Date.AddDays(1)}'  )");
        //                break;
        //            case (int)Enum.EnumHasArticleType.最近7天有文章:
        //                sbSql.AppendFormat($@"
        //                            AND EXISTS ( SELECT 1
        //                                                 FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
        //                                                 WHERE  AI.WxNum = WOA.WxNumber
        //                                                        AND AI.PubTime >= '{DateTime.Now.Date.AddDays(-6)}'
        //                                                        AND AI.PubTime < '{DateTime.Now.Date.AddDays(1)}'  )");
        //                break;
        //            case (int)Enum.EnumHasArticleType.无文章:
        //                sbSql.AppendFormat($@"
        //                            AND NOT EXISTS ( SELECT 1
        //                                                 FROM   BaseData2017.dbo.Weixin_ArticleInfo AI
        //                                                 WHERE  AI.WxNum = WOA.WxNumber)");
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return sbSql.ToString();
        //}
        #endregion

        protected string GetAPPSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.APP} MediaType ,
                                        MBPC.Name ,
                                        MBPC.HeadIconURL HeadImg ,
                                        MBPC.CreateTime ,
                                        MBPC.DailyLive StatisticsCount 
                                FROM    Chitunion2017.dbo.Media_BasePCAPP MBPC
                                WHERE   MBPC.Status = 0 
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"         
                                    AND MBPC.RecID IN ( SELECT  MC.WxID
                                                        FROM    Chitunion2017.dbo.MediaCategory MC
                                                        WHERE   MC.MediaType = 14002
                                                                AND MC.CategoryID = {RequestQuery.DictId} ) ");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MBPC.Name LIKE '%{RequestQuery.Name}%'");

            #region V2.0.3未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = 14002
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MBPC.Name )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = 14002
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MBPC.Name )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = 14002
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MBPC.Name )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = 14002
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MBPC.Name )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = 14002
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MBPC.Name )");

                sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = 14002
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MBPC.Name ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   Chitunion2017.dbo.Media_PCAPP MP
                                                            JOIN Chitunion2017.dbo.UserInfo UI ON MP.CreateUserID = UI.UserID
                                                     WHERE  MP.Status = 0
                                                            AND MP.AuditStatus = 48002
                                                            AND MP.Name = MBPC.Name
                                                            AND UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND EXISTS ( SELECT 1
                                                 FROM   Chitunion2017.dbo.Media_PCAPP MP
                                                        JOIN Chitunion2017.dbo.UserInfo UI ON MP.CreateUserID = UI.UserID
                                                 WHERE  MP.Status = 0
                                                        AND MP.AuditStatus = 48002
                                                        AND MP.Name = MBPC.Name
                                                        AND UI.Source = 3001 )");
            return sbSql.ToString();
        }
        protected string GetVideoSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.视频} MediaType ,
                                        MV.Name ,
                                        MV.HeadIconURL HeadImg ,
                                        MV.CreateTime ,
                                        MV.FansCount StatisticsCount
                                FROM    Chitunion2017.dbo.Media_Video MV
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MV.Status = 0
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"AND MV.CategoryID = {RequestQuery.DictId} ");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MV.Name LIKE '%{RequestQuery.Name}%'");

            #region V2.0.2未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MV.Name )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
            //                                                AND BM.MediaName = MV.Name )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
                                                            AND MLR.Status = 0     
                                                            AND MLR.MediaName = MV.Name )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MV.Name )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
                                                            AND MLR.Status = 0     
                                                            AND MLR.MediaName = MV.Name )");

                sbSql.AppendFormat($@"
                                    OR EXISTS ( SELECT 1
                                                     FROM   dbo.BatchMedia BM
                                                     WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.视频}
                                                            AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}     
                                                            AND BM.MediaName = MV.Name ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND MV.CreateUserID NOT IN ( SELECT UI.UserID
                                                                 FROM   Chitunion2017.dbo.UserInfo UI
                                                                 WHERE  UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND MV.CreateUserID IN ( SELECT UI.UserID
                                                             FROM   Chitunion2017.dbo.UserInfo UI
                                                             WHERE  UI.Source = 3001 )");
            return sbSql.ToString();
        }
        protected string GetBroadcastSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.直播} MediaType ,
                                        MB.Name ,
                                        MB.HeadIconURL HeadImg ,
                                        MB.CreateTime ,
                                        MB.FansCount StatisticsCount
                                FROM    Chitunion2017.dbo.Media_Broadcast MB
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MB.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MB.Status = 0
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"AND MB.CategoryID = {RequestQuery.DictId}  ");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MB.Name LIKE '%{RequestQuery.Name}%'");

            #region V2.0.3未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MB.Name )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MB.Name )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MB.Name )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MB.Name )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MB.Name )");
                sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.直播}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MB.Name ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND MB.CreateUserID NOT IN ( SELECT UI.UserID
                                                                 FROM   Chitunion2017.dbo.UserInfo UI
                                                                 WHERE  UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND MB.CreateUserID IN ( SELECT UI.UserID
                                                             FROM   Chitunion2017.dbo.UserInfo UI
                                                             WHERE  UI.Source = 3001 )");
            return sbSql.ToString();
        }
        protected string GetWeiboSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博} MediaType ,
                                        MV.Name ,
                                        MV.HeadIconURL HeadImg ,
                                        MV.CreateTime ,
                                        MV.FansCount StatisticsCount
                                FROM    Chitunion2017.dbo.Media_Weibo MV
                                        JOIN Chitunion2017.dbo.UserRole UR ON UR.UserID = MV.CreateUserID
                                WHERE   UR.RoleID = 'SYS001RL00005'
                                        AND MV.Status = 0
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"AND MV.CategoryID = {RequestQuery.DictId}  ");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MV.Name LIKE '%{RequestQuery.Name}%'");

            #region V2.0.3未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MV.Name )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MV.Name )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MV.Name )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MV.Name )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MV.Name )");

                sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.新浪微博}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MV.Name ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND MV.CreateUserID NOT IN ( SELECT UI.UserID
                                                                 FROM   Chitunion2017.dbo.UserInfo UI
                                                                 WHERE  UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND MV.CreateUserID IN ( SELECT UI.UserID
                                                             FROM   Chitunion2017.dbo.UserInfo UI
                                                             WHERE  UI.Source = 3001 )");
            return sbSql.ToString();
        }
        protected string GetTouTiaoSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.头条} MediaType ,
                                        MT.UserName Name ,
                                        MT.HeadImg ,
                                        MT.CreateTime ,
                                        MT.FansCount StatisticsCount,
                                        ( SELECT TOP 1
                                                    BM.BatchMediaID
                                          FROM      dbo.BatchMedia BM
                                          WHERE     BM.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                                    AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体}
                                                    AND BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                    AND BM.MediaName = MT.UserName
                                                    AND BM.CreateUserID = {RequestQuery.CurrentUserID}
                                        ) BatchMediaID
                                FROM    Chitunion2017.dbo.Media_TouTiao MT
                                WHERE   1 = 1
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"         
                                  AND MT.MediaID IN ( SELECT    MC.WxID
                                                     FROM      Chitunion2017.dbo.MediaCategory MC
                                                     WHERE     MC.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                               AND MC.CategoryID = {RequestQuery.DictId} )");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MT.UserName LIKE '%{RequestQuery.Name}%'");

            #region V2.0.3未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MT.UserName )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MT.UserName )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MT.UserName )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MT.UserName )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MT.UserName )");

                sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MT.UserName ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND MT.CreateUserID NOT IN ( SELECT UI.UserID
                                                                 FROM   Chitunion2017.dbo.UserInfo UI
                                                                 WHERE  UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND MT.CreateUserID IN ( SELECT UI.UserID
                                                             FROM   Chitunion2017.dbo.UserInfo UI
                                                             WHERE  UI.Source = 3001 )");

            if (RequestQuery.HasArticleType != -2)
            {
                switch (RequestQuery.HasArticleType)
                {
                    case (int)Enum.EnumHasArticleType.最近30天有文章:
                        sbSql.AppendFormat($@"
                                            AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.TouTiaoArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName
                                                                AND AI.PublishTime >= '{DateTime.Now.Date.AddDays(-29)}'
                                                                AND AI.PublishTime < '{DateTime.Now.Date.AddDays(1)}' )");
                        break;
                    case (int)Enum.EnumHasArticleType.最近7天有文章:
                        sbSql.AppendFormat($@"
                                            AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.TouTiaoArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName
                                                                AND AI.PublishTime >= '{DateTime.Now.Date.AddDays(-6)}'
                                                                AND AI.PublishTime < '{DateTime.Now.Date.AddDays(1)}' )");
                        break;
                    case (int)Enum.EnumHasArticleType.无文章:
                        sbSql.AppendFormat($@"
                                            AND NOT EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.TouTiaoArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName )");
                        break;
                    default:
                        break;
                }
            }
            return sbSql.ToString();
        }
        protected string GetSouHuSQL()
        {
            var sbSql = new StringBuilder();
            sbSql.AppendFormat($@"
                                SELECT  {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} MediaType ,
                                        MT.UserName Name ,
                                        MT.HeadImg ,
                                        MT.CreateTime ,
                                        MT.FansCount StatisticsCount,
                                        ( SELECT TOP 1
                                                    BM.BatchMediaID
                                          FROM      dbo.BatchMedia BM
                                          WHERE     BM.Status = {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打}
                                                    AND BM.TaskType = {(int)Entities.ENUM.ENUM.EnumTaskType.媒体}
                                                    AND BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                    AND BM.MediaName = MT.UserName
                                                    AND BM.CreateUserID = {RequestQuery.CurrentUserID}
                                        ) BatchMediaID
                                FROM    Chitunion2017.dbo.Media_SouHu MT
                                WHERE   1 = 1
                            ");
            if (RequestQuery.DictId != -2)
                sbSql.AppendFormat($@"         
                                  AND MT.MediaID IN ( SELECT    MC.WxID
                                                     FROM      Chitunion2017.dbo.MediaCategory MC
                                                     WHERE     MC.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                               AND MC.CategoryID = {RequestQuery.DictId} )");

            if (!string.IsNullOrEmpty(RequestQuery.Name))
                sbSql.AppendFormat($@"AND MT.UserName LIKE '%{RequestQuery.Name}%'");

            #region V2.0.3未录已录标签
            //if (RequestQuery.LabelStatus == 0)//未录标签
            //    sbSql.AppendFormat($@"
            //                        AND NOT EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MT.UserName )");
            //else if (RequestQuery.LabelStatus == 1)//已录标签
            //    sbSql.AppendFormat($@"
            //                        AND EXISTS ( SELECT 1
            //                                         FROM   dbo.BatchMedia BM
            //                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
            //                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
            //                                                AND BM.MediaName = MT.UserName )");
            #endregion
            if (RequestQuery.LabelStatus == 0)//未录标签
            {
                sbSql.AppendFormat($@"
                                    AND NOT EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MT.UserName )");

                sbSql.AppendFormat($@"
                                        AND NOT EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MT.UserName )");
            }
            else if (RequestQuery.LabelStatus == 1)//已录标签
            {
                sbSql.AppendFormat($@"
                                    AND (EXISTS ( SELECT 1
                                                     FROM   dbo.MediaLabelResult MLR
                                                     WHERE  MLR.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                            AND MLR.Status = 0 
                                                            AND MLR.MediaName = MT.UserName )");

                sbSql.AppendFormat($@"
                                        OR EXISTS ( SELECT 1
                                                         FROM   dbo.BatchMedia BM
                                                         WHERE  BM.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐}
                                                                AND BM.Status > {(int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打} 
                                                                AND BM.MediaName = MT.UserName ))");
            }

            if (RequestQuery.SelfDoBusiness == 0)//非自营
                sbSql.AppendFormat(@"
                                    AND MT.CreateUserID NOT IN ( SELECT UI.UserID
                                                                 FROM   Chitunion2017.dbo.UserInfo UI
                                                                 WHERE  UI.Source = 3001 )");
            else if (RequestQuery.SelfDoBusiness == 1)//自营
                sbSql.AppendFormat(@"
                                    AND MT.CreateUserID IN ( SELECT UI.UserID
                                                             FROM   Chitunion2017.dbo.UserInfo UI
                                                             WHERE  UI.Source = 3001 )");

            if (RequestQuery.HasArticleType != -2)
            {
                switch (RequestQuery.HasArticleType)
                {
                    case (int)Enum.EnumHasArticleType.最近30天有文章:
                        sbSql.AppendFormat($@"
                                            AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.SouHuArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName
                                                                AND AI.PublishTime >= '{DateTime.Now.Date.AddDays(-29)}'
                                                                AND AI.PublishTime < '{DateTime.Now.Date.AddDays(1)}' )");
                        break;
                    case (int)Enum.EnumHasArticleType.最近7天有文章:
                        sbSql.AppendFormat($@"
                                            AND EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.SouHuArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName
                                                                AND AI.PublishTime >= '{DateTime.Now.Date.AddDays(-6)}'
                                                                AND AI.PublishTime < '{DateTime.Now.Date.AddDays(1)}' )");
                        break;
                    case (int)Enum.EnumHasArticleType.无文章:
                        sbSql.AppendFormat($@"
                                            AND NOT EXISTS ( SELECT 1
                                                         FROM   BaseData2017.dbo.SouHuArticleInfo AI
                                                         WHERE  AI.UserName = MT.UserName )");
                        break;
                    default:
                        break;
                }
            }
            return sbSql.ToString();
        }
        private string GetOrderBy(int orderBy)
        {
            var orderByStr = " CreateTime DESC ";
            if (RequestQuery.MediaType == (int)Entities.ENUM.ENUM.EnumMediaType.微信)
                orderByStr = " CreateTime,RecID DESC ";
            var orderDictionary = new Dictionary<int, string>()
            {
                {1001," StatisticsCount DESC "},
                {1002," StatisticsCount ASC "},
                {2001," ReadCount DESC "},
                {2002," ReadCount ASC "},
                {3001," CreateTime DESC "},
                {3002," CreateTime ASC "}
            };

            var value = orderDictionary.FirstOrDefault(s => s.Key == orderBy);
            return value.Value ?? orderByStr;
        }
        protected override DataListQuery<ResInputListMediaDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            switch (RequestQuery.MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    sbSql.Append(GetWeiXinSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.APP:
                    sbSql.Append(GetAPPSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.视频:
                    sbSql.Append(GetVideoSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.直播:
                    sbSql.Append(GetBroadcastSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.新浪微博:
                    sbSql.Append(GetWeiboSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
                    sbSql.Append(GetTouTiaoSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.搜狐:
                    sbSql.Append(GetSouHuSQL());
                    break;
                default:
                    sbSql.Append(GetWeiXinSQL());
                    break;
            }

            sbSql.AppendLine(@") T");
            var query = new DataListQuery<ResInputListMediaDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = GetOrderBy(RequestQuery.OrderBy),
                PageSize = RequestQuery.PageSize,
                PageIndex = RequestQuery.PageIndex
            };
            return query;
        }
    }
}
