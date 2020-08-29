using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetCostListResDTO
    {
        public List<CostListItem> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class CostListItem
    {
        public int CostID { get; set; }
        public string HeadIconUrl { get; set; }
        public string WxName { get; set; }
        public string WxNumber { get; set; }
        public string ChannelName { get; set; }
        public string CostPriceRange { get; set; }
        public string SalePriceRange { get; set; }
        public decimal OriginalPrice { get; set; }
        public string CooperateDate { get; set; }
        public int SaleStatus { get; set; }
        public string SaleStatusName { get; set; }
    }
}
