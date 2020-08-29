using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class ADGroupDTO : Entities.GDT.GdtAdGroup
    {
        public string SystemStatusName { get; set; }
        public int TotalImpression { get; set; }
        public int TotalClick { get; set; }
        public decimal AvgClickPercent { get; set; }
        public int OrderQuantity { get; set; }
        public int BillOfQuantities { get; set; }
        public int TotalCost { get; set; }
        //关联加载用到
        public string CampaignName { get; set; }
        public bool IsChecked { get; set; }
        //
        public DateTime Date { get; set; }
        public int Hour { get; set; }
        public string AdgroupDate { get; set; }
        //
        public int DemandId { get; set; }
        public string Name { get; set; }

        public int DemandBillNo { get; set; }

    }
}
