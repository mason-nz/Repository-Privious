using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.EPTask
{
    public partial class EPHBugCarTaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string YPFanXianURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCCHBugCar_URL");//易湃签入CC惠买车任务页面，APPID
        public string TaskURL = ConfigurationUtil.GetAppSettingValue("EPEmbedCC_HBuyCarTaskURL");//易湃签入CC惠买车任务页面，APPID
        public string EPEmbedCC_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC惠买车页面，APPID
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}