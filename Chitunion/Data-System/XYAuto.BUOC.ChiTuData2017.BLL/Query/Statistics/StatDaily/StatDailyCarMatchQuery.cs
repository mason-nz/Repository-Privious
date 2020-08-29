/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 13:43:09
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily
{
    public class StatDailyCarMatchQuery
          : PublishInfoQueryClient<ReqDailyDto, RespDailyCarMatchDto>
    {
        public StatDailyCarMatchQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDailyCarMatchDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  CMS.RecID ,
                                    CMS.ChannelID ,
                                    CMS.ChannelName ,
                                    CMS.MatchArticleCount ,
                                    CMS.UnMatchArticleCount ,
                                    CMS.Date ,
                                    CMS.Status ,
                                    CMS.CreateTime
                            FROM    Stat_CarMatchStatistics AS CMS WITH ( NOLOCK )
                            WHERE   CMS.Status = 0
                                    AND CMS.ChannelID > 0
                                    AND CMS.Date BETWEEN '{0}' AND '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND CMS.ArticleType = {RequetQuery.ArticleType}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND CMS.ChannelID = {RequetQuery.ChannelId}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDailyCarMatchDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Date DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}