/********************************************************
*创建人：lixiong
*创建时间：2017/9/30 16:56:55
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
    public class RespDemandCarAndCitysDto
    {
        public int DemandBillNo { get; set; }

        public List<DeliveryBrandInfoDto> BrandInfos { get; set; }
        public List<DeliveryCarInfoDto> SerieInfos { get; set; }
        public List<DeliveryProvinceInfoDto> ProvinceInfos { get; set; }
        public List<DeliveryCitysInfoDto> CitysInfos { get; set; }
    }
}