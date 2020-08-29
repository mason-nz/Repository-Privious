using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class ProcessJudge : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        ///制作此页面的目的是供CRM调用，使其能跳转到指定的工单处理页面
        /// 注意：此页面在CRM中有两处在引用，修改需慎重
        public string Requester
        {
            get
            {
                if (Request["requester"] != null)
                {
                    return HttpUtility.UrlDecode(Request["requester"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string strOrderId = "";
            if (Request["OrderID"] != null)
            {
                strOrderId = Request["OrderID"].ToString();
            }
            BitAuto.YanFa.Crm2009.Entities.UserInfo user = BitAuto.YanFa.Crm2009.BLL.UserInfo.Get(BLL.Util.GetLoginUserID());
            //运营客服
            switch (user.UserClass)
            {
                case 2:
                case 7: Response.Redirect(GetRedirectUrl(user.UserID,strOrderId)); break;
                default: Response.Redirect("/WorkOrder/SalesProcess.aspx?OrderID=" + strOrderId + "&r=" + new Random().Next() + ((!string.IsNullOrEmpty(Requester) && Requester.Trim() == "intelligentplatform") ? "&requester=" + Requester : "")); break;
            }

        }
        private string GetRedirectUrl(int userid,string strOrderId)
        {
            string strUrl = string.Empty;

            DataTable dt = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserInfoByUserID(userid);
            string[] departIds = ConfigurationManager.AppSettings["PartID"].Split(',');
            DataTable dtAllDeparts = BLL.WorkOrderInfo.Instance.GetChildDepartMent(departIds);
            foreach (DataRow departRow in dtAllDeparts.Rows)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (departRow[0] != null && row["DepartID"] != null && departRow[0].ToString() == row["DepartID"].ToString())
                    {
                        strUrl = "/WorkOrder/CCProcess.aspx?OrderID=" + strOrderId + "&r=" + new Random().Next() + ((!string.IsNullOrEmpty(Requester) && Requester.Trim() == "intelligentplatform") ? "&requester=" + Requester : "");
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(strUrl))
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(strUrl))
            {
                strUrl = "/WorkOrder/SalesProcess.aspx?OrderID=" + strOrderId + "&r=" + new Random().Next() + ((!string.IsNullOrEmpty(Requester) && Requester.Trim() == "intelligentplatform") ? "&requester=" + Requester : "");
            }
            return strUrl;
        }
         
    }
}