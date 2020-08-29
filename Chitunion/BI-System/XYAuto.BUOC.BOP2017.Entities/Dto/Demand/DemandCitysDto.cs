/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 11:21:44
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Entities.Dto.Demand
{
    public class DemandCitysDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public List<CityItem> City { get; set; }
    }

    public class CityItem
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}