using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend
{
    public class RecommendWeiXinQuery : PublishInfoQueryClient<RecommendSearchDto, ResRecommendWeiXin>
    {
        public RecommendWeiXinQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<ResRecommendWeiXin> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");
            sbSql.Append(@"
                    SELECT  HM.RecID ,
                            HM.SortNumber ,
                            WO.RecID AS MediaID ,
                            WO.NickName AS Name ,
                            WO.WxNumber AS Number,
                            WO.HeadImg AS HeadIconURL ,
                            WO.FansCount ,
                            IW.AveragePointCount ,
                            IW.MaxinumReading ,
                            IW.ReferReadCount,
                            IW.UpdateCount
                    FROM    dbo.Home_Media AS HM WITH ( NOLOCK )
                            INNER JOIN dbo.Weixin_OAuth AS WO WITH ( NOLOCK ) ON WO.RecID = HM.MediaID AND WO.Status = 0
		                    LEFT JOIN dbo.Interaction_Weixin AS IW WITH ( NOLOCK ) ON IW.WxID = HM.MediaID
                    WHERE 1=1  ");

            sbSql.AppendFormat(@" AND  HM.MediaType = {0}
		                        AND HM.CategoryID = {1}", (int)MediaType.WeiXin, RequetQuery.CategoryId);

            if (!string.IsNullOrWhiteSpace(RequetQuery.MediaName))
            {
                sbSql.AppendFormat(" AND WO.NickName LIKE '%{0}%' OR WO.WxNumber LIKE '%{0}%'", RequetQuery.MediaName.ToSqlFilter());
            }
            sbSql.AppendLine(@" ) T");
            var query = new PublishQuery<ResRecommendWeiXin>()
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