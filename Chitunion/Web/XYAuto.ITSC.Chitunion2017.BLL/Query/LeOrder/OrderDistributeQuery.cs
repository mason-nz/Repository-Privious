using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder
{
    /// <summary>
    /// auth:lixiong
    /// desc:订单-内容分发列表
    /// </summary>
    public class OrderDistributeQuery
        : PublishInfoQueryClient<ReqOrderCoverImageDto, RespOrderDistributeDto>
    {
        public OrderDistributeQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }

        protected override PublishQuery<RespOrderDistributeDto> GetQueryParams()
        {
            var sbSql = new StringBuilder();
            sbSql.Append("select T.* YanFaFROM ( ");

            sbSql.AppendFormat(@"
                                         
                    SELECT  AO.RecID AS OrderId,
                            AO.TotalAmount ,
                            AO.Status ,
                            AO.OrderType ,
                            AO.OrderName ,
                            AO.TaskID ,
                            AO.CreateTime AS ReceiveTime,
                            AO.BillingRuleName ,
                            DC.DictName AS OrderStatus ,
                            AO.CPCUnitPrice AS CPCPrice,
							AO.CPLUnitPrice AS CPLPrice
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK ) 
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.Status
                    WHERE   AO.MediaType = 14001
                            AND AO.OrderType = {1}
                            AND AO.UserID = {0}
                        ", RequetQuery.UserId, (int)LeTaskTypeEnum.ContentDistribute);

            if (RequetQuery.OrderStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND AO.Status = {RequetQuery.OrderStatus}");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.OrderName))
            {
                sbSql.AppendFormat($" AND AO.OrderName LIKE '%{RequetQuery.OrderName.ToSqlFilter()}%'");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespOrderDistributeDto>()
            {
                StrSql = sbSql.ToString(),
                OrderBy = " OrderId DESC ",
                PageSize = RequetQuery.PageSize,
                PageIndex = RequetQuery.PageIndex
            };
            return query;
        }
    }
}
