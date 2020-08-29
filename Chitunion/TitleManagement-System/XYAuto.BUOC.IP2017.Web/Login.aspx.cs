using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.BUOC.IP2017.Web
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
            ITSC.Chitunion2017.Common.LoginUser lu = new ITSC.Chitunion2017.Common.LoginUser();
            try
            {
                if (ITSC.Chitunion2017.Common.UserInfo.IsLogin(out lu) &&
                    lu.UserID > 0 && lu.Category == 29003)
                {
                    DataTable dtParent = ITSC.Chitunion2017.Common.UserInfo.Instance.GetParentModuleInfoByUserID(lu.UserID);
                    if (dtParent != null)
                    {
                        if (string.IsNullOrEmpty(dtParent.Rows[0]["ModuleUrl"].ToString()))
                        {
                            DataTable dtChild = ITSC.Chitunion2017.Common.UserInfo.Instance.GetChildModuleByUserId(lu.UserID, dtParent.Rows[0]["moduleid"].ToString());
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
                else
                {
                    Response.Write("当前用户没有权限访问");
                    Response.End();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}