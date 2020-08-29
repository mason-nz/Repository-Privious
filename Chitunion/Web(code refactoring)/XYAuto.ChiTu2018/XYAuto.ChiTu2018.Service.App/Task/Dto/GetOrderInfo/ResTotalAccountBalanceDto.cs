namespace XYAuto.ChiTu2018.Service.App.Task.Dto.GetOrderInfo
{
    /// <summary>
    /// 注释：ResTotalAccountBalanceDto
    /// 作者：lihf
    /// 日期：2018/5/14 11:56:18
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ResTotalAccountBalanceDto
    {
        //CPC收入金额
        public decimal? TotalCPCTotalPrice { get; set; }

        //CPL收入金额
        public decimal? TotalCPLTotalPrice { get; set; }

        //收入总金额
        public decimal? TotalMoney { get; set; }

        //CPC点击数量
        public int TotalCPCCount { get; set; }

        //CPL线索数量
        public int TotalCPLCount { get; set; }
    }
}
