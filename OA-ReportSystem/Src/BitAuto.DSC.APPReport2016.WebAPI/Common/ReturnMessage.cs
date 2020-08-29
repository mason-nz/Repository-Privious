using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.DSC.APPReport2016.WebAPI
{
    public class ReturnMessage
    {
        public  ReturnMessage()
        {
            Success = true;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}