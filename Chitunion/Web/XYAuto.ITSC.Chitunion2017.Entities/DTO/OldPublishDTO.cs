using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Publish;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class OldPublishDTO
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set;}
        public decimal PurchaseDiscount { get; set; }
        public decimal SaleDiscount { get; set; }
        public bool IsAppointment { get; set; }
        public string OrderRemarkName { get; set; }
        public List<OldADDetailDTO> Details { get; set; }
        public List<AppPriceInfo> Prices { get; set; }
    }

    public class OldADDetailDTO {

        public string ADPosition { get; set; }
        public string ADPositionName { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
    }
}
