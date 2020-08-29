using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class GetADDetailResDTO
    {
        public int BaseMediaID { get; set; }
        public List<TemplateItem> TemplateList { get; set; }
        public PublishBasicItem PublishBasicInfo { get; set; }
        public List<PriceItem> PriceList { get; set; }
        public PubAuditItem PubAuditInfo { get; set; }
    }

    public class TemplateItem {
        public string MediaName { get; set; }
        public string MediaLogo { get; set; }
        //public int MediaID { get; set; }
        public int TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string AuditStatusName { get; set; }
        public string AdFormName { get; set; }
        public string AdLegendURL { get; set; }
        public int CarouselCount { get; set; }
        public List<KeyValueItem> SellingModeList { get; set; }
        public List<KeyValueItem> SellingPlatformList { get; set; }
        public List<KeyValueItem> AdStyleList { get; set; }
        public List<KeyValueItem> AdGroupList { get; set; }
        public List<PubDateItem> PubDateList { get; set; }
        
        //sql里拼接的
        public string SaleAreaStr { get; set; }
        public string TemplateStyleStr { get; set; }
        public string SellingModeStr { get; set; }
        public string SellingPlatformStr { get; set; }
    }

    public class PubDateItem {
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string PubFileUrl { get; set; }
        public string FileName { get; set; }
    }

    public class PublishBasicItem
    {
        public int PubID { get; set; }
        public int MediaID { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal PurchaseDiscount { get; set; }
        public decimal SaleDiscount { get; set; }
        public string PubFileUrl { get; set; }
        public bool HasHoliday { get; set; }

        public string ADName { get; set; }
        public string MediaName { get; set; }
        public string MediaLogo { get; set; }
        public string PubFileName { get; set; }
        public string SubmitUserName { get; set; }
    }

    public class PubAuditItem
    {
        public int OptType { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public string RejectMsg { get; set; }
    }

    public class PriceItem
    {
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

    public class KeyValueItem {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
