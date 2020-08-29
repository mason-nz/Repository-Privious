using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.DSC.IM_2015.Entities
{
    /// 订单对象
    /// <summary>
    /// 订单对象
    /// </summary>
    public class OrderInfoHBList
    {
        /// 订单总数
        /// <summary>
        /// 订单总数
        /// </summary>
        public int recordCount { get; set; }

        /// 订单数据
        /// <summary>
        /// 订单数据
        /// </summary>
        public List<OrderInfoHB> orders { get; set; }
    }
    /// 订单
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderInfoHB
    {
        /// 手机号
        /// <summary>
        /// 手机号
        /// </summary>
        public string userPhone { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime orderTime { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string orderType { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<List<string>> orderDetail { get; set; }
    }
    /// 订单明细
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderDetailHB
    {
        // orderType =1,3
        public string ClueID { get; set; }
        public string DealerID { get; set; }
        public string LocationID { get; set; }
        public string DataSourceId { get; set; }
        public string ClueCreateTime { get; set; }
        public string CarID { get; set; }
        public string lastUpdateTime { get; set; }
        public string CookieId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserMobile { get; set; }
        public string UserGender { get; set; }

        // orderType =2
        public string ReplacementCarId { get; set; }
        public string ReplacementCarColor { get; set; }
        public string ReplacementCarUsedMiles { get; set; }
        public string ReplacementCarGetLisenceDate { get; set; }
        public string ReplacementCarLocationID { get; set; }

        // orderType =5
        public string OrderID { get; set; }
        public string CreateTime { get; set; }
        public string Email { get; set; }
        public string BrandID { get; set; }
        public string SerialID { get; set; }
        public string PlanBuyTime { get; set; }
        public string AGuid { get; set; }

        // orderType =7
        public string OrderStatusByC { get; set; }
        public string Subscription { get; set; }
        public string OrderType { get; set; }
        public string DealerIds { get; set; }
        public string LastUpTime { get; set; }

        // orderType =8
        public string ID { get; set; }
        public string Type { get; set; }
        public string GoodsName { get; set; }
        public string Price { get; set; }
        public string UserStatusName { get; set; }
        public string UserAddress { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public string Mileage { get; set; }
        public string BookingDate { get; set; }

        // orderType =9
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public string CityID { get; set; }
        public string OrderTime { get; set; }
        public string DataExtractTime { get; set; }
        public string ProductID { get; set; }

        // orderType =11
        public string OrderTypeID { get; set; }
        public string ProvinceID { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string TotalPrice { get; set; }
        public string OrderStatusID { get; set; }
        public string OrderStatus { get; set; }
    }

    /// 业务数据-订单
    /// <summary>
    /// 业务数据-订单
    /// </summary>
    [Serializable]
    public class OrderData
    {
        /// 姓名
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string UserPhone { get; set; }

        /// 主品牌
        /// <summary>
        /// 主品牌
        /// </summary>
        public string MasterBrand { get; set; }
        /// 品牌
        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }
        /// 车型
        /// <summary>
        /// 车型
        /// </summary>
        public string Serial { get; set; }
        /// 下单时间
        /// <summary>
        /// 下单时间
        /// </summary>
        public string OrderTime { get; set; }
        /// 来源
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// 订单编号
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderID { get; set; }
        /// 备注
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// 跳转链接
        /// <summary>
        /// 跳转链接
        /// </summary>
        public string Url { get; set; }
        /// 订单类型
        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }
    }
}
