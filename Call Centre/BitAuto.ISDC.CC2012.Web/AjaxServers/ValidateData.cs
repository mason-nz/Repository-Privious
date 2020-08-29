using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public class ValidateData
    {
        private List<ControlInfo> controlInfo;

        public List<ControlInfo> ControlInfo
        {
            get { return controlInfo; }
            set { controlInfo = value; }
        }
    }

    public class ControlInfo
    {
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        private string controlType;

        public string ControlType
        {
            get { return controlType; }
            set { controlType = value; }
        }
        private string vType;

        public string VType
        {
            get { return vType; }
            set { vType = value; }
        }
        private string vMsg;

        public string VMsg
        {
            get { return vMsg; }
            set { vMsg = value; }
        }
        private string optionLen;

        public string OptionLen
        {
            get { return optionLen; }
            set { optionLen = value; }
        }
        private string firstOptionVal;

        public string FirstOptionVal
        {
            get { return firstOptionVal; }
            set { firstOptionVal = value; }
        }
        private string maxLen;

        public string MaxLen
        {
            get { return maxLen; }
            set { maxLen = value; }
        }
    }
}