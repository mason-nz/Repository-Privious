using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.InCome
{
    /// <summary>
    /// auth:lixiong
    /// desc:收入管理-体现明细列表接口
    /// </summary>
    public class IncomeWithdrawalsQuery
         : PublishInfoQueryClient<ReqInComeDto, RespWithdrawalsDto>
    {
        public IncomeWithdrawalsQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespWithdrawalsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");


            sbSql.AppendFormat($@"
                   
                    SELECT  WD.RecID,
                            WD.RecID AS Id ,
                            WD.WithdrawalsPrice ,
                            WD.IndividualTaxPeice ,
                            WD.PracticalPrice ,
                            WD.PayeeAccount ,
                            WD.Status AS PayStatus,
                            WD.ApplicationDate ,
                            WD.PayDate ,
                            WD.OrderID ,
                            WD.PayeeID ,
                            WD.Reason ,

                            DC.DictName AS PayStatusName,
                            AI.RejectMsg
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = WD.Status

                            LEFT JOIN dbo.AuditInfo AS AI WITH ( NOLOCK ) ON WD.RecID=AI.RelationId  AND AI.RelationType = {(int)AuditTypeEnum.提现审核}
                    WHERE   WD.IsActive = 1 ");
            var sbSqlWhere = new StringBuilder();

            sbSqlWhere.AppendFormat(@" AND WD.PayeeID = {0}
                            ", RequetQuery.UserId);
            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                var endTime = Convert.ToDateTime(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND WD.ApplicationDate >= '{RequetQuery.StartDate}' AND WD.ApplicationDate < '{endTime}' ");
            }

            if (RequetQuery.PayType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND WD.Status = {RequetQuery.PayType}");
            }

            sbSql.Append(sbSqlWhere);

            RequetQuery.SqlWhere = sbSqlWhere.ToString();

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespWithdrawalsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " Id DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespWithdrawalsDto> GetResult(List<RespWithdrawalsDto> resultList, QueryPageBase<RespWithdrawalsDto> query)
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
                TotalMoney = Dal.LETask.LeWithdrawalsDetail.Instance.GetTotalAmount(RequetQuery.SqlWhere)
            };
            return resp;
        }
    }
}
