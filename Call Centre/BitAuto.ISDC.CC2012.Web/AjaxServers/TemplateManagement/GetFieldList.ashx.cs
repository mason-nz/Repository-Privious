using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// GetFieldList 的摘要说明
    /// </summary>
    public class GetFieldList : IHttpHandler, IRequiresSessionState
    {
        public string TTCode
        {
            get
            {
                return HttpContext.Current.Request["ttcode"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ttcode"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string jsonStr = "";

            GetFieldListByID(out jsonStr);

            context.Response.Write(jsonStr);
        }

        private void GetFieldListByID(out string jsonStr)
        {
            jsonStr = "";

            Entities.QueryTField query = new Entities.QueryTField();
            query.TTCode = TTCode;

            int totalCount = 0;
            DataTable dt = BLL.TField.Instance.GetTField(query, "TFSortIndex", 1, 9999, out totalCount);

            if (dt != null)
            {
                jsonStr = BLL.Util.DateTableToJson2(dt);
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