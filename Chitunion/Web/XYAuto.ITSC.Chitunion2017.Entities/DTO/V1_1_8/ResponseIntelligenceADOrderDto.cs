using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class IntelligenceADOrderInfoQueryDto
    {
        public ResponseIntelligenceADOrderDto ADOrderInfo { get; set; }
        public List<ResponseIntelligenceADOrderAreaInfoDto> AreaInfos { get; set; }
    }
    public class ResponseIntelligenceADOrderDto
    {
        public int OrderType { get; set; } = -2;
        public string OrderID { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public decimal TotalAmount { get; set; } = 0;
        public string MarketingPolices { get; set; } = string.Empty;
        public string MarketingUrl { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
        public string UploadFileName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; } = -2;
        public string RejectMsg { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        [JsonIgnore]
        public int CustomerIDINT { get; set; } = -2;
        public string CRMCustomerID { get; set; } = string.Empty;
        public string CustomerText { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CreatorUserName { get; set; } = string.Empty;
        public string CustomerUserName { get; set; } = string.Empty;       
        public string OrderRemark { get; set; } = string.Empty;
        public int MasterID { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string MasterName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public bool JKEntrance { get; set; } = false;
        public decimal BudgetTotal { get; set; } = 0;
        public decimal CostTotal { get; set; } = 0;
        //public List<ResponseIntelligenceADOrderAreaInfoDto> AreaInfos { get; set; }
    }
    public class ResponseIntelligenceADOrderAreaInfoDto
    {
        public int CityExtendID { get; set; } = -2;
        public int ProvinceID { get; set; } = -2;
        public string ProvinceName { get; set; } = string.Empty;
        public int CityID { get; set; } = -2;
        public string CityName { get; set; } = string.Empty;
        public decimal Budget { get; set; } = 0;
        public int MediaCount { get; set; } = 0;
        public bool OriginContain { get; set; } = false;
        public List<ResponseIntelligencePublishDetailDto> PublishDetails { get; set; }
    }
    public class ResponseIntelligencePublishDetailDto
    {
        public int PublishDetailID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string HeadIconURL { get; set; } = string.Empty;
        public int FansCount { get; set; } = 0;
        public int AverageReadCount { get; set; } = 0;
        public string ADPosition { get; set; } = string.Empty;
        public int ADPositionID { get; set; } = -2;
        public string CreateType { get; set; } = string.Empty;
        public int CreateTypeID { get; set; } = -2;
        public decimal SalePrice { get; set; } = 0;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal OriginalReferencePrice { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public decimal FinalCostPrice { get; set; } = 0;
        public bool EnableOriginPrice { get; set; } = false;
        public int ADLaunchDays { get; set; } = 0;
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
        public int MediaStatus { get; set; } = -2;
        public decimal AdjustPrice { get; set; } = 0;
        public int ChannelID { get; set; } = -2;
        public string ChannelName { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);

    }
}
