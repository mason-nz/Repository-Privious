using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //收入明细
    public class LeAccountBalance
    {

        //主键
        public int RecID { get; set; }

        //CPC点击数量
        public int CPCCount { get; set; }

        //CPL线索数量
        public int CPLCount { get; set; }

        //PV数量
        public int PVCount { get; set; }

        //关联订单表主键
        public int OrderID { get; set; }

        public int Status { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //统计时间
        public DateTime StatisticsTime { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //CPC收入金额
        public decimal? CPCTotalPrice { get; set; }

        //CPL收入金额
        public decimal? CPLTotalPrice { get; set; }

        //收入总金额
        public decimal? TotalMoney { get; set; }

        //CPC点击数量
        public int CPCShowCount { get; set; }

        //CPL线索数量
        public int CPLShowCount { get; set; }
    }
}