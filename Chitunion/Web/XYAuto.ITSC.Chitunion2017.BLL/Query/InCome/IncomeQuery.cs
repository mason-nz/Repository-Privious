using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Media;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.InCome
{
    /// <summary>
    /// auth:lixiong
    /// desc:收入管理-收入明细列表接口
    /// </summary>
    public class IncomeQuery
          : PublishInfoQueryClient<ReqInComeDto, RespInComeDto>
    {
        public IncomeQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespInComeDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                   
                    SELECT  AO.RecID AS OrderId ,
                            AO.TotalAmount ,--订单收入
                            AO.OrderType ,
                            DC.DictName AS OrderTypeName ,
                            AO.OrderName,
		                    DATEADD(DAY,1,AO.EndTime) AS IncomeTime
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.OrderType
                            WHERE 1= 1
                        ");

            var sbSqlWhere = new StringBuilder();

            sbSqlWhere.AppendFormat(@" AND AO.UserID = {0}
                            AND AO.Status = {1}", RequetQuery.UserId, (int)LeOrderStatusEnum.Finished);

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                var startTime = Convert.ToDateTime(RequetQuery.StartDate).AddDays(-1).ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND AO.EndTime >= '{startTime}'");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.EndDate))
            {
                var endTime = Convert.ToDateTime(RequetQuery.EndDate).AddDays(-1).ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND AO.EndTime <= '{endTime}'");
            }
            if (RequetQuery.OrderType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND AO.OrderType = {RequetQuery.OrderType}");
            }

            //将sql where 条件追加到后面，后面有用
            sbSql.Append(sbSqlWhere);

            RequetQuery.SqlWhere = sbSqlWhere.ToString();

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespInComeDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " OrderId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespInComeDto> GetResult(List<RespInComeDto> resultList, QueryPageBase<RespInComeDto> query)
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
            resp.Extend = new
            {
                TotalMoney = Dal.LETask.LeAdOrderInfo.Instance.GetTotalAmount(RequetQuery.SqlWhere)
            };

            return resp;
        }
    }
}
