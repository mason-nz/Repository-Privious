namespace XYAuto.BUOC.BOP2017.Entities.Dto
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

        public int PV { get; set; }

        public int UV { get; set; }

        public int ClueCount { get; set; }

        public decimal CluePrice { get; set; }

        public string AdPage { get; set; }
    }
}