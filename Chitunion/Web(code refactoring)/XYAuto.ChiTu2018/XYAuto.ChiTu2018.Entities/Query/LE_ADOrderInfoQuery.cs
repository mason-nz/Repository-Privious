using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.Entities.Extend;

namespace XYAuto.ChiTu2018.Entities.Query
{
    /// <summary>
    /// 注释：LE_ADOrderInfoQuery
    /// 作者：lihf
    /// 日期：2018/5/16 19:15:01
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class LE_ADOrderInfoQuery : Pagination
    {
        public int RecID { get; set; } = -2;

        public DateTime BeginTime { get; set; } = Constants.Constant.DATE_INVALID_VALUE;

        public DateTime EndTime { get; set; } = Constants.Constant.DATE_INVALID_VALUE;

        public decimal TotalAmount { get; set; } = 0;

        public int Status { get; set; } = -2;

        public int OrderType { get; set; } = -2;

        public string OrderName { get; set; } = string.Empty;

        public string OrderUrl { get; set; } = string.Empty;

        public string PasterUrl { get; set; } = string.Empty;

        public int UserID { get; set; } = -2;

        public int TaskID { get; set; } = -2;

        public DateTime CreateTime { get; set; } = Constants.Constant.DATE_INVALID_VALUE;

        public string BillingRuleName { get; set; } = string.Empty;

        public string OrderCoding { get; set; } = string.Empty;

        public int MediaType { get; set; } = -2;

        public int MediaID { get; set; } = -2;

        public int ChannelID { get; set; } = -2;

        public string UserIdentity { get; set; } = string.Empty;

        public decimal CPCUnitPrice { get; set; } = 0;

        public decimal CPLUnitPrice { get; set; } = 0;

        public int StatisticsStatus { get; set; } = 0;

        public string IP { get; set; } = string.Empty;

        public long PromotionChannelID { get; set; } = 0;

        /// <summary>
        /// 获取用户前n天订单数量
        /// </summary>
        public int Days { get; set; } = 0;
    }
}
