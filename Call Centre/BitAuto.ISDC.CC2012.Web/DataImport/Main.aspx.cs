using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.DataImport
{
    public partial class Main : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string SessionUserID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //增加“客户池--个人客户”导入功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024BUT2102"))
            {
                int UserID = BLL.Util.GetLoginUserID();
                SessionUserID = UserID.ToString();
            }
            else
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }
    }
}