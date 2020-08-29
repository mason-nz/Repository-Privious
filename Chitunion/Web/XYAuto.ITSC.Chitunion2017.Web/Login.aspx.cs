using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoginLogic();
            }
        }

        private void LoginLogic()
        {
            string gourl = string.Empty;
            //int userid = -1;
            //int category = -1;
            Chitunion2017.Common.LoginUser lu = new Chitunion2017.Common.LoginUser();
            try
            {
                if (Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                    lu.UserID > 0)
                {
                    Chitunion2017.Common.UserRole ur = new Chitunion2017.Common.UserRole(lu.UserID, lu.RoleIDs);
                    if (lu.Category == 29001 && ur != null && ur.IsAD)//广告主跳转统一这个页面
                    {
                        gourl = "/OrderManager/wx_list.html";
                        Response.Redirect(gourl, false);
                    }
                    else
                    {
                        DataTable dtParent = Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(lu.UserID);
                        if (dtParent != null)
                        {
                            if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                            {
                                DataTable dtChild = Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(lu.UserID, dtParent.Rows[0]["moduleid"].ToString());
                                if (dtChild.Rows.Count > 0)
                                {
                                    gourl = dtChild.Rows[0]["url"].ToString();
                                }
                            }
                            else
                            {
                                gourl = dtParent.Rows[0]["url"].ToString();
                            }
                            Response.Redirect(gourl, false);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}