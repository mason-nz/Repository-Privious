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
    public partial class WorkOrderViewDealer : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //private HttpRequest Request
        //{
        //    get { return HttpContext.Current.Request; }
        //}

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

        public string title = string.Empty;
        public string dataSource = string.Empty;
        //public string createTime = string.Empty;
        public string crmCustName = string.Empty;
        public string contact = string.Empty;
        public string tel = string.Empty;
        public string crmCustID = string.Empty;
        public string CustID = string.Empty;
        public string CRMCustURL = string.Empty;
        protected string custArea = string.Empty;


        public string CHUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OperInfoControl2.OrderID = OperInfoControl1.OrderID = OrderID;
                OperInfoControl2.ViewType = 2;
                OperInfoControl1.ViewType = 1;

                bindData();
            }

        }

        private void bindData()
        {
            if (!string.IsNullOrEmpty(OrderID))
            {
                Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(OrderID) && BLL.WorkOrderInfo.Instance.HasConversation(OrderID))
                    {
                        string imurl = ConfigurationManager.AppSettings["DealerIMURL"].ToString();
                        int thisuserid = BLL.Util.GetLoginUserID();
                        CHUrl = "<a href='javascript:void(0)' onclick=\"GotoConversation(this,'" + imurl + "/ConversationHistoryForCC.aspx','" + thisuserid + "','','" + OrderID + "')\">关联对话</a>";
                    }

                    title = model.Title;
                    dataSource = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderDataSource), (int)model.DataSource);
                    //createTime = DateTime.Parse(model.CreateTime.ToString()).ToString("yyyy年MM月dd日HH:mm");

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
                    contact = model.Contact;
                    tel = model.ContactTel;
                    crmCustID = model.CRMCustID;

                    if (!string.IsNullOrEmpty(crmCustID) && !string.IsNullOrEmpty(crmCustName))
                    {
                        string url = "/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + crmCustID;
                        CRMCustURL = "<a href='" + url + "' target='_blank'>" + crmCustName + "</a>";
                    }
                    else
                    {
                        CRMCustURL = crmCustName;
                    }


                    this.lblStatus.Text = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderStatus), (int)model.WorkOrderStatus);
                    this.lblCategory.Text = BitAuto.ISDC.CC2012.BLL.WorkOrderCategory.Instance.GetCategoryFullName(model.CategoryID.ToString());
                    this.lblPriorityLevel.Text = BLL.Util.GetEnumOptText(typeof(Entities.PriorityLevelEnum), (int)model.PriorityLevel);

                    this.hlRelateDemand.Text = model.DemandID;
                    this.hlRelateDemand.NavigateUrl = ConfigurationManager.AppSettings["DemandDetailsUrl"] + "?DemandID=" + model.DemandID + "&r=" + new Random().Next();
                    this.hlRelateDemand.Target = "_blank";
                    if (string.IsNullOrEmpty(hlRelateDemand.Text))
                    {
                        liDemandDealer.Visible = false;
                    }
                    else
                    {
                        liDemandDealer.Visible = true;
                    }
                    if (model.IsComplaintType == true)
                    {
                        this.lblComplaintType.Text = "投诉";
                    }
                    else
                    {
                        this.lblComplaintType.Text = "普通";
                    }
                    this.lblReceiverPerson.Text = model.ReceiverName;
                    CustID = BLL.WorkOrderInfo.Instance.GetCustIDByWorkOrderTel(model);
                    if (!string.IsNullOrEmpty(CustID))
                    {
                        CustID = "<a target='_blank' href='/TaskManager/CustInformation.aspx?CustID=" + CustID + "'>查看用户</a>";
                    }

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
            else
            {
                Response.Write(@"<script language='javascript'>javascript:alert('工单ID不能为空！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
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