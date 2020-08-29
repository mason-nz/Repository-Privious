using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.Utils.Config;
using System.Security.Cryptography;
using System.Text;
using BitAuto.ISDC.CC2012.WebService.EasyPass;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl
{
    public partial class DataListInfo : System.Web.UI.UserControl
    {
        public static string WebUrl = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress");

        public static string YPFanXianURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBugCar_URL");//易湃签入CC惠买车任务页面，APPID
        public static string EPEmbedCC_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC惠买车页面，APPID
        public static string HuiMaiChe_InBound_Url = ConfigurationUtil.GetAppSettingValue("HuiMaiChe_InBound_Url");//惠买车业务，呼入页面URL

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}