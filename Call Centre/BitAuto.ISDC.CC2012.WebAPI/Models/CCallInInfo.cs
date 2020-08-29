using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.ISDC.CC2012.WebAPI.Models
{
    public class CCallInInfo
    {
        public string Result;
        public string Id;
        public string mphone;
        public string voiceCode;
        public string skillId;
        public string ivrNo;
        public DateTime CreateTime = DateTime.Now;

        public override string ToString()
        {            
            return string.Format("Result:{0},id:{1},mphone:{2},voiceCode:{3},skillId:{4},ivrNo:{5}", Result, Id, mphone, voiceCode, skillId, ivrNo);            
        }
    }
}