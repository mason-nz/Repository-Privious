using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.WebService.EasyPass;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{

    /// <summary>
    /// EPEmbedCC 的摘要说明
    /// </summary>
    public class EPEmbedCC : IHttpHandler, IRequiresSessionState
    {
        protected static string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
        public HttpContext currentContext;
        private string YPFanXianURL
        {
            get { return currentContext.Request["YPFanXianURL"] == null ? string.Empty : currentContext.Request["YPFanXianURL"].Trim(); }
        }
        private string RequestGoToEPURL
        {
            get { return currentContext.Request["GoToEPURL"] == null ? string.Empty : currentContext.Request["GoToEPURL"].Trim(); }
        }
        private string EPEmbedCC_APPID
        {
            get { return currentContext.Request["EPEmbedCC_APPID"] == null ? string.Empty : currentContext.Request["EPEmbedCC_APPID"].Trim(); }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            int userid = BLL.Util.GetLoginUserID();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            string msg = string.Empty;
            string RoleID = string.Empty;
            DataTable dtRole = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid, sysID);
            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                RoleID = dtRole.Rows[0]["RoleID"].ToString();
            }
            string ReturnURL = string.Empty;
            int ret = FanXianHelper.Instance.EPEmbedCC_AuthService(userid, RoleID, RequestGoToEPURL, out msg, EPEmbedCC_APPID);
            if (ret == 1)
            {
                msg = YPFanXianURL + "?" + msg;
            }
            else
            {
                msg = "Error";
            }
            context.Response.Write(msg);

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