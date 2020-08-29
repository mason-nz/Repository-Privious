using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class CCProcess : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

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
        protected string title = string.Empty;
        protected string dataSource = string.Empty;
        //protected string createTime = string.Empty;
        protected string crmCustName = string.Empty;
        protected string contact = string.Empty;
        protected string tel = string.Empty;
        protected string status = string.Empty;
        protected string categoryID = string.Empty;
        protected string categoryID1 = string.Empty;
        protected string categoryID2 = string.Empty;
        protected string priorityLevel = string.Empty;
        protected string crmCustID = string.Empty;
        protected string isComplaintType = string.Empty;
        protected string tagInfo = string.Empty;
        protected string tagIDs = string.Empty;
        protected string custArea = string.Empty;

        protected string crmCustNameUrl = string.Empty;
        protected string viewCCUserUrl = string.Empty;
        protected string ccUserID = string.Empty;
        protected string ccCustName = string.Empty;

        public string CHUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加“任务列表--工单记录”审核 和 处理功能验证逻辑
                int userId = BLL.Util.GetLoginUserID();
                WorkOrderInfo modelout = null;
                if (!string.IsNullOrEmpty(OrderID))
                {
                    modelout = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                }
                if (BLL.Util.CheckRight(userId, "SYS024BUG100601") || BLL.Util.CheckRight(userId, "SYS024BUG100602") || (modelout != null && modelout.ReceiverID == userId) || Requester == "intelligentplatform")
                {
                    //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                    CheckProcessRight();
                    OperInfoControl2.OrderID = OperInfoControl1.OrderID = OrderID;
                    OperInfoControl2.ViewType = 2;
                    OperInfoControl1.ViewType = 1;
                    // Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);

                    bindData();

                    CustSalesControl1.crmCustID = crmCustID;

                    ccUserID = BLL.WorkOrderInfo.Instance.GetCustIDByWorkOrderTel(modelout);

                    if (!string.IsNullOrEmpty(OrderID) && BLL.WorkOrderInfo.Instance.HasConversation(OrderID))
                    {
                        string imurl = ConfigurationManager.AppSettings["DealerIMURL"].ToString();
                        int thisuserid = BLL.Util.GetLoginUserID();
                        CHUrl = "<a href='javascript:void(0)' onclick=\"GotoConversation(this,'" + imurl + "/ConversationHistoryForCC.aspx','" + thisuserid + "','','" + OrderID + "')\">关联对话</a>";
                    }

                    if (ccUserID != string.Empty)
                    {
                        viewCCUserUrl = "<span><a target='_blank' href='/TaskManager/CustInformation.aspx?CustID=" + ccUserID + "' style='font-size:12px;'>查看用户</a></span>";
                        Entities.CustBasicInfo model = new Entities.CustBasicInfo();
                        model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(ccUserID);
                        ccCustName = model.CustName;
                    }
                }
                else
                {
                    Response.Write("您没有访问该页面的权限");
                    Response.End();
                }
            }
        }

        private void bindData()
        {
            if (!string.IsNullOrEmpty(OrderID))
            {
                Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                if (model != null)
                {
                    hlRelateDemand.Text = model.DemandID == null ? "" : model.DemandID;
                    hlRelateDemand.NavigateUrl = ConfigurationManager.AppSettings["DemandDetailsUrl"] + "?DemandID=" + model.DemandID + "&r=" + new Random().Next();
                    hlRelateDemand.Target = "_blank";
                    if (string.IsNullOrEmpty(hlRelateDemand.Text))
                    {
                        liDemandCC.Visible = false;
                    }
                    else
                    {
                        liDemandCC.Visible = true;
                    }

                    title = model.Title;
                    dataSource = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderDataSource), (int)model.DataSource);

                    DataTable dt_Revert = BLL.WorkOrderRevert.Instance.GetWorkOrderRevertByOrderID(model.OrderID, " w.CreateTime Asc ");

                    if (dt_Revert != null && dt_Revert.Rows.Count > 0)
                    {
                        string pName = string.Empty;
                        string cName = string.Empty;
                        string ctName = string.Empty;
                        if (!string.IsNullOrEmpty(dt_Revert.Rows[0]["ProvinceName"].ToString()))
                        {
                            pName = dt_Revert.Rows[0]["ProvinceName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt_Revert.Rows[0]["CityName"].ToString()))
                        {
                            cName = dt_Revert.Rows[0]["CityName"].ToString();
                        }
                        if (!string.IsNullOrEmpty(dt_Revert.Rows[0]["CountyName"].ToString()))
                        {
                            ctName = dt_Revert.Rows[0]["CountyName"].ToString();
                        }
                        if (pName != string.Empty)
                        {
                            custArea = pName + " " + cName + " " + ctName;
                        }
                    }

                    crmCustName = model.CustName;

                    if (!string.IsNullOrEmpty(model.CRMCustID) && model.CRMCustID != "-2")
                    {
                        crmCustNameUrl = "<a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + model.CRMCustID + "'                                    target='_blank'>" + model.CustName + "</a>";
                    }
                    else
                    {
                        crmCustNameUrl = model.CustName;
                    }

                    contact = model.Contact;
                    tel = model.ContactTel;
                    status = model.WorkOrderStatus.ToString();

                    categoryID = model.CategoryID.ToString();
                    if (!string.IsNullOrEmpty(categoryID))
                    {
                        DataTable dtCategory = BLL.WorkOrderCategory.Instance.GetCategoryFullByCategoryID(categoryID);
                        if (dtCategory.Rows.Count == 2)
                        {
                            categoryID2 = "-1";
                            categoryID1 = dtCategory.Select("Level=1")[0]["CID"].ToString();
                        }
                        else if (dtCategory.Rows.Count == 3)
                        {
                            categoryID2 = dtCategory.Select("Level=2")[0]["CID"].ToString();
                            categoryID1 = dtCategory.Select("Level=1")[0]["CID"].ToString();
                        }

                    }
                    priorityLevel = model.PriorityLevel.ToString();
                    crmCustID = model.CRMCustID;
                    isComplaintType = model.IsComplaintType.ToString();

                    DataTable dt_Tag = BLL.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(model.OrderID);
                    for (int k = 0; k < dt_Tag.Rows.Count; k++)
                    {
                        tagInfo += dt_Tag.Rows[k]["TagName"].ToString() + ",";
                        tagIDs += dt_Tag.Rows[k]["TagID"].ToString() + ",";
                    }

                    tagInfo = tagInfo.TrimEnd(',');
                    tagIDs = tagIDs.TrimEnd(',');
                }
            }
        }

        private void CheckProcessRight()
        {
            BitAuto.YanFa.Crm2009.Entities.UserInfo user = BitAuto.YanFa.Crm2009.BLL.UserInfo.Get(BLL.Util.GetLoginUserID());
            //运营客服
            switch (user.UserClass)
            {
                case 2:
                case 7: break;
                default: Response.Redirect("/WorkOrder/SalesProcess.aspx?OrderID=" + OrderID + "&r=" + new Random().Next()); break;
            }
        }

        /// <summary>
        /// 时间转换为时间戳字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GenerateTimeStamp(DateTime dt)
        {
            // Default implementation of UNIX time of the current UTC time   
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

    }
}