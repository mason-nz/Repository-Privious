using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class MainListAllUser : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string Pwd
        {
            get { return BLL.Util.GetCurrentRequestStr("pwd"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Pwd.ToLower().Equals("3219jdhliweruy@hfkjhi@oghg"))
                {
                    BitAuto.Utils.ScriptHelper.ShowAlertScript("您无此页面权限！");
                    Response.End();
                }
                else
                {
                    DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID(ConfigurationUtil.GetAppSettingValue("ThisSysID"));
                    Rpt_Role.DataSource = db;
                    Rpt_Role.DataBind();
                }
            }
            
        }
    }
}