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
    public class RecommendVideoQuery : PublishInfoQueryClient<RecommendSearchDto, ResRecommendVideo>
    {
        public RecommendVideoQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResRecommendVideo> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                    SELECT  HM.RecID ,
                            HM.SortNumber ,
		                    HM.ImageUrl,
		                    HM.VideoUrl,
                            MV.MediaID ,
                            MV.Name ,
                            MV.HeadIconURL ,
                            MV.FansCount ,
		                    IV.AverageCommentCount,
		                    IV.AveragePointCount
                    FROM    dbo.Home_Media AS HM WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Video AS MV WITH ( NOLOCK ) ON MV.MediaID = HM.MediaID AND MV.Status = 0
                            INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MV.MediaID = A.MediaID AND A.MediaType = HM.MediaType
							LEFT JOIN dbo.Interaction_Video AS IV WITH ( NOLOCK ) ON IV.MediaID = HM.MediaID
                    WHERE 1=1 ");

            sbSql.AppendFormat(@" AND HM.MediaType = {0}
                                AND EXISTS ( SELECT PD.RecID
                                             FROM   dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                             WHERE  PD.PubID = A.PubID
                                                    AND A.MediaType = PD.MediaType
                                                    AND PD.PublishStatus = {1} )
                                AND GETDATE() BETWEEN A.BeginTime AND A.EndTime
		                        AND HM.CategoryID = {2}", (int)MediaType.Video, (int)EnumPublishStatus.OnSold,
                RequetQuery.CategoryId);

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND (MV.Name LIKE '%{0}%' OR MV.Number LIKE '%{0}%' )", RequetQuery.MediaName.ToSqlFilter());
            }

            sbSql.AppendLine(@" ) T");
            var query = new PublishQuery<ResRecommendVideo>()
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