using System;

namespace XYAuto.BUOC.ChiTuData2017.Entities.QingNiao
{
    //渠道统计
    public class ChituChannelStat
    {
        //日期
        public DateTime Dt { get; set; }

        //渠道
        public string Channel { get; set; }

        //pv
        public int Pv { get; set; }

        //uv
        public int Uv { get; set; }

        //平均访问时间
        public decimal Avg_dur { get; set; }

        //订单量
        public int Orders { get; set; }

        //订单独立手机号
        public int Order_Phones { get; set; }
    }
}