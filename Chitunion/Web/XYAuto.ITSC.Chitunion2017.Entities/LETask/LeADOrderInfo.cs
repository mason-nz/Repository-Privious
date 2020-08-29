using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //投放订单信息
    public class LeAdOrderInfo
    {

        //主键ID
        public int RecID { get; set; }

        //执行周期(开始)
        public DateTime BeginTime { get; set; }

        //执行周期(结束)
        public DateTime EndTime { get; set; }

        //订单总金额
        public decimal TotalAmount { get; set; }

        //状态
        public int Status { get; set; }

        //1代客下单 2自助下单 3智投订单
        public int OrderType { get; set; }

        //订单名称
        public string OrderName { get; set; }

        //专属链接地址
        public string OrderUrl { get; set; }

        //头图地址
        public string PasterUrl { get; set; }

        public int UserID { get; set; }

        public int TaskID { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public string BillingRuleName { get; set; }

        //针对每个用户每个任务的唯一编码
        public string OrderCoding { get; set; }

        public int MediaType { get; set; }

        public int MediaID { get; set; }

        public int ChannelID { get; set; }

        public string UserIdentity { get; set; }

        /*冗余*/
        public string OrderStatus { get; set; }

        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }


        public decimal CPCUnitPrice { get; set; }
        public decimal CPLUnitPrice { get; set; }
        public int StatisticsStatus { get; set; }

        public string IP { get; set; }

        public long PromotionChannelID { get; set; }

        //冗余字段（）
        public string ShareTempQrImage { get; set; }
    }
}