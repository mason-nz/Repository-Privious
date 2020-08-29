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
    public partial class SalesProcess : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        public string SourceUrl
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SourceUrl");
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
        protected string crmCustID = string.Empty;
        protected string crmCustName = string.Empty;
        protected string contact = string.Empty;


        public string SysUrl = Utils.Config.ConfigurationUtil.GetAppSettingValue("SysUrl");

        public Entities.WorkOrderInfo modelInfo = new Entities.WorkOrderInfo();
        public string dataSource = string.Empty;

        public string CrmCustInfoUrl = string.Empty;
        protected string custArea = string.Empty;
        protected string telVisible = "style=\"display:none\"";
        protected int userid;
        protected int BGID;
        protected int SCID;
        protected string ccUserID = string.Empty;
        protected string ccCustName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckProcessRight();
                //判断是否登录
                if (!BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin())
                {
                    string CrmLoginURL = System.Configuration.ConfigurationManager.AppSettings["SysUrl"];
                    string CCLoginURL = System.Configuration.ConfigurationManager.AppSettings["ExitAddress"];

                    string redirectURL = "http://" + CrmLoginURL + "?gourl=" + CCLoginURL + "/WorkOrder/SalesProcess.aspx?OrderID=" + OrderID;
                    //验证失败
                    Response.Redirect(redirectURL);
                }

                userid = BLL.Util.GetLoginUserID();
                Entities.WorkOrderInfo workorderinfo = null;
                if (!string.IsNullOrEmpty(OrderID))
                {
                    workorderinfo = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                    if ((workorderinfo != null && workorderinfo.ReceiverID != userid) && Requester != "intelligentplatform")
                    {
                        Response.Write("您不是该工单的接收人，没有访问该页面的权限");
                        Response.End();
                    }
                }
                BGID = BLL.SurveyCategory.Instance.GetSelfBGIDByUserID(userid);//登陆者所在业务组ID
                SCID = BLL.SurveyCategory.Instance.GetSelfSCIDByUserID(userid);//登陆者所在业务组下的工单分类ID
                ccUserID = BLL.WorkOrderInfo.Instance.GetCustIDByWorkOrderTel(workorderinfo);
                if (ccUserID != string.Empty)
                {
                    Entities.CustBasicInfo model = new Entities.CustBasicInfo();
                    model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(ccUserID);
                    ccCustName = model.CustName;
                }
                OperInfoControl1.OrderID = OrderID;
                OperInfoControl1.ViewType = 1;
                bindData();

                string CrmUrl = System.Configuration.ConfigurationManager.AppSettings["CrmUrl"].ToString();
                if (!string.IsNullOrEmpty(modelInfo.CRMCustID) && modelInfo.CRMCustID != "-2")
                {
                    CrmCustInfoUrl = "<a href='http://" + CrmUrl + "/CustomInfo/Main.aspx?CustID=" + modelInfo.CRMCustID + "' target='_blank'>" + modelInfo.CustName + "</a>";
                }
                else
                {
                    CrmCustInfoUrl = modelInfo.CustName;
                }
                CheckUserRightByRoleName();
            }
        }


        private void bindData()
        {
            if (!string.IsNullOrEmpty(OrderID))
            {
                Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(OrderID);
                if (model != null)
                {

                    modelInfo = model;
                    hlRelateDemand.Text = model.DemandID == null ? "" : model.DemandID;
                    hlRelateDemand.NavigateUrl = ConfigurationManager.AppSettings["DemandDetailsUrl"] + "?DemandID=" + model.DemandID + "&r=" + new Random().Next();
                    hlRelateDemand.Target = "_blank";

                    crmCustID = model.CRMCustID;
                    crmCustName = model.CustName;
                    contact = model.Contact;

                    if (string.IsNullOrEmpty(hlRelateDemand.Text))
                    {
                        ulReturn.Visible = false;
                    }
                    else
                    {
                        ulReturn.Visible = true;
                    }

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
                }
            }
        }


        private void CheckUserRightByRoleName()
        {
            ////根据用户在CC系统中的角色名称判断权限
            //int RecordCount = 0;
            //QueryEmployeeSuper query = new QueryEmployeeSuper();
            //query.UserID = BLL.Util.GetLoginUserID();   // int.Parse(Session["userid"].ToString());
            //DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    string strRoleNames = dt.Rows[0]["RoleName"].ToString();
            //    string[] roleNames = strRoleNames.Split(',');
            //    foreach (string rn in roleNames)
            //    {
            //        if (rn.Trim() == "运营客服")
            //        {
            //            telVisible = "";
            //            break;
            //        }
            //    }
            //}

            //根据用户在集中权限系统中的客户分类判断权限
            BitAuto.YanFa.Crm2009.Entities.UserInfo user = BitAuto.YanFa.Crm2009.BLL.UserInfo.Get(BLL.Util.GetLoginUserID());
            if (user.UserClass == 7)
            {
                telVisible = "";
            }
        }

        private void CheckProcessRight()
        {
            BitAuto.YanFa.Crm2009.Entities.UserInfo user = BitAuto.YanFa.Crm2009.BLL.UserInfo.Get(BLL.Util.GetLoginUserID());
            //运营客服
            switch (user.UserClass)
            {
                case 2:
                case 7: if (!string.IsNullOrEmpty(GetRedirectUrl(user.UserID, OrderID)))
                    {
                        Response.Redirect(GetRedirectUrl(user.UserID, OrderID));
                    }; break;
                default: break;
            }
        }
        private string GetRedirectUrl(int userid, string strOrderId)
        {
            string strUrl = string.Empty;

            DataTable dt = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserInfoByUserID(userid);
            string[] departIds = ConfigurationManager.AppSettings["YichePartID"].Split(',');
            DataTable dtAllDeparts = BLL.WorkOrderInfo.Instance.GetChildDepartMent(departIds);
            foreach (DataRow departRow in dtAllDeparts.Rows)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (departRow[0] != null && row["DepartID"] != null && departRow[0].ToString() == row["DepartID"].ToString())
                    {
                        strUrl = "/WorkOrder/CCProcess.aspx?OrderID=" + strOrderId + "&r=" + new Random().Next();
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(strUrl))
                {
                    break;
                }
            }
            return strUrl;
        }
    }
}

