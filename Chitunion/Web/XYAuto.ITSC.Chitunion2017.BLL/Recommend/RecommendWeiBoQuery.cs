using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend
{
    public class RecommendWeiBoQuery : PublishInfoQueryClient<RecommendSearchDto, ResRecommendWeiBo>
    {
        public RecommendWeiBoQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResRecommendWeiBo> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                    SELECT  HM.RecID ,
                            HM.SortNumber ,
                            MW.MediaID ,
                            MW.Name ,
                            MW.HeadIconURL ,
                            MW.FansCount ,
		                    IW.AverageCommentCount,
		                     IW.AveragePointCount,
							IW.AverageForwardCount
                    FROM    dbo.Home_Media AS HM WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Weibo AS MW WITH ( NOLOCK ) ON MW.MediaID = HM.MediaID AND MW.Status = 0
							INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MW.MediaID = A.MediaID AND A.MediaType = HM.MediaType
							LEFT JOIN dbo.Interaction_Weibo AS IW WITH ( NOLOCK ) ON IW.MediaID = HM.MediaID
                    WHERE 1=1 ");

            sbSql.AppendFormat(@" AND HM.MediaType = {0}
                                AND EXISTS ( SELECT PD.RecID
                                             FROM   dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                             WHERE  PD.PubID = A.PubID
                                                    AND A.MediaType = PD.MediaType
                                                    AND PD.PublishStatus = {1} )
                                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime
		                        AND HM.CategoryID = {2}", (int)MediaType.WeiBo, (int)EnumPublishStatus.OnSold,
                RequetQuery.CategoryId);

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MW.Name LIKE '%{0}%'", RequetQuery.MediaName.ToSqlFilter());
            }
            sbSql.AppendLine(@" ) T");
            var query = new PublishQuery<ResRecommendWeiBo>()
            {
                OrderBy = " SortNumber DESC,RecID DESC ",
                StrSql = sbSql.ToString(),
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}