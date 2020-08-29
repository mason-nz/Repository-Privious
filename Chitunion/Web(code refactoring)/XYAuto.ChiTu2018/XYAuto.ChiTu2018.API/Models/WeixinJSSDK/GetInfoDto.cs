using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.ChiTu2018.API.Models.WeixinJSSDK
{
    public class GetInfoDto
    {
        public string AppId { get; set; }

        public string NonceStr { get; set; }

        public string Timestamp { get; set; }

        public string Signature { get; set; }
    }
}