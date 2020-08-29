using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data; 

namespace BitAuto.ISDC.CC2012.Web._20160720_工单html
{
    public partial class testWOrderBasicInfoControlPage : System.Web.UI.Page
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
                    WOrderInfoInfo modelout =BLL.WOrderInfo.Instance.GetWOrderInfoInfo(OrderID) ;
                    if (modelout!=null)
                    {
                        custInfoView.CustID = modelout.CBID;

                        DataTable dtCBInfo = BLL.WOrderInfo.Instance.GetCBInfoByPhone(modelout.CBID, "");
                        if (dtCBInfo != null && dtCBInfo.Rows.Count > 0)
                        {
                            //worderBasicInfo.CustCategoryID = dtCBInfo.Rows[0]["CustCategoryID"].ToString();
                            //worderBasicInfo.WorkOrderID = OrderID;
                        }
                    }
                    custInfoView.CanSeeTelImg = true;
                    worderBasicInfo.WOrderInfo = modelout;
                    worderBasicInfo.CanSeeTelImg = true;
                    dealView.OrderId = OrderID;
                }
            }
        }
    }
}