using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.AdOrder
{

    public class RespOrderDto
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public string BillingRuleName { get; set; }

        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime ReceiveTime { get; set; }
        public decimal CPCPrice { get; set; }
        public decimal CPLPrice { get; set; }
    }


    public class RespOrderCoverImageDto : RespOrderDto
    {
        public string MediaName { get; set; }
        
    }

}
