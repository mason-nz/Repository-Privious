using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace BitAuto.ISDC.CC2012.WebService.YiCheMall
{
    public class CCLoginHelper
    {
        #region Instance
        public static readonly CCLoginHelper Instance = new CCLoginHelper();
        #endregion

        private string YICHEMALLLoginURL = System.Configuration.ConfigurationManager.AppSettings["YICHEMALLLoginURL"];

        public string CCLogin(string domainName)
        {
            return BLL.Util.HttpGet(YICHEMALLLoginURL, "domainAccount=" + domainName);
        }
    }
}
