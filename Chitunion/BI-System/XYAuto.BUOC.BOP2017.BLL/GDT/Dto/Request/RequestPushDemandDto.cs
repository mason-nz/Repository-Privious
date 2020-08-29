/********************************************************
*创建人：lixiong
*创建时间：2017/8/16 13:53:59
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request
{
    public class RequestPushDemandDto : RequestDemandBillNoDto
    {
        [Necessary(MtName = "需求名称DemandName")]
        public string DemandName { get; set; }

        public List<CarInfoDto> CarInfo { get; set; }
        public List<AreaInfoDto> AreaInfo { get; set; }
        public List<DistributorDto> Distributor { get; set; }

        public string PromotionPolicy { get; set; }

        [Necessary(MtName = "DayBudget", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public decimal DayBudget { get; set; }

        public int ClueNumber { get; set; }

        [Necessary(MtName = "BeginDate")]
        public string BeginDate { get; set; }

        [Necessary(MtName = "EndDate")]
        public string EndDate { get; set; }

        [Necessary(MtName = "OrganizeId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OrganizeId { get; set; }
    }

    public class CarInfoDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public List<CarserialInfoDto> CarSerialInfo { get; set; }
    }

    public class CarserialInfoDto
    {
        public int CarSerialId { get; set; }
        public string CarSerialName { get; set; }
    }

    public class AreaInfoDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public List<CityDto> City { get; set; }
    }

    public class CityDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class DistributorDto
    {
        public int DistributorId { get; set; }
        public string DistributorName { get; set; }
    }
}