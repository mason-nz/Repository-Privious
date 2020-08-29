using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.ChannelStat;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.ChannelStat
{
    /// <summary>
    /// auth:lixiong
    /// desc:渠道月结数据
    /// </summary>
    public class ChannelStatMonthQuery
        : PublishInfoQueryClient<ReqChannelStatDto, RespChannelStatDto>
    {
        public ChannelStatMonthQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespChannelStatDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                   
                    SELECT  DSB.RecID AS StatisticsId ,
                            DSB.CreateTime AS DtSummaryDate ,
                            DSB.Date AS DtMonth ,
                            DSB.ChannelID ,
                            DSB.OrderNumber ,
                            DSB.TotalAmount ,
                            ISNULL(CSM.PayStatus, {0}) AS PayStatus ,
                            CSM.PayTime ,
                            DC.DictName AS ChannelName ,
                            DC1.DictName AS PayStatusName
                    FROM    Chitunion_DataSystem2017.dbo.DataStatisticsByMonth AS DSB WITH ( NOLOCK )
                            LEFT JOIN dbo.LE_ChannelStatMonthRelation AS CSM WITH ( NOLOCK ) ON CSM.StatisticsId = DSB.RecID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = DSB.ChannelID
                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = ISNULL(CSM.PayStatus,{0})
                    WHERE   1 = 1
                        ", (int)ChannelStatMonthPayStatusEnum.未支付);

            var sbSqlWhere = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(RequetQuery.SummaryDate))
            {
                //追加1个月
                var startTime = Convert.ToDateTime(RequetQuery.SummaryDate).FirstDayOfMonth().ToString("yyyy-MM-dd");
                var endTime = Convert.ToDateTime(RequetQuery.SummaryDate).AddMonths(1).FirstDayOfMonth().ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND DSB.Date >= '{startTime}'");
                sbSqlWhere.Append($@" AND DSB.Date < '{endTime}'");
            }

            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@" AND DSB.ChannelID = {0}", RequetQuery.ChannelId);
            }
            if (RequetQuery.PayStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                if (RequetQuery.PayStatus == (int)ChannelStatMonthPayStatusEnum.未支付)
                {
                    sbSqlWhere.AppendFormat(@" AND CSM.PayStatus IS NULL");
                }
                else
                {
                    sbSqlWhere.AppendFormat(@" AND CSM.PayStatus = {0}", RequetQuery.PayStatus);
                }
            }

            //将sql where 条件追加到后面，后面有用
            sbSql.Append(sbSqlWhere);

            RequetQuery.SqlWhere = sbSqlWhere.ToString();

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespChannelStatDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " DtMonth DESC ,StatisticsId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespChannelStatDto> GetResult(List<RespChannelStatDto> resultList, QueryPageBase<RespChannelStatDto> query)
        {
            var resp = base.GetResult(resultList, query);
            if (!resultList.Any())
            {
                resp.Extend = new
                {
                    TotalMoney = 0m
                };
                return resp;
            }

            resultList.ForEach(s =>
            {
                s.Month = s.DtMonth.ToString("Y");
                s.SummaryDate = s.DtSummaryDate.ToString("yyyy-MM-dd");
            });
            resp.Extend = new
            {
                TotalMoney = Dal.LETask.LeChannelStatMonthRelation.Instance.GetToTalDecimal(RequetQuery.SqlWhere)
            };

            return resp;
        }
    }
}
