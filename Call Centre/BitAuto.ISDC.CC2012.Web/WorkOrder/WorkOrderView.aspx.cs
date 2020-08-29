using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class WorkOrderView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string OrderID
        {
            get
            {
                if (Request["OrderID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["OrderID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entities.WorkOrderInfo WorkOrderInfoModel = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                string viewpage = "";
                if (WorkOrderInfoModel != null)
                {
                    if (WorkOrderInfoModel.WorkCategory == 1)
                    {
                        viewpage = "/WorkOrder/WorkOrderViewPersonal.aspx?OrderID=" + OrderID;
                    }
                    else if (WorkOrderInfoModel.WorkCategory == 2)
                    {
                        viewpage = "/WorkOrder/WorkOrderViewDealer.aspx?OrderID=" + OrderID;
                    }
                    Response.Redirect(viewpage);
                }
                else
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('工单不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }

            }
        }
    }
}