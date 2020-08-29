using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.ShoppingCart.Dto
{
    public class ResponseQueryShoppingDto
    {
        public decimal TotalAmount { get; set; } = 0;
        public List<ResponseQuerySelfMediaDto> SelfMedia { get; set; }
        public List<ResponseQueryAPPMediaDto> APP { get; set; }
    }
    public class ResponseQuerySelfMediaDto
    {
        public string MediaOwner { get; set; } = string.Empty;
        public List<ResponseQuerySelfItemDto> Medias { get; set; }
    }
    public class ResponseQuerySelfItemDto
    {
        [JsonIgnore]
        public string Source { get; set; } = string.Empty;
        public int CartID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string Name { get; set; }
        public int PublishDetailID { get; set; } = -2;
        public string IsAuth { get; set; } = string.Empty;
        public string ADMasterImage { get; set; } = string.Empty;
        public string ADMasterTitle { get; set; } = string.Empty;
        public string ADPosition { get; set; } = string.Empty;
        public string CreateType { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
        public decimal TotalAmmount { get; set; } = 0;
        public int IsSelected { get; set; } = 0;
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
        public int MediaStatus { get; set; } = -2;
        public int HasOtherPublish { get; set; } = 0;
        public DateTime PubBeginTime { get; set; }
        public DateTime PubEndTime { get; set; }
        public List<ResponseQuerySelfADScheduleDto> ADSchedule { get; set; }
    }
    public class ResponseQuerySelfADScheduleDto
    {
        public int RecID { get; set; } = -2;
        public DateTime BeginData { get; set; }
    }
    public class ResponseQueryAPPMediaDto
    {
        public string MediaOwner { get; set; } = string.Empty;
        public List<ResponseQueryAPPItemDto> Medias { get; set; }
    }
    public class ResponseQueryAPPItemDto
    {
        public int CartID { get; set; } = -2;
        public int MediaType { get; set; } = -2;
        public int MediaID { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public int PublishDetailID { get; set; } = -2;
        public int CPDCPM { get; set; }=-2;
        public string ADMasterImage { get; set; } = string.Empty;
        public string ADMasterTitle { get; set; } = string.Empty;
        public string ADStyle { get; set; } = string.Empty;
        public string SaleArea { get; set; } = string.Empty;
        public int SaleAreaID { get; set; } = -2;
        public int CarouselNumber { get; set; } = -2;
        public string SalePlatform { get; set; } = string.Empty;
        public int ADLaunchDays { get; set; } = 0;
        public int HasHoliday { get; set; } = 0;
        public decimal PriceHoliday { get; set; } = 0;
        public decimal Price { get; set; } = 0;
        public decimal TotalAmmount { get; set; } = 0;
        public int IsSelected { get; set; } = 0;
        public int expired { get; set; } = 0;
        public int PublishStatus { get; set; } = -2;
        public int MediaStatus { get; set; } = -2;
        public string Source { get; set; } = string.Empty;
        public DateTime PubBeginTime { get; set; }
        public DateTime PubEndTime { get; set; }
        public int HasOtherPublish { get; set; } = 0;
        public int TemplateID { get; set; } = -2;
        public List<ResponseQueryAPPADScheduleDto> ADSchedule { get; set; }
    }    
    public class ResponseQueryAPPADScheduleDto
    {
        public int RecID { get; set; } = -2;
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
        public int AllDays { get; set; } = 0;
        public int Holidays { get; set; } = 0;
        public List<ResponseQueryHolidayDto> Holiday { get; set; }

    }
    public class ResponseQueryHolidayDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime BeginData { get; set; }
        public DateTime EndData { get; set; }
    }
}
