using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class RequestIntelligenceRecommendDto
    {
        public List<RequestIntelligenceRecommendAreaInfoDto> AreaInfo { get; set; }
        public decimal BudgetTotal { get; set; } = 0;
        public string OrderRemark { get; set; } = string.Empty;
        public string MasterBrand { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public string CarSerial { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public int JKEntrance { get; set; } = 0;
    }
    public class RequestIntelligenceRecommendAreaInfoDto
    {
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public decimal Budget { get; set; } = 0;
        public int MediaCount { get; set; } = 0;
        public bool OriginContain { get; set; } = false;
    }
    public class ResponseIntelligenceRecommendDto
    {
        public int ProvinceID { get; set; } = -2;
        public string ProvinceName { get; set; } = string.Empty;
        public int CityID { get; set; } = -2;
        public string CityName { get; set; } = string.Empty;
        public List<ResponseIntelligenceRecommendDetailDto> PublishDetails { get; set; }
    }
    public class ResponseIntelligenceRecommendDetailDto
    {
        public int ProvinceID { get; set; } = -2;
        public string ProvinceName { get; set; } = string.Empty;
        public int CityID { get; set; } = -2;
        public string CityName { get; set; } = string.Empty;
        public int PublishDetailID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string HeadIconURL { get; set; } = string.Empty;
        public int FansCount { get; set; } = 0;
        public int AverageReadCount { get; set; } = 0;
        public string ADPosition { get; set; } = string.Empty;
        public string CreateType { get; set; } = string.Empty;
        public decimal SalePrice { get; set; } = 0;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal OriginalReferencePrice { get; set; } = 0;
    }
    public class PubDetailCostPriceDto
    {
        public int BaseMediaID { get; set; } = -2;
        public int PublishDetailID { get; set; } = -2;
        public decimal CostReferencePrice { get; set; } = 0;
    }
    public class PubDetailIDResultDto
    {
        public string PublishDetailIDs { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; } = 0;
        public decimal AbsPrice { get; set; } = 0;
        public int OrderBy { get; set; } = 0;
    }
    public class RequestIntelligenceADOrderInfoCrudDto
    {        
        public EnumIntelligenceADOrderInfoCrudOptType optType { get; set; } = EnumIntelligenceADOrderInfoCrudOptType.ADD;
        public RequestIntelligenceADOrderDto ADOrderInfo { get; set; }
        public List<RequestIntelligenceADOrderCityDto> ADDetails { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (!System.Enum.IsDefined(typeof(EnumIntelligenceADOrderInfoCrudOptType), optType))
                sb.Append($"操作类型参数错误!{optType}");

            if (optType != EnumIntelligenceADOrderInfoCrudOptType.ADDADOrderNote)
            {
                if(ADDetails==null || ADDetails.Count==0)
                    sb.Append("新增修改智投项目广告位至少有一个!");
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
    public class RequestIntelligenceADOrderCityDto
    {
        public int ProvinceID { get; set; } = -2;
        public int CityID { get; set; } = -2;
        public decimal Budget { get; set; } = 0;
        public int MediaCount { get; set; } = 0;
        public bool OriginContain { get; set; } = false;
        public List<RequestIntelligencePublishDetailDto> PublishDetails { get; set; }
    }

    public class RequestIntelligencePublishDetailDto
    {
        public int PublishDetailID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public decimal AdjustPrice { get; set; } = 0;
        public bool EnableOriginPrice { get; set; } = false;
        public int ChannelID { get; set; } = -2;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public decimal FinalCostPrice { get; set; } = 0;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
    }
    public class RequestIntelligenceADOrderDto
    {
        public string OrderID { get; set; } = string.Empty;
        public string OrderName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int Status { get; set; } = -2;
        public string CustomerID { get; set; } = string.Empty;
        [JsonIgnore]
        public int CustomerIDINT { get; set; } = -2;
        public string MarketingPolices { get; set; } = string.Empty;
        public string MarketingUrl { get; set; } = string.Empty;
        public string UploadFileURL { get; set; } = string.Empty;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public string CRMCustomerID { get; set; } = string.Empty;
        public string CustomerText { get; set; } = string.Empty;
        public decimal BudgetTotal { get; set; } = 0;
        public string OrderRemark { get; set; } = string.Empty;
        public int MasterID { get; set; } = -2;
        public int BrandID { get; set; } = -2;
        public int SerialID { get; set; } = -2;
        public string MasterName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string SerialName { get; set; } = string.Empty;
        public bool JKEntrance { get; set; } = false;
    }    
}
