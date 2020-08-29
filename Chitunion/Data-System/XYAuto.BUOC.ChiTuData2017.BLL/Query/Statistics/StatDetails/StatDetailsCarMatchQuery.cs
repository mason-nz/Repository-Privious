/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 19:01:11
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDetails;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDetails
{
    /// <summary>
    /// auth:lixiong
    /// desc:明细数据-车型匹配
    /// </summary>
    public class StatDetailsCarMatchQuery
          : PublishInfoQueryClient<ReqDetailsDto, RespStatDetailsCarMatchDto>
    {
        public StatDetailsCarMatchQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespStatDetailsCarMatchDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  SCM.RecID ,
                                    SCM.ArticleID ,
                                    SCM.Title ,
                                    SCM.Url ,
                                    SCM.ChannelID ,
                                    SCM.ChannelName ,
                                    SCM.ArticlePublishTime ,
                                    SCM.ArticleSpiderTime ,
                                    SCM.MatchCarTime ,
                                    SCM.Category ,
                                    SCM.BrandId ,
                                    SCM.BrandName ,
                                    SCM.SerialId ,
                                    SCM.SerialName ,
                                    SCM.ArticleSorce ,
                                    SCM.MatchStatus ,
                                    SCM.MatchName
                            FROM    dbo.Stat_CarMatchData AS SCM WITH ( NOLOCK )
                            WHERE   SCM.Status = 0
                                    AND SCM.MatchCarTime >= '{0}'
                                    AND SCM.MatchCarTime < '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(),
                        Convert.ToDateTime(RequetQuery.EndDate.ToSqlFilter()).AddDays(1).ToString("yyyy-MM-dd"));

            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SCM.ChannelID = {RequetQuery.ChannelId}");
            }
            if (RequetQuery.MatchStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SCM.MatchStatus = {RequetQuery.MatchStatus}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespStatDetailsCarMatchDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " MatchCarTime DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}