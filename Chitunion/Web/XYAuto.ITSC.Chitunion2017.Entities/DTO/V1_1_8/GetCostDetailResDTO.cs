using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class GetCostDetailResDTO
    {
        public int CostID { get; set; }
        public int MediaID { get; set; }
        public string HeadIconUrl { get; set; }
        public string WxName { get; set; }
        public string WxNumber { get; set; }
        public string ChannelName { get; set; }
        public string CooperateDate { get; set; }
        public DateTime CooperateBeginDate { get; set; }
        public DateTime CooperateEndDate { get; set; }
        public string SaleStatusName { get; set; }
        public decimal OriginalPrice { get; set; }
        public int CreateUserID { get; set; }
        public List<CostItem> CostDetailList { get; set; }
    }

    public class CostItem
    {
        public int DetailID { get; set; }
        public string ADPosition { get; set; }
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
    }

}
