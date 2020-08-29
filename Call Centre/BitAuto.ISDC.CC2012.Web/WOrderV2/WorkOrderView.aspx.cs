using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2
{
    public partial class WorkOrderView : System.Web.UI.Page
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
                if (!string.IsNullOrEmpty(OrderID))
                {
                    WOrderInfoInfo modelout = BLL.WOrderInfo.Instance.GetWOrderInfoInfo(OrderID);
                    if (modelout != null)
                    {
                        ucCustInfoView.CustID = modelout.CBID;
                        ucCustInfoView.Telphone = modelout.Phone_Value;
                        ucCustInfoView.CanSeeTelImg = false;

                        ucWOrderBasicInfo.WOrderInfo = modelout;
                        ucWOrderBasicInfo.CustCategoryID = ucCustInfoView.CustType;
                        ucWOrderBasicInfo.CanSeeTelImg = false;

                        string orderStateName = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), (int)modelout.WorkOrderStatus_Value);
                        if (orderStateName == "待审核" || orderStateName == "已完成")
                        {
                            ucWOrderDealView.Visible = false;
                        }
                        else
                        {
                            ucWOrderDealView.OrderId = OrderID;
                        }
                    }
                    else
                    {
                        BLL.Util.CloseCurrentPageAfterAlert(Response, "获取不到此工单信息！");
                    }
                }
                else
                {
                    BLL.Util.CloseCurrentPageAfterAlert(Response, "传入参数错误！");
                }
            }
        }
    }
}