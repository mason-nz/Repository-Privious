using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.POBU.Chitunion2018.AdminWebAPI.Common
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            //base.OnPreInit(e);

            //获取登录信息
            IsLogin();
        }

        private void IsLogin()
        {
            ITSC.Chitunion2017.Common.LoginUser userInfo = new ITSC.Chitunion2017.Common.LoginUser();

            bool returnBool = ITSC.Chitunion2017.Common.UserInfo.IsLogin(out userInfo);
            if (!returnBool)
            {
                Response.Write("您没有权限,请登录");
                Response.End();
            }
        }


    }
}