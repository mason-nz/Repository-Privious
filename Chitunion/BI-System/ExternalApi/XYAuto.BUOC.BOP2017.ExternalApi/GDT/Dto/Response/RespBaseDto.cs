/********************************************************
*创建人：lixiong
*创建时间：2017/8/14 15:24:12
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using Newtonsoft.Json;

namespace XYAuto.BUOC.BOP2017.ExternalApi.GDT.Dto.Response
{
    public class RespBaseDto
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class RespBaseDto<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}