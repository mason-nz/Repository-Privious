using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Withdrawals
{
    /// <summary>
    /// auth:lixiong
    /// desc:资源管理后台-财务管理-提现管理-媒体主提现列表
    /// </summary>
    public class WithdrawalsQuery
        : PublishInfoQueryClient<ReqWithdrawalsDto, RespWithdrawalsDto>
    {
        public WithdrawalsQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespWithdrawalsDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                   
                    SELECT  WD.RecID ,
                            WD.RecID AS Id,
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
                            WD.CreateTime ,
                            WD.IsLock ,
                            UI.SysName AS TrueName,
                            UI.UserName,
		                    WD.AuditStatus ,
                            DC1.DictName AS AuditStatusName ,
                            A.CreateTime AS AuditTime,
                            DC2.DictName AS PayStatusName ,
                            UI.Type AS UserType ,
							DC.DictName AS UserTypeName ,
                            A.RejectMsg
                    FROM    dbo.LE_WithdrawalsDetail AS WD WITH ( NOLOCK )
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = WD.PayeeID
                            LEFT JOIN DBO.DictInfo AS DC WITH(NOLOCK) ON DC.DictId = UI.Type
                            LEFT JOIN dbo.DictInfo AS DC1 WITH ( NOLOCK ) ON DC1.DictId = WD.AuditStatus
                            LEFT JOIN dbo.DictInfo AS DC2 WITH ( NOLOCK ) ON DC2.DictId = WD.Status
                            LEFT JOIN ( SELECT  AI.CreateTime ,
                                                AI.RelationId ,
                                                AI.AuditStatus ,
                                                AI.RejectMsg
                                        FROM    dbo.AuditInfo AS AI WITH ( NOLOCK )
                                        WHERE   AI.RelationType = {0}
                                      ) AS A ON A.RelationId = WD.RecID
                                                AND A.AuditStatus = WD.AuditStatus
                    WHERE   WD.IsActive = 1
                        ", (int)AuditTypeEnum.提现审核);

            var sbSqlWhere = new StringBuilder();

            sbSqlWhere.AppendFormat(@" AND WD.AuditStatus = {0}", RequetQuery.AuditStatus);

            if (RequetQuery.OrderStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.AppendFormat(@" AND WD.Status = {0}", RequetQuery.OrderStatus);
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.StartDate))
            {
                sbSqlWhere.Append($@" AND WD.ApplicationDate >= '{RequetQuery.StartDate}'");
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.EndDate))
            {
                var endTime = Convert.ToDateTime(RequetQuery.EndDate).AddDays(1).ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND WD.ApplicationDate < '{endTime}'");
            }
          
            if (!string.IsNullOrWhiteSpace(RequetQuery.UserName))
            {
                sbSqlWhere.AppendFormat(@" AND (UI.SysName = '{0}' OR UI.UserName = '{0}')", RequetQuery.UserName.ToSqlFilter());
            }
            //*******张劲龙添加—支付时间查询条件
            if (!string.IsNullOrWhiteSpace(RequetQuery.BeginPayDate))
            {
                sbSqlWhere.Append($@" AND WD.PayDate >= '{RequetQuery.BeginPayDate}'");
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.EndPayDate))
            {
                var endpaydate = Convert.ToDateTime(RequetQuery.EndPayDate).AddDays(1).ToString("yyyy-MM-dd");
                sbSqlWhere.Append($@" AND WD.PayDate < '{endpaydate}'");
            }

            //将sql where 条件追加到后面，后面有用
            sbSql.Append(sbSqlWhere);

            RequetQuery.SqlWhere = sbSqlWhere.ToString();

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespWithdrawalsDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " RecId ",
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
                TotalMoney = Dal.LETask.LeWithdrawalsDetail.Instance.GetTotalAmountByAdmin(RequetQuery.SqlWhere)
            };

            return resp;
        }
    }
}
