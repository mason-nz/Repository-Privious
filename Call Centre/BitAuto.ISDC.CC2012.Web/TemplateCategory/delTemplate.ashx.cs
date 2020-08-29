using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    /// <summary>
    /// delTemplate 的摘要说明
    /// </summary>
    public class delTemplate : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            int userID = BLL.Util.GetLoginUserID();
            string msg = string.Empty;
            if (!BLL.Util.CheckRight(userID, "SYS024MOD5101"))
            {
                msg = "您没有执行此操作的权限";
            }
            else
            {
                string TemplateIDs = HttpContext.Current.Request["TemplateIDs"].ToString();
                if (TemplateIDs.Length > 0)
                {
                    TemplateIDs = TemplateIDs.Substring(0, TemplateIDs.Length - 1);
                }
                string[] TemplateIDArr = TemplateIDs.Split(',');
                foreach (string TemplateID in TemplateIDArr)
                {
                    TemplateInfo.Instance.Delete(Convert.ToInt32(TemplateID));
                    BLL.Util.InsertUserLog("【删除】信息模板，模板ID：" + TemplateID + "。操作人ID：" + BLL.Util.GetLoginUserID().ToString());
                }
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