using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Web.Channels;
using System.Data;


namespace BitAuto.DSC.IM_2015.Web.CustInfo
{
    public partial class ServiceInfo : System.Web.UI.Page
    {

        public string OrderID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["orderid"]))
                {
                    return HttpContext.Current.Request["orderid"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string RequestVisitID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["VisitID"]))
                {
                    return HttpContext.Current.Request["VisitID"];
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CustID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["custid"]))
                {
                    return HttpContext.Current.Request["custid"];
                }
                else
                {
                    return string.Empty;
                    //return "CB00124751";
                }
            }

        }
        protected void GetData(string custid, string userid)
        {
            DataTable dt = null;

            dt = BitAuto.DSC.IM_2015.WebService.CC.CCWebServiceHepler.Instance.CCDataInterface_GetCustHistoryInfo(custid, userid);
            repterList.DataSource = dt;
            repterList.DataBind();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userid = BLL.Util.GetLoginUserID().ToString();
                //调用接口,有custidstr调用接口
                if (!string.IsNullOrEmpty(CustID))
                {
                    GetData(CustID, userid);
                }
                else
                {
                    Entities.UserVisitLog model = null;
                    if (!string.IsNullOrEmpty(RequestVisitID))
                    {
                        int _visitid = 0;
                        int.TryParse(RequestVisitID, out _visitid);
                        model = BLL.UserVisitLog.Instance.GetUserVisitLog(_visitid);
                        if (model != null && string.IsNullOrEmpty(model.CustID) && !string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Phone))
                        {
                            //如果电话，姓名不为空//调用cc接口，取custinfo
                            //如果custinfo 存在，用custinfo 信息填充表单，否则用现有信息填充
                            string json = string.Empty;
                            Entities.CEntity CEntityModel = BitAuto.DSC.IM_2015.WebService.CC.CCWebServiceHepler.Instance.GetCustomer(model.Phone.Trim(), model.UserName.Trim());
                            if (CEntityModel != null)
                            {
                                model.CustID = CEntityModel.CustID;
                                BLL.UserVisitLog.Instance.UpdateUserVisitLog(model);
                            }
                        }
                    }
                    if (model != null && !string.IsNullOrEmpty(model.CustID))
                    {
                        GetData(model.CustID, userid);
                    }
                }

            }
        }
        public string GetStrByOrderID(string taskid)
        {
            string Returnstr = string.Empty;
            Returnstr = "style='position: relative; top: -2px;right: 2px;display:none'";
            if (!string.IsNullOrEmpty(OrderID))
            {
                if (OrderID == taskid)
                {

                    Returnstr = "style='position: relative; top: -2px;right: 2px;'";
                }
                else
                {
                    Returnstr = "style='position: relative; top: -2px;right: 2px;display:none'";
                }
            }
            return Returnstr;
        }
    }
}