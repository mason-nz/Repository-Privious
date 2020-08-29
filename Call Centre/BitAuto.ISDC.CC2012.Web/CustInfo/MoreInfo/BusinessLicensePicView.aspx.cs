using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class BusinessLicensePicView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// CRM系统URL
        /// </summary>
        public string CRMSiteURL = string.Empty;

        public int BLID
        {
            get { return BLL.Util.GetCurrentRequestQueryInt("BLID"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //BLHelper helper = new BLHelper();
            //Entities.BusinessLicense bl = helper.GetOne();
            string sysid = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("CRMSysID");
            if (!string.IsNullOrEmpty(sysid))
            {
                CRMSiteURL = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetSysURLBySysID(sysid);
            }

            //取营业执照照片新方法
            IList<BitAuto.YanFa.Crm2009.Entities.AttachFile> listFile = null;
            listFile = BitAuto.YanFa.Crm2009.BLL.BusinessLicense.Instance.GetAttachFile(BLID);
            if (listFile != null)
            {
                this.repeaterFileDocList.DataSource = listFile;
                this.repeaterFileDocList.DataBind();
            }

        }
    }
}