using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4
{
    public class ArticleQuery : DataListQueryClient<ReqArticleQueryOrReciveDto, ResArticleQueryDto>
    {
        protected string GetMediaSQL()
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;
            //if (RequestQuery.StartDate == new DateTime(1900, 1, 1) && RequestQuery.EndDate == new DateTime(1900, 1, 1))
            //{
            //    //如果发布日期未选择则直接查询发布日期默认为最近7天的文章的按发布时间倒序的前30篇文章
            //    topCon = " TOP 30 ";
            //}
            //如果RequestQuery.ArticleCount为0则不限制查询数据数量
            if (RequestQuery.ArticleCount > 0)
                topCon = $@" TOP {RequestQuery.ArticleCount} ";
            string strResource = string.Empty;
            string strData = string.Empty;
            string strMediaType = string.Empty;
            switch (RequestQuery.Resource)
            {
                case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                    strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.微信} ";
                    strData = $@" AND AI.DataId = '{RequestQuery.Number}' ";
                    strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信} ";
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                    strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.今日头条} ";
                    strData = $@" AND AI.DataName = '{RequestQuery.Number}' ";
                    strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信} ";
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    strResource = $@" AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.搜狐} ";
                    strData = $@" AND AI.DataName = '{RequestQuery.Number}' ";
                    strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} ";
                    break;
            }
            sbSql.AppendFormat($@"
                                SELECT  {topCon} 
                                        AI.RecID ArticleId ,
                                        AI.Url ,
                                        AI.Title ,
                                        AI.ReadNum ,
                                        AI.LikeNum ,
                                        AI.ComNum ,
                                        AI.PublishTime
                                FROM    BaseData2017.dbo.ArticleInfo AI
                                WHERE   1=1 
                                        --AND AI.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.微信}
                                        {strResource} 
                                        --AND AI.DataId = '{RequestQuery.Number}' 
                                        {strData} 
                                        AND AI.RecID NOT IN ( SELECT    BMAI.ArticleID
                                                              FROM      dbo.BatchMediaArticle BMAI
                                                              WHERE     1=1 
                                                                        --AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信}
                                                                        {strMediaType} 
                                                                        AND BMAI.CreateUserID = {RequestQuery.CurrentUserID} 
                                                                        AND BMAI.MediaNumber = '{RequestQuery.Number}' )
                            ");

            //if (RequestQuery.StartDate == new DateTime(1900, 1, 1) && RequestQuery.EndDate == new DateTime(1900, 1, 1))
            //{
            //    //如果发布日期未选择则直接查询发布日期默认为最近7天的文章的按发布时间倒序的前30篇文章
            //    sbSql.AppendFormat(@" AND AI.PublishTime >= '{0}'", DateTime.Now.Date.AddDays(-7));
            //    sbSql.AppendFormat(@" AND AI.PublishTime < '{0}'", DateTime.Now.Date);
            //}

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

            return sbSql.ToString();
        }
        protected string GetWeiXinSQL()
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            if (RequestQuery.ArticleCount > 0)
                topCon = $@" TOP {RequestQuery.ArticleCount} ";
            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.WxNum = '{RequestQuery.Number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.微信} ";
            sbSql.AppendFormat($@"
                                SELECT  {topCon}
                                        AI.RecID ArticleId ,
                                        AI.ContentURL Url ,
                                        AI.Title ,
                                        AI1.ReadNum ,
                                        AI1.LikeNum ,
                                        AI1.ComNum ,
                                        AI.PubTime PublishTime
                                FROM    BaseData2017.dbo.Weixin_ArticleInfo AI
                                        LEFT JOIN BaseData2017.dbo.ArticleInfo AI1 ON AI1.Url = AI.ContentURL
                                                                                      AND AI1.Resource = {(int)Entities.ENUM.ENUM.EnumResourceType.微信}
                                WHERE   1 = 1
                                        {strData} 
                                        AND AI.RecID NOT IN ( SELECT    BMAI.ArticleID
                                                              FROM      dbo.BatchMediaArticle BMAI
                                                              WHERE     1=1 
                                                                        {strMediaType} 
                                                                        AND BMAI.CreateUserID = {RequestQuery.CurrentUserID} 
                                                                        AND BMAI.MediaNumber = '{RequestQuery.Number}' )
                            ");

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PubTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PubTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

            return sbSql.ToString();
        }
        protected string GetTouTiaoSQL()
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            if (RequestQuery.ArticleCount > 0)
                topCon = $@" TOP {RequestQuery.ArticleCount} ";
            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.UserName = '{RequestQuery.Number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.头条} ";
            sbSql.AppendFormat($@"
                                SELECT  {topCon}
                                        AI.Id ArticleId ,
                                        AI.Url Url ,
                                        AI.Title ,
                                        AI.ReadNum ,
                                        AI.ComNum ,
                                        AI.PublishTime
                                FROM    BaseData2017.dbo.TouTiaoArticleInfo AI
                                WHERE   1 = 1
                                        {strData} 
                                        AND AI.Id NOT IN ( SELECT    BMAI.ArticleID
                                                              FROM      dbo.BatchMediaArticle BMAI
                                                              WHERE     1=1 
                                                                        {strMediaType} 
                                                                        AND BMAI.CreateUserID = {RequestQuery.CurrentUserID} 
                                                                        AND BMAI.MediaNumber = '{RequestQuery.Number}' )
                            ");

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

            return sbSql.ToString();
        }
        protected string GetSouHuSQL()
        {
            var sbSql = new StringBuilder();
            string topCon = string.Empty;

            if (RequestQuery.ArticleCount > 0)
                topCon = $@" TOP {RequestQuery.ArticleCount} ";
            string strData = string.Empty;
            string strMediaType = string.Empty;
            strData = $@" AND AI.UserName = '{RequestQuery.Number}' ";
            strMediaType = $@" AND BMAI.MediaType = {(int)Entities.ENUM.ENUM.EnumMediaType.搜狐} ";
            sbSql.AppendFormat($@"
                                        SELECT  {topCon} 
                                                AI.RecID ArticleId ,
                                                AI.Url Url ,
                                                AI.Title ,
                                                AI.ReadNum ,
                                                AI.ComNum ,
                                                AI.PublishTime
                                        FROM    BaseData2017.dbo.SouHuArticleInfo AI
                                        WHERE   1 = 1
                                                {strData} 
                                        AND AI.RecID NOT IN ( SELECT    BMAI.ArticleID
                                                              FROM      dbo.BatchMediaArticle BMAI
                                                              WHERE     1=1 
                                                                        {strMediaType} 
                                                                        AND BMAI.CreateUserID = {RequestQuery.CurrentUserID} 
                                                                        AND BMAI.MediaNumber = '{RequestQuery.Number}' )
                            ");

            if (RequestQuery.StartDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime >= '{0}'", RequestQuery.StartDate.Date);

            if (RequestQuery.EndDate != new DateTime(1900, 1, 1))
                sbSql.AppendFormat(@" AND AI.PublishTime < '{0}'", RequestQuery.EndDate.Date.AddDays(1));

            return sbSql.ToString();
        }
        protected override Entities.Query.DataListQuery<ResArticleQueryDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            //sbSql.Append(GetMediaSQL()); 
            switch (RequestQuery.Resource)
            {
                case (int)Entities.ENUM.ENUM.EnumResourceType.微信:
                    sbSql.Append(GetWeiXinSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.今日头条:
                    sbSql.Append(GetTouTiaoSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumResourceType.搜狐:
                    sbSql.Append(GetSouHuSQL());
                    break;
            }

            sbSql.AppendLine(@") T");
            var query = new Entities.Query.DataListQuery<ResArticleQueryDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " PublishTime DESC",
                PageSize = RequestQuery.PageSize,
                PageIndex = RequestQuery.PageIndex
            };
            return query;
        }
    }
}
