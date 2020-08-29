using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
   public class TotalAccountBalance
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
