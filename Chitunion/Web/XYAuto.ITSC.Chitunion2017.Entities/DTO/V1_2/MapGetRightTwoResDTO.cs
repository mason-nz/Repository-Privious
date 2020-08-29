using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_2
{
    public class MapGetRightTwoResDTO
    {
        public string AdgroupName { get; set; }
        public string BelongYY { get; set; }
        public string AuditStatusName { get; set; }
        public int ADGroupCount { get; set; }
        public int TotalImpression { get; set; }
        public int TotalClick { get; set; }
        public decimal AvgClickPercent { get; set; }
        public int OrderQuantity { get; set; }
        public int BillOfQuantities { get; set; }
        public int TotalCost { get; set; }
    }
}
