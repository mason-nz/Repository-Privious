/********************************************************
*创建人：lixiong
*创建时间：2017/8/18 11:46:49
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request
{
    public class RequestBaseTokenDto
    {
        [Necessary(MtName = "AccessToken")]
        public string AccessToken { get; set; }

        [Necessary(MtName = "P")]
        public string P { get; set; }

        [Necessary(MtName = "Appid", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int Appid { get; set; }

        [Necessary(MtName = "Sign")]
        public string Sign { get; set; }
    }
}