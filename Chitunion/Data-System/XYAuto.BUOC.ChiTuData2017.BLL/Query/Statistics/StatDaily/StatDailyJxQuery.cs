/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 10:57:36
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
    public class StatDailyJxQuery
         : PublishInfoQueryClient<ReqDailyDto, RespDailyJxDto>
    {
        public StatDailyJxQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDailyJxDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"

                            SELECT  SAS.RecID ,
                                    SAS.ChannelID ,
                                    SAS.ChannelName ,
                                    SAS.ArticleType ,
                                    SAS.ArticleCount ,
                                    SAS.AccountCount ,
                                    SAS.StorageArticleCount ,
                                    SAS.StorageAccountCount ,
                                    SAS.Date ,
                                    DC.DictName AS ArticleTypeName
                            FROM    dbo.Stat_AutoStatistics AS SAS WITH ( NOLOCK )
                                    LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = SAS.ArticleType
                            WHERE   SAS.Status = 0
                                    AND SAS.ChannelID > 0
                                    AND SAS.Date BETWEEN '{0}' AND '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAS.ArticleType = {RequetQuery.ArticleType}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SAS.ChannelID = {RequetQuery.ChannelId}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDailyJxDto>()
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