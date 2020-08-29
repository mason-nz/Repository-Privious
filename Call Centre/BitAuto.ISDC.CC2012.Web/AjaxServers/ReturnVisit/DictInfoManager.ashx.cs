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
    /// DictInfoManager 的摘要说明
    /// </summary>
    public class DictInfoManager : IHttpHandler, IRequiresSessionState
    {

        bool success = true;
        string result = "";
        string message = "";
        public HttpContext currentContext;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if ((context.Request["show"] + "").Trim() == "yes")
            {
                DataTable dstTable = BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictInfoByDictType(int.Parse(context.Request["DictType"]));
                StringBuilder sb = new StringBuilder();
                if (dstTable.Rows.Count > 0)
                {
                    foreach (DataRow newRow in dstTable.Rows)
                    {
                        sb.Append("{'ID':'" + newRow["DictID"].ToString() + "','Name':'" + newRow["DictName"].ToString() + "'},");
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
            else
            {
                success = false;
                context.Response.Write("[]");
                context.Response.End();
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