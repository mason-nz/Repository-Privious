using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    /// <summary>
    /// Summary description for CustInfoManager
    /// </summary>
    public class CustInfoManager : IHttpHandler, IRequiresSessionState
    {
        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;

        #region 属性        
        /// <summary>
        /// 客户ID
        /// </summary>
        public string RequestCustID
        {
            get { return currentContext.Request["CustID"] == null ? string.Empty : currentContext.Request["CustID"].Trim(); }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RequestCustName
        {
            get { return currentContext.Request["CustName"] == null ? string.Empty : currentContext.Request["CustName"].Trim(); }
        }        
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            currentContext = context;
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if ((context.Request["getCustInfo"] + "").Trim() == "yesbycustid")
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(RequestCustID);
                if (custInfo != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{'TypeID':'" + custInfo.TypeID + "','Name':'" + custInfo.CustName + "'}");
                    message = sb.ToString();
                    //如果客户类别是 个人、经纪公司、交易市场，则选中二手车,其他选择新车
                    //switch (custInfo.TypeID)
                    //{
                    //    case "20010":
                    //    case "20011":
                    //    case "20012": this.chkTypeSnd.Checked = true; break;
                    //    default: this.chkTypeNew.Checked = true; break;
                    //}


                    //BindText();
                    //litCustName.InnerText = custInfo.CustName;
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["getCustInfo"] + "").Trim() == "binddata")
            {

                DataTable dt_cc_returnVisit = BLL.ProjectTask_ReturnVisit.Instance.GetTable(RequestCustID);
                if (dt_cc_returnVisit.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{'VisitType':'" + dt_cc_returnVisit.Rows[0]["VisitType"].ToString() + "','RVType':'" + dt_cc_returnVisit.Rows[0]["RVType"].ToString() +
                               "','Remark':'" + dt_cc_returnVisit.Rows[0]["Remark"].ToString() + "','TypeID':'" + dt_cc_returnVisit.Rows[0]["TypeID"].ToString() + "'}");

                    message = sb.ToString();
                }

                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else
            {
                success = false;
                message = "request error";
                BitAuto.ISDC.CC2012.BLL.AJAXHelper.WrapJsonResponse(success, result, message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}