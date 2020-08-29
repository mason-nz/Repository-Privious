using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Media
{
    public class RespWeiXinBindingsDto
    {
        public int MediaId { get; set; }
        public string HeadImg { get; set; }
        public string WxNumber { get; set; }
        public string NickName { get; set; }
        public PriceWeiXinInfo First { get; set; }
        public PriceWeiXinInfo Second { get; set; }
        public PriceWeiXinInfo Third { get; set; }
        public PriceWeiXinInfo Fourth { get; set; }
        [JsonIgnore]
        public string PricesInfo { get; set; }
    }
             

    public class PriceWeiXinInfo
    {
        public decimal Forward { get; set; }       //转发
        public decimal OriginalPublish { get; set; }      //原创 + 发布
    }

}
