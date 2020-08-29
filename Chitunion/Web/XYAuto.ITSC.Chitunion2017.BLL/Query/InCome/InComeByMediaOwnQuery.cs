using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.InCome;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.InCome
{
    /// <summary>
    /// auth:lixiong
    /// desc:财务管理-媒体主收入记录(查询订单状态为【已结束】的订单)
    /// </summary>
    public class InComeByMediaOwnQuery
          : PublishInfoQueryClient<ReqInComeByMediaOwnDto, RespInComeByMediaOwnDto>
    {
        public InComeByMediaOwnQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespInComeByMediaOwnDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                   
                    SELECT  AO.RecID AS OrderId ,
                            AO.BeginTime ,
                            AO.EndTime ,
                            AO.TotalAmount ,
                            AO.Status ,
                            AO.OrderType ,
                            AO.OrderName ,
                            AO.UserID ,
                            AO.TaskID ,
                            DATEADD(DAY,1,AO.EndTime) AS IncomeTime,
                            AO.MediaID ,
                            AO.ChannelID ,
                            DC.DictName AS OrderTypeName ,
                            T.MaterialUrl ,
                            T.MaterialID AS MaterielId,
                            UI.UserName
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                            LEFT JOIN DBO.LE_TaskInfo AS T WITH(NOLOCK) ON T.RecID = AO.TaskID
                            LEFT JOIN dbo.v_UserInfo AS UI WITH ( NOLOCK ) ON UI.UserID = AO.UserID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.OrderType
                    WHERE   1 = 1
                        ");

            var sbSqlWhere = new StringBuilder();

            sbSqlWhere.AppendFormat(@" AND AO.Status = {0}", (int)LeOrderStatusEnum.Finished);
            
            if (!string.IsNullOrWhiteSpace(RequetQuery.OrderId) &&
                    RequetQuery.OrderId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT) != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND AO.RecID = {RequetQuery.OrderId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT)}");
            }
            if (RequetQuery.OrderType != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND AO.OrderType = {RequetQuery.OrderType}");
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.TaskId) && 
                RequetQuery.TaskId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT) != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND AO.TaskID = {RequetQuery.TaskId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT)}");
            }

            if (!string.IsNullOrWhiteSpace(RequetQuery.MaterielId) && 
                RequetQuery.MaterielId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT) != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND EXISTS ( SELECT 1
                                                     FROM   dbo.LE_TaskInfo AS T WITH ( NOLOCK )
                                                     WHERE  T.RecID = AO.TaskID
                                                            AND T.MaterialID = {RequetQuery.MaterielId.ToInt(Entities.Constants.Constant.INT_INVALID_VALUE_TOINT)} )");
            }
            if (RequetQuery.ChannelId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSqlWhere.Append($@" AND AO.ChannelID = {RequetQuery.ChannelId}");
            }
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

            if (!string.IsNullOrWhiteSpace(RequetQuery.UserName))
            {
                sbSqlWhere.Append($@" AND (UI.SysName = '{RequetQuery.UserName.ToSqlFilter()}' OR UI.UserName= '{RequetQuery.UserName.ToSqlFilter()}')");
            }

            //将sql where 条件追加到后面，后面有用
            sbSql.Append(sbSqlWhere);

            RequetQuery.SqlWhere = sbSqlWhere.ToString();

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespInComeByMediaOwnDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " OrderId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }

        protected override BaseResponseEntity<RespInComeByMediaOwnDto> GetResult(List<RespInComeByMediaOwnDto> resultList,
            QueryPageBase<RespInComeByMediaOwnDto> query)
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
                TotalMoney = Dal.LETask.LeAdOrderInfo.Instance.GetTotalAmountByMediaOwn(RequetQuery.SqlWhere)
            };

            return resp;
        }
    }
}
