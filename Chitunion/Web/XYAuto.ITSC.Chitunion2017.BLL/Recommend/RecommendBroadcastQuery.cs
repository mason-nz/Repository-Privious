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
    public class RecommendBroadcastQuery : PublishInfoQueryClient<RecommendSearchDto, ResRecommendBroadcast>
    {
        public RecommendBroadcastQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResRecommendBroadcast> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"

                    SELECT   HM.RecID ,
                            HM.SortNumber ,
                            MB.MediaID ,
                            MB.Name ,
		                    MB.Number,
		                    MB.Sex,
                            MB.HeadIconURL ,
                            MB.FansCount ,
		                    IW.CumulateReward
                    FROM    dbo.Home_Media AS HM WITH ( NOLOCK )
                            INNER JOIN dbo.Media_Broadcast AS MB WITH ( NOLOCK ) ON MB.MediaID = HM.MediaID AND MB.Status = 0
                            INNER JOIN dbo.Publish_BasicInfo AS A WITH ( NOLOCK ) ON MB.MediaID = A.MediaID AND A.MediaType = HM.MediaType
                            LEFT JOIN dbo.Interaction_Broadcast AS IW WITH ( NOLOCK ) ON IW.MediaID = HM.MediaID
                    WHERE   1 = 1       ");

            sbSql.AppendFormat(@" AND HM.MediaType = {0}
                            AND EXISTS ( SELECT PD.RecID
                                         FROM   dbo.Publish_DetailInfo AS PD WITH ( NOLOCK )
                                         WHERE  PD.PubID = A.PubID
                                                AND A.MediaType = PD.MediaType
                                                AND PD.PublishStatus = {1} )
                            AND GETDATE() BETWEEN A.BeginTime AND A.EndTime
                            AND HM.CategoryID = {2}", (int)MediaType.Broadcast, (int)EnumPublishStatus.OnSold,
                RequetQuery.CategoryId);

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MB.Name LIKE '%{0}%' ", RequetQuery.MediaName.ToSqlFilter());
            }

            sbSql.AppendLine(@" ) T");
            var query = new PublishQuery<ResRecommendBroadcast>()
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