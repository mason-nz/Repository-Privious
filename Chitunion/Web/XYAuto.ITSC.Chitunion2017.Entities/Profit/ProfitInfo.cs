using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.Profit
{
    public class ProfitInfo
    {
        public int ProfitType { get; set; }
        public string ProfitDate { get; set; } = "";
        public decimal ProfitPrice { get; set; }
        public string ProfitDescribe { get; set; } = "";
        public long RowNum { get; set; }
        public string Nickname { get; set; } = "";
        public string Headimgurl { get; set; } = "";
        public DateTime IncomeTime { get; set; }
        public long ReadCount { get; set; }
    }
}
