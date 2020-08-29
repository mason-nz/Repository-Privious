using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustomerCallin
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        public string Action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Action").ToString();
            }
        }
        public string RecID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("RecID").ToString();
            }
        }
        public string Status
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("Status").ToString();
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string msg = "";

            switch (Action.ToLower())
            {
                case "savestatus":
                    msg = SaveStatus();
                    break;
            }

            context.Response.Write(msg);
        }

        private string SaveStatus()
        {
            int recid = CommonFunction.ObjectToInteger(RecID);
            int status = CommonFunction.ObjectToInteger(Status);
            CustomerVoiceMsgInfo info = CommonBll.Instance.GetComAdoInfo<CustomerVoiceMsgInfo>(recid);
            if (info != null)
            {
                info.ProcessTime = DateTime.Now;
                info.ProcessUserID = BLL.Util.GetLoginUserID();
                info.Status = status;
                CommonBll.Instance.UpdateComAdoInfo(info);
                //根据主叫号码刷新客户信息
                string CustID = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(info.CallNO);
                return "{success:true,custid:'" + CustID + "',processtime:'" + info.ProcessTime + "',processuser:'" + BLL.Util.GetLoginRealName() + "',tel:'" + info.CallNO + "'}";
            }
            else
            {
                return "{success:false}";
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