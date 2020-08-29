using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    /// <summary>
    /// MemberInfoManager 的摘要说明
    /// </summary>
    public class MemberInfoManager : IHttpHandler, IRequiresSessionState
    {
        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;

        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        public int RequestID
        {
            get { return currentContext.Request["ID"] == null ? 0 : Convert.ToInt32(currentContext.Request["ID"]); }
        }
        /// <summary>
        /// 直接上级
        /// </summary>
        public int RequestPID
        {
            get { return currentContext.Request["PID"] == null ? 0 : Convert.ToInt32(currentContext.Request["PID"]); }
        }
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
        public string RequestCName
        {
            get { return currentContext.Request["CName"] == null ? string.Empty : currentContext.Request["CName"].Trim(); }
        }
        /// <summary>
        /// 英文名
        /// </summary>
        public string RequestEName
        {
            get { return currentContext.Request["EName"] == null ? string.Empty : currentContext.Request["EName"].Trim(); }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string RequestSex
        {
            get { return currentContext.Request["Sex"] == null ? string.Empty : currentContext.Request["Sex"].Trim(); }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string RequestDepartMent
        {
            get { return currentContext.Request["DepartMent"] == null ? string.Empty : currentContext.Request["DepartMent"].Trim(); }
        }
        /// <summary>
        /// 职级
        /// </summary>
        public string RequestOfficeTypeCode
        {
            get { return currentContext.Request["OfficeTypeCode"] == null ? string.Empty : currentContext.Request["OfficeTypeCode"].Trim(); }
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string RequestTitle
        {
            get { return currentContext.Request["Title"] == null ? string.Empty : currentContext.Request["Title"].Trim(); }
        }
        /// <summary>
        /// 办公室电话
        /// </summary>
        public string RequestOfficeTel
        {
            get { return currentContext.Request["OfficeTel"] == null ? string.Empty : currentContext.Request["OfficeTel"].Trim(); }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string RequestPhone
        {
            get { return currentContext.Request["Phone"] == null ? string.Empty : currentContext.Request["Phone"].Trim(); }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string RequestEmail
        {
            get { return currentContext.Request["Email"] == null ? string.Empty : currentContext.Request["Email"].Trim(); }
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string RequestFax
        {
            get { return currentContext.Request["Fax"] == null ? string.Empty : currentContext.Request["Fax"].Trim(); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string RequestReamrk
        {
            get { return currentContext.Request["Reamrk"] == null ? string.Empty : currentContext.Request["Reamrk"].Trim(); }
        }

        public string RequestAddress
        {
            get { return currentContext.Request["Address"] == null ? string.Empty : currentContext.Request["Address"].Trim(); }
        }
        public string RequestZipCode
        {
            get { return currentContext.Request["ZipCode"] == null ? string.Empty : currentContext.Request["ZipCode"].Trim(); }
        }
        public string RequestMSN
        {
            get { return currentContext.Request["MSN"] == null ? string.Empty : currentContext.Request["MSN"].Trim(); }
        }
        public string RequestBirthday
        {
            get { return currentContext.Request["Birthday"] == null ? string.Empty : currentContext.Request["Birthday"].Trim(); }
        }
        public string ContactUserS
        {
            get { return currentContext.Request["hid_ContactUserS"] == null ? string.Empty : currentContext.Request["hid_ContactUserS"].Trim(); }
        }
        public string Hobby
        {
            get { return currentContext.Request["Hobby"] == null ? string.Empty : currentContext.Request["Hobby"].Trim(); }
        }
        public string Responsible
        {
            get { return currentContext.Request["Responsible"] == null ? string.Empty : currentContext.Request["Responsible"].Trim(); }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if ((context.Request["showMemberInfo"] + "").Trim() == "yes")
            {
                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> listDMSMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(RequestCustID);
                StringBuilder sb = new StringBuilder();
                if (listDMSMember.Count > 0)
                {
                    foreach (BitAuto.YanFa.Crm2009.Entities.DMSMember newDMSMember in listDMSMember)
                    {
                        sb.Append("{'ID':'" + newDMSMember.MemberCode + "','Name':'" + newDMSMember.Name + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["showMemberInfo"] + "").Trim() == "yesbycustid")
            {
                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> listDMSMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByCustID(RequestCustID);
                StringBuilder sb = new StringBuilder();
                if (listDMSMember.Count > 0)
                {
                    foreach (BitAuto.YanFa.Crm2009.Entities.DMSMember newDMSMember in listDMSMember)
                    {
                        sb.Append("{'ID':'" + newDMSMember.MemberCode + "','Name':'" + newDMSMember.Name + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["getCustInfo"] + "").Trim() == "yesbycustid")
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