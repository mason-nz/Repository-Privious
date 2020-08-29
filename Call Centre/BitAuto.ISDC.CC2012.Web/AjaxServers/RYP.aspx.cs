using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.YPSuperLoginService;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public partial class RYP : PageBase
    {

        public string NeedRedirect = "1";

        public string TID
        {
            get
            {
                return HttpContext.Current.Request["tid"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["tid"].ToString());
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lbError.Visible = false;
            SuperLoginServiceClient client = new SuperLoginServiceClient();
            try
            {                
                var tokent = client.GetAccessTokenInTypeByDealerId(Convert.ToInt32(TID), 2).m_AccessToken.ToString();
                token.Value = tokent;                
            }
            catch(Exception ex)
            {
                NeedRedirect = "0";
                lbError.Visible = true;
                BLL.Loger.Log4Net.Error("亚超登录-调用接口报错,会员编号为："+TID,ex);
            }

            wkopuserid.Value = BLL.Util.GetLoginUserID().ToString(); ;

        }
    }
}