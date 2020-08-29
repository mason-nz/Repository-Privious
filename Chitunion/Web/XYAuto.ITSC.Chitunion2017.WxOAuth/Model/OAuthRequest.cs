using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.WxOAuth.Model
{
    public class OAuthRequest
    {
        public int Type { get; set; }
        public string YZM { get; set; }
        public string Url { get; set; }
        public string WxNumber { get; set; }
        public string jsonpcallback { get; set; }
    }
}