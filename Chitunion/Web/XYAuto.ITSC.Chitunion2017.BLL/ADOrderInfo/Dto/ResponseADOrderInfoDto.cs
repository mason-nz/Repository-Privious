using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto
{
    public class ResponseADOrderInfoDto
    {
        public ResponseADOrderDto ADOrderInfo { get; set; }
        public List<ResponseMediaOrderInfoDto> MediaOrderInfos { get; set; }
        public List<ResponseSubADInfoDto> SubADInfos { get; set; }
    }
    public class ResponseADOrderDto
    {
        public int OrderType { get; set; } = -2;
        public string OrderID { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public decimal TotalAmount { get; set; } = 0;
        public decimal CostTotal { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; } = -2;
        public string RejectMsg { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        public string CRMCustomerID { get; set; } = string.Empty;
        public string CustomerText { get; set; } = string.Empty;
        [JsonIgnore]
        public int CustomerIDINT { get; set; } = -2;
        public string CreatorName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CreatorUserName { get; set; } = string.Empty;
        public string CustomerUserName { get; set; } = string.Empty;
        public string MarketingPolices { get; set; } = string.Empty;
        public string MarketingUrl { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
        public string UploadFileName { get; set; } = string.Empty;
    }
    public class ResponseMediaOrderInfoDto
    {
        public int MediaType { get; set; } = -2;
        public string Note { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
        public string UploadFileName { get; set; } = string.Empty;
    }
    public class ResponseSubADInfoDto
    {
        public string SubOrderID { get; set; } = string.Empty;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string MediaOwner { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } = 0;
        public int Status { get; set; } = -2;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; } = -2;
        public decimal CostTotal { get; set; } = 0;
        public int ChannelID { get; set; } = -2;
        public string ChannelName { get; set; } = string.Empty;
        public List<ResponseSelfDetailDto> SelfDetails { get; set; }
        public List<ResponseAPPDetailDto> APPDetails { get; set; }
        public List<ResponseOperateInfoDto> OperateInfo { get; set; }
    }
    public class ResponseSelfDetailDto
    {
        [JsonIgnore]
        public int ADDetailID { get; set; } = -2;
        public string IsAuth { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int PubID { get; set; } = -2;
        public DateTime PubBeginTime { get; set; }
        public DateTime PubEndTime { get; set; }
        public int PublishDetailID { get; set; } = -2;
        public int HasOtherPublish { get; set; } = 0;
        public string ADMasterImage { get; set; } = string.Empty;
        public string ADMasterTitle { get; set; } = string.Empty;
        public string ADPosition { get; set; } = string.Empty;
        public string CreateType { get; set; } = string.Empty;
        public int ADPositionID { get; set; } = -2;
        public int CreateTypeID { get; set; } = -2;
        public decimal OriginalPrice { get; set; } = 0;
        public decimal OriginalReferencePrice { get; set; } = 0;
        public decimal FinalCostPrice { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public bool EnableOriginPrice { get; set; } = false;
        public decimal AdjustPrice { get; set; } = 0;
        public decimal AdjustDiscount { get; set; } = 0;
        public decimal PurchaseDiscount { get; set; } = 0;
        public decimal SaleDiscount { get; set; } = 0;
        public int ADLaunchDays { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
        public int MediaStatus { get; set; } = -2;
        public int ChannelID { get; set; } = -2;
        public string ChannelName { get; set; } = string.Empty;
        public decimal CostReferencePrice { get; set; }
        public List<ResponseSelfADSchedule> ADSchedules { get; set; }
    }
    public class ResponseSelfADSchedule
    {
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
    }
    public class ResponseAPPDetailDto
    {
        [JsonIgnore]
        public int ADDetailID { get; set; } = -2;
        public int CPDCPM { get; set; } = -2;
        public string Source { get; set; } = string.Empty;
        public int PubID { get; set; } = -2;
        public DateTime PubBeginTime { get; set; }
        public DateTime PubEndTime { get; set; }
        public int PublishDetailID { get; set; } = -2;
        public string ADMasterImage { get; set; } = string.Empty;
        public string ADMasterTitle { get; set; } = string.Empty;
        public string ADStyle { get; set; } = string.Empty;
        public int CarouselNumber { get; set; } = -2;
        public string SalePlatform { get; set; } = string.Empty;
        public int SaleAreaID { get; set; } = -2;
        public string SaleArea { get; set; } = string.Empty;
        public int HasHoliday { get; set; } = 0;
        public decimal PriceHoliday { get; set; } = 0;
        public decimal OriginalPrice { get; set; } = 0;
        public decimal AdjustPrice { get; set; } = 0;
        public decimal AdjustDiscount { get; set; } = 0;
        public decimal PurchaseDiscount { get; set; } = 0;
        public decimal SaleDiscount { get; set; } = 0;
        public int ADLaunchDays { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; } = -2;
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
        public int MediaStatus { get; set; } = -2;
        public int HasOtherPublish { get; set; } = 0;
        public int TemplateID { get; set; } = -2;
        public List<ResponseAPPADScheduleDto> ADSchedules { get; set; }
    }
    public class ResponseAPPADScheduleDto
    {
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
        public int AllDays { get; set; } = 0;
        public int Holidays { get; set; } = 0;
        public List<ResponseHolidayDto> Holiday { get; set; }
    }
    public class ResponseHolidayDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
    }
    public class ResponseOperateInfoDto
    {
        public int LastOrderStatus { get; set; } = -2;
        public int OrderStatus { get; set; } = -2;
        public string Creator { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
    }
    public class ResponseGetSubADinfoDto
    {
        public ResponseADOrderDto ADOrderInfo { get; set; }
        public ResponseMediaOrderInfoDto MediaOrderInfo { get; set; }
        public ResponseSubADInfoDto SubADInfo { get; set; }
    }
}
