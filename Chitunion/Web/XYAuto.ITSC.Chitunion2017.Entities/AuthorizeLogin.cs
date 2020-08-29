using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    public class AuthorizeLogin
    {
        public int RecID { get; set; } = Constants.Constant.INT_INVALID_VALUE;
        public int APPID { get; set; } = Constants.Constant.INT_INVALID_VALUE;
        public string IP { get; set; } = Constants.Constant.STRING_EMPTY_VALUE;
        public int TimeStamp { get; set; } = 0;
        public string MD5Code { get; set; } = Constants.Constant.STRING_EMPTY_VALUE;
        public int Status { get; set; } = 0;
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
    }
}
