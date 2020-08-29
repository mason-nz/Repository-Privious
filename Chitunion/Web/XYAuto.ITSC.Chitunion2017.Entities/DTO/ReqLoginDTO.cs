using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    public class ReqLoginDTO
    {
        public string mobile { get; set; }
        public string mobileCheckCode { get; set; }
        public string openid { get; set; } = string.Empty;
        public string unionid { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string nickname { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string headimgurl { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string Ip { get; set; }
    }
}
