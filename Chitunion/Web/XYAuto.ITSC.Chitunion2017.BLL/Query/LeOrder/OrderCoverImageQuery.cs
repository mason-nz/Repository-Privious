/********************************************************
*创建人：lixiong
*创建时间：2017/11/29 13:43:09
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.LeOrder
{
    /// <summary>
    /// auth:lixiong
    /// desc:订单-贴片广告订单列表
    /// </summary>
    public class OrderCoverImageQuery
          : PublishInfoQueryClient<ReqOrderCoverImageDto, RespOrderCoverImageDto>
    {
        public OrderCoverImageQuery(ConfigEntity configEntity) : base(configEntity)
        {
        }


        protected override PublishQuery<RespOrderCoverImageDto> GetQueryParams()
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
                            WXR.NickName AS MediaName ,
                            AO.CPCUnitPrice AS CPCPrice,
							AO.CPLUnitPrice AS CPLPrice
                    FROM    dbo.LE_ADOrderInfo AS AO WITH ( NOLOCK )
                            LEFT JOIN dbo.LE_Weixin_Repea AS WXR WITH ( NOLOCK ) ON AO.MediaID = WXR.RecID
                            LEFT JOIN dbo.DictInfo AS DC WITH ( NOLOCK ) ON DC.DictId = AO.Status
                    WHERE   AO.MediaType = 14001
                            AND AO.OrderType = {1}
                            AND AO.UserID = {0}
                        ", RequetQuery.UserId, (int)LeTaskTypeEnum.CoverImage);

            if (RequetQuery.OrderStatus != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sbSql.AppendFormat($" AND AO.Status = {RequetQuery.OrderStatus}");
            }
            if (!string.IsNullOrWhiteSpace(RequetQuery.OrderName))
            {
                sbSql.AppendFormat($" AND AO.OrderName LIKE '%{RequetQuery.OrderName.ToSqlFilter()}%'");
            }

            sbSql.AppendLine(@") T");

            var query = new PublishQuery<RespOrderCoverImageDto>()
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