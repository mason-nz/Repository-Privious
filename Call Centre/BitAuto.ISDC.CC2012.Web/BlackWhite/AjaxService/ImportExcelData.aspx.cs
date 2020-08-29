using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService
{
    public partial class ImportExcelData : PageBase
    {
        public string Type
        {
            get { return Request["Type"] == null ? string.Empty : HttpUtility.UrlDecode(Request["Type"].ToString()); }
        }
        public string PhoneNumber
        {
            get { return Request["PhoneNumber"] == null ? string.Empty : HttpUtility.UrlDecode(Request["PhoneNumber"].ToString()); }
        }
        public string strTempUrl = "/Statistics/MemberIDImport/Templet/黑白名单导入模板.xls";
        protected void Page_Load(object sender, EventArgs e)
        {
            // BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (Type == "0")
            {
                strTempUrl = "/Statistics/MemberIDImport/Templet/免打扰名单导入模板.xls";
            } 
        }
    }
}