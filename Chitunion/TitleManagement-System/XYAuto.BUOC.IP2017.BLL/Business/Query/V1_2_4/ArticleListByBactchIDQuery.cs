using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;

namespace XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4
{
    public class ArticleListByBactchIDQuery : DataListQueryClient<ReqArticleListByBactchIDQueryDto, ResArticleQueryDto>
    {
        protected string GetMediaSQL()
        {
            var sbSql = new StringBuilder();

            sbSql.AppendFormat($@"
                                SELECT  AI.RecID ArticleId ,
                                        AI.Url ,
                                        AI.Title ,
                                        AI.ReadNum ,
                                        AI.LikeNum ,
                                        AI.ComNum ,
                                        AI.PublishTime ,
                                        AI.Resource
                                FROM    BaseData2017.dbo.ArticleInfo AI
                                WHERE   1 = 1
                                        AND AI.RecID IN (
                                        SELECT DISTINCT
                                                BMAI.ArticleID
                                        FROM    dbo.BatchMediaArticle BMAI
                                        WHERE   BMAI.BatchMediaID = {RequestQuery.BatchMediaID} )
                            ");
            //RequestQuery.PageSize = 50;
            return sbSql.ToString();
        }
        protected string GetWeiXinSQL()
        {
            var sbSql = new StringBuilder();
           
            sbSql.AppendFormat($@"
                                SELECT  AI.RecID ArticleId ,
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
                                        AND AI.RecID IN (
                                                            SELECT DISTINCT
                                                                    BMAI.ArticleID
                                                            FROM    dbo.BatchMediaArticle BMAI
                                                            WHERE   BMAI.BatchMediaID = {RequestQuery.BatchMediaID} )
                            ");            

            return sbSql.ToString();
        }
        protected string GetTouTiaoSQL()
        {
            var sbSql = new StringBuilder();
            
            sbSql.AppendFormat($@"
                                SELECT  AI.Id ArticleId ,
                                        AI.Url Url ,
                                        AI.Title ,
                                        AI.ReadNum ,
                                        AI.ComNum ,
                                        AI.PublishTime
                                FROM    BaseData2017.dbo.TouTiaoArticleInfo AI
                                WHERE   1 = 1 
                                        AND AI.Id IN (
                                                        SELECT DISTINCT
                                                                BMAI.ArticleID
                                                        FROM    dbo.BatchMediaArticle BMAI
                                                        WHERE   BMAI.BatchMediaID = {RequestQuery.BatchMediaID} )
                            ");            

            return sbSql.ToString();
        }
        protected string GetSouHuSQL()
        {
            var sbSql = new StringBuilder();
         
            sbSql.AppendFormat($@"
                                SELECT  AI.RecID ArticleId ,
                                        AI.Url Url ,
                                        AI.Title ,
                                        AI.ReadNum ,
                                        AI.ComNum ,
                                        AI.PublishTime
                                FROM    BaseData2017.dbo.SouHuArticleInfo AI
                                WHERE   1 = 1
                                        AND AI.RecID IN (
                                        SELECT DISTINCT
                                                BMAI.ArticleID
                                        FROM    dbo.BatchMediaArticle BMAI
                                        WHERE   BMAI.BatchMediaID = {RequestQuery.BatchMediaID} )  
                            ");

            return sbSql.ToString();
        }
        protected override Entities.Query.DataListQuery<ResArticleQueryDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            //sbSql.Append(GetMediaSQL());
            switch (RequestQuery.MediaType)
            {
                case (int)Entities.ENUM.ENUM.EnumMediaType.微信:
                    sbSql.Append(GetWeiXinSQL());
                    break;
                case (int)Entities.ENUM.ENUM.EnumMediaType.头条:
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
