using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.WebService.CC;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    public partial class SendSMSPoperNew2 : System.Web.UI.Page
    {
        /// <summary>
        /// 1-CRM客户联系人调用，2-其他任务调用
        /// </summary>
        public string PageType
        {
            get
            {
                return HttpContext.Current.Request["PageType"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["PageType"].ToString());
            }
        }
        //CRM客户ID
        public string CRMCustID
        {
            get
            {
                return HttpContext.Current.Request["CRMCustID"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CRMCustID"].ToString());
            }
        }
        //个人客户ID
        public string CustID
        {
            get
            {
                return HttpContext.Current.Request["CustID"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString());
            }
        }
        //电话
        public string Tel
        {
            get
            {
                return HttpContext.Current.Request["Tel"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Tel"].ToString());
            }
        }
        //任务类型4-其它任务
        public string TaskType
        {
            get
            {
                if (Request["TaskType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //任务ID
        public string TaskID
        {
            get
            {
                if (Request["TaskID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TaskID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //短信模板ID
        public string TemplateID
        {
            get
            {
                if (Request["TemplateID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TemplateID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string DMSCode
        {
            get
            {
                return HttpContext.Current.Request["DMSCode"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["DMSCode"].ToString());
            }
        }
        public string CustName
        {
            get
            {
                return HttpContext.Current.Request["CustName"] == null ?
                    string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString());
            }
        }
        public string DMSTEL = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                if (!string.IsNullOrEmpty(DMSCode))
                {
                    int memberCode = 0;
                    if (int.TryParse(DMSCode, out memberCode))
                    {
                        var obj = CCWebServiceHepler.Instance.InvokeWebService("", memberCode);
                        DataSet ds = (DataSet)obj;
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DMSTEL = ds.Tables[0].Rows[0][2].ToString();
                        }
                    }
                }
            }
        }
    }
}