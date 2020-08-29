using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.BLL.V1_8_3.Dto
{
    public class IRecommendExportDto
    {
        public string ExcelName { get; set; } = string.Empty;
        public string MasterBrand { get; set; } = string.Empty;
        public string CarBrand { get; set; } = string.Empty;
        public string CarSerial { get; set; } = string.Empty;
        public decimal BudgetTotal { get; set; } = 0;
        public DateTime LaunchTime { get; set; } = new DateTime(1990, 1, 1);
        public bool JKEntrance { get; set; } = false;
        public List<IRecommendExportAreaInfoDto> AreaInfo { get; set; }
        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Empty;
            if (string.IsNullOrWhiteSpace(CarBrand))
                sb.Append($"品牌是必填项!");
            if (string.IsNullOrWhiteSpace(CarSerial))
                sb.Append($"车型是必填项!");

            if(AreaInfo==null || AreaInfo.Count==0)
                sb.Append($"投放城市至少一个!");
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
    public class IRecommendExportPublishDetailDto
    {
        public string MediaName { get; set; } = string.Empty;
        public string MediaNumber { get; set; } = string.Empty;
        public string ADPosition { get; set; } = string.Empty;
        public string CreateType { get; set; } = string.Empty;
        public int FansCount { get; set; } = 0;
        public int ADLaunchDays { get; set; } = 1;
        public decimal CostReferencePrice { get; set; } = 0;
        public decimal OriginalReferencePrice { get; set; } = 0;
    }
    public class IRecommendExportAreaInfoDto
    {
        public string ProvinceName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public List<IRecommendExportPublishDetailDto> PublishDetails { get; set; }
    }
}
