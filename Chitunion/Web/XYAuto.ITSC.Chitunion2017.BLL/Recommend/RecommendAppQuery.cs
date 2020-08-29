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
    public class RecommendAppQuery : PublishInfoQueryClient<RecommendSearchDto, ResRecommendApp>
    {
        public RecommendAppQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResRecommendApp> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"

                        SELECT  HM.RecID ,
                                HM.SortNumber ,
                                HM.ImageUrl ,
                                HM.VideoUrl ,
                                MBP.RecID AS MediaID ,
                                MBP.Name ,
                                MBP.HeadIconURL ,
                                ADT.AdTemplateName AS AdPosition ,
                                ADT.AdForm,
		                        ADT.AdLegendURL
                        FROM    dbo.Home_Media AS HM WITH ( NOLOCK )
								INNER JOIN DBO.Publish_BasicInfo AS PB WITH(NOLOCK) ON HM.ADDetailID = PB.PubID AND PB.MediaType = HM.MediaType
								LEFT JOIN dbo.App_AdTemplate AS ADT WITH ( NOLOCK ) ON ADT.RecID = HM.TemplateID
								LEFT JOIN DBO.Media_PCAPP AS MP WITH(NOLOCK) ON MP.MediaID = PB.MediaID AND MP.Status = 0
                                LEFT JOIN dbo.Media_BasePCAPP AS MBP WITH ( NOLOCK ) ON MBP.RecID = MP.BaseMediaID
                                                                                      AND MBP.Status = 0
                        WHERE   1 = 1 ");
            sbSql.AppendFormat(@" AND HM.MediaType = {0} ", (int)MediaType.APP);
            sbSql.AppendFormat(@" AND PB.Wx_Status = {0} ", (int)AppPublishStatus.已上架);//必须是已上架的

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND MBP.Name LIKE '%{0}%' ", RequetQuery.MediaName.ToSqlFilter());
            }
            sbSql.AppendLine(@" ) T");
            var query = new PublishQuery<ResRecommendApp>()
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