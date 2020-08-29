using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Response
{
    public class RespOrderIncomeDto
    {
        public List<OrderIncomeItem> List { get; set; }
        public OrderIncomeTotalItem Extend { get; set; }
    }

    public class OrderIncomeItem
    {

        public DateTime Date { get; set; }
        //CPC收入金额
        public decimal? CPCTotalPrice { get; set; }

        //CPL收入金额
        public decimal? CPLTotalPrice { get; set; }

        //收入总金额
        public decimal? TotalMoney { get; set; }
        public int AccountStatus { get; set; }
        //CPC点击数量
        public int CPCCount { get; set; }

        //CPL线索数量
        public int CPLCount { get; set; }

    }

    public class OrderIncomeTotalItem
    {
        public decimal? TotalCPCTotalPrice { get; set; }
        public decimal? TotalCPLTotalPrice { get; set; }
        public decimal? TotalMoney { get; set; }
        //CPC点击数量
        public int TotalCPCCount { get; set; }

        //CPL线索数量
        public int TotalCPLCount { get; set; }
    }
}
