/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 10:20:25
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.ResponseDto.Statistics.StatDaily;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage;
using XYAuto.BUOC.ChiTuData2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.ChiTuData2017.Entities.Query;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Extend;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Query.Statistics.StatDaily
{
    /// <summary>
    /// auth:lixiong
    /// desc:趋势分析-抓取-日汇总数据
    /// </summary>
    public class StatDailyGrabQuery
         : PublishInfoQueryClient<ReqDailyDto, RespDailyGrabDto>
    {
        public StatDailyGrabQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override QueryPageBase<RespDailyGrabDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                        SELECT  SS.RecID ,
                                SS.ChannelID ,
                                SS.ChannelName ,
                                SS.ArticleType ,
                                SS.ArticleCount ,
                                SS.AccountCount ,
                                SS.Date ,
                                DC.DictName AS ArticleTypeName
                        FROM    dbo.Stat_SpiderStatistics AS SS WITH ( NOLOCK )
                                LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = SS.ArticleType
                        WHERE   SS.Status = 0
                                AND SS.ChannelID > 0
                                AND SS.Date BETWEEN '{0}' AND '{1}'
                        ", RequetQuery.StartDate.ToSqlFilter(), RequetQuery.EndDate.ToSqlFilter());

            if (RequetQuery.ArticleType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SS.ArticleType = {RequetQuery.ArticleType}");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND SS.ChannelID = {RequetQuery.ChannelId}");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespDailyGrabDto>()
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