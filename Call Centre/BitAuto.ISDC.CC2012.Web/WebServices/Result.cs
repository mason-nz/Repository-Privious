using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// 返回结果类
    /// <summary>
    /// 返回结果类
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public Result()
        {
        }

        public Result(bool s, string msg)
        {
            Success = s;
            Message = msg;
        }

        public override string ToString()
        {
            return "Success：" + Success + " Message：" + Message;
        }
    }
}
