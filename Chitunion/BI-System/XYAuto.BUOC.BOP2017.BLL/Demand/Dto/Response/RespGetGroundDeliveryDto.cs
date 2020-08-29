/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 20:00:39
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response
{
    public class RespGetGroundDeliveryDto
    {
        public int DemandBillNo { get; set; }
        public string DemandName { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusName { get; set; }
        public List<GroundInfoDto> GroundInfo { get; set; }
    }

    public class GroundInfoDto
    {
        public int GroundId { get; set; }
        public string PromotionUrl { get; set; }
        public List<DeliveryCarInfoDto> CarInfo { get; set; }
        public List<DeliveryCitysInfoDto> AreaInfo { get; set; }
        public List<DeliveryInfoDto> DeliveryList { get; set; }
    }

    public class DeliveryBrandInfoDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }

    public class DeliveryProvinceInfoDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
    }

    public class DeliveryCarInfoDto : DeliveryBrandInfoDto
    {
        public int SerielId { get; set; }
        public string SerialName { get; set; }
    }

    public class DeliveryCitysInfoDto : DeliveryProvinceInfoDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class DeliveryInfoDto
    {
        public int DeliveryId { get; set; }
        public string DeliveryTypeName { get; set; }
        public string AdSiteSetName { get; set; }
        public string AdCreativeName { get; set; }
        public string AdName { get; set; }
        public string PromotionUrl { get; set; }
        public string CampaignName { get; set; }
        public string AdgroupName { get; set; }
        public int AdgroupId { get; set; }
    }
}