/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 11:52:47
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.BOP2017.Entities.Demand
{
    public class DemandGroundDeliveryExt : DemandGroundDelivery
    {
        /* 冗余 */
        public string DemandName { get; set; }
        public string DeliveryTypeName { get; set; }
        public string AdSiteSetName { get; set; }
        public string AdCreativeName { get; set; }

        public string CarInfo { get; set; }
        public string CityInfo { get; set; }

        public int BrandId { get; set; }
        public int SerielId { get; set; }//
        public int ProvinceId { get; set; }
        public int CityId { get; set; }

        public int AdgroupId { get; set; }
        public int AuditStatus { get; set; }
        public string AuditStatusName { get; set; }
        public string AdgroupName { get; set; }
        public string CampaignName { get; set; }
    }
}