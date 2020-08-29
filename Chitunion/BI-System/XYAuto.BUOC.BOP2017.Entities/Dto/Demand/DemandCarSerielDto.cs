/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 11:19:32
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto.Demand
{
    public class DemandCarSerielDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public List<CarserialInfo> CarSerialInfo { get; set; }
    }

    public class CarserialInfo
    {
        public int CarSerialId { get; set; }
        public string CarSerialName { get; set; }
    }
}