/********************************************************
*创建人：lixiong
*创建时间：2017/10/30 11:20:03
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

namespace XYAuto.ITSC.Chitunion2017.BLL.ThirdApi.Dto.Response
{
    public class RespChituOpBaseDto<T>
    {
        public T Result { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}