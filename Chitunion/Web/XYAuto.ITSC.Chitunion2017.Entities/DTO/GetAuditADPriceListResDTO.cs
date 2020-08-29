using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetAuditADPriceListResDTO
    {
        public OtherItem ADInfo { get; set; }
        public List<PriceListItem> List {get;set;}
    }

    public class OtherItem
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string MediaName { get; set; }
        public string MediaLogo { get; set; }
        public string PubFileUrl { get; set; }
        public string PubFileName { get; set; }
        public string SubmitUserName { get; set; }
    }

    public class PriceListItem {
        public PublishBasicItem PublishBasicInfo { get; set; }
        public List<PriceItem> PriceList { get; set; }
    }

    public class PublishPriceItem {

        public string MediaName { get; set; }
        public string MediaLogo { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PubFileUrl { get; set; }
        public string PubFileName { get; set; }
        public string SubmitUserName { get; set; }

        public int PubID { get; set; }
        public string ADName { get; set; }
        public decimal PurchaseDiscount { get; set; }
        public decimal SaleDiscount { get; set; }
        public bool HasHoliday { get; set; }

        public int RecID { get; set; }
        public int CarouselNumber { get; set; }
        public int SaleType { get; set; }
        public string SaleTypeName { get; set; }
        public int SalePlatform { get; set; }
        public string SalePlatformName { get; set; }
        public int ADStyle { get; set; }
        public string ADStyleName { get; set; }
        public int SaleArea { get; set; }
        public string SaleAreaName { get; set; }
        public int ClickCount { get; set; }
        public int ExposureCount { get; set; }
        public decimal PubPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal PubPrice_Holiday { get; set; }
        public decimal SalePrice_Holiday { get; set; }
        public string RowState { get; set; }

    }
}
