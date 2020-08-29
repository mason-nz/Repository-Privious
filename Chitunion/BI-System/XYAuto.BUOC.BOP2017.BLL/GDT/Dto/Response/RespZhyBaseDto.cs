/********************************************************
*创建人：lixiong
*创建时间：2017/8/17 18:07:30
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

namespace XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Response
{
    public class RespZhyBaseDto<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}