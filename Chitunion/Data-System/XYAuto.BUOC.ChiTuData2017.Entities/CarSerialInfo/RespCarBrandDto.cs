/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 20:16:33
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

namespace XYAuto.BUOC.ChiTuData2017.Entities.CarSerialInfo
{
    public class RespCarBrandDto
    {
        public int BrandId { get; set; }
        public int MasterId { get; set; }
        public string Name { get; set; }
    }

    public class RespCarSerialDto : RespCarBrandDto
    {
        public int CarSerialId { get; set; }
        public string ShowName { get; set; }
    }
}