using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{

    public class OneDayPubDTO
    {
        public OneDayPubDTO()
        {
            this.ADList = new List<ADDTO>();
        }
        public decimal OriginalPrice { get; set; }
        public DateTime PubDate { get; set; }
        public List<ADDTO> ADList { get; set; }
    }

    public class ManyDaysPubDTO
    {
        public ManyDaysPubDTO()
        {
            this.ADList = new List<ADDTO>();
        }
        public DateTime PubBeginDate { get; set; }
        public DateTime PubEndDate { get; set; }
        public List<ADDTO> ADList { get; set; }
        public decimal OriginalPrice { get; set; }
    }

    public class ADDTO
    {
        public int ADPosition1 { get; set; }
        public int ADPosition2 { get; set; }
        public int ADPosition3 { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int CostDetailID { get; set; }
        public int ChannelID { get; set; }
    }
}
