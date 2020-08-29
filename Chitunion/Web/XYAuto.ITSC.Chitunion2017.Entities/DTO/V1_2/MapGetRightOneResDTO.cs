using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class MapGetRightOneResDTO
    {
        public AccountDTO XJAccount { get; set; } = new AccountDTO();
        public AccountDTO XNAccount { get; set; } = new AccountDTO();
        public AccountDTO FCAccount { get; set; } = new AccountDTO();
        public AccountDTO YZAccount { get; set; } = new AccountDTO();

        public DemandStatisticDTO DemandStatistic { get; set; } = new DemandStatisticDTO();
        public ADGroupStatisticDTO ADGroupStatistic { get; set; } = new ADGroupStatisticDTO();
    }

    public class AccountDTO
    {
        public decimal Current { get; set; }
        public decimal Cost { get; set; }
    }
    public class DemandStatisticDTO
    {
        public int DSH { get; set; }
        public int DTF { get; set; }
        public int TFZ { get; set; }
        public int YJS { get; set; }
    }
    public class ADGroupStatisticDTO
    {
        public int DSH { get; set; }
        public int TFZ { get; set; }
        public int YZT { get; set; }
    }

    public class AccountItemDTO
    {
        public int FundType { get; set; }
        public int Current { get; set; }
        public int Cost { get; set; }
    }

    public class StatisticItemDTO
    {
        public int AuditStatus { get; set; }
        public int Count { get; set; }
    }
}
