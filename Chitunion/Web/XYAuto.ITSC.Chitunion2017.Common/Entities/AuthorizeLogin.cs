using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Common.Entities
{
    public class AuthorizeLogin
    {
        public int RecID { get; set; } = -2;
        public int APPID { get; set; } = -2;
        public string IP { get; set; } = string.Empty;
        public long TimeStamp { get; set; } = 0;
        public string MD5Code { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
