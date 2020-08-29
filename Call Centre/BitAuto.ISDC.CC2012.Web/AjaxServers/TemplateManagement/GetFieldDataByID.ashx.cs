using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// GetFieldDataByID 的摘要说明
    /// </summary>
    public class GetFieldDataByID : IHttpHandler
    {

        public string RecID
        {
            get
            {
                return HttpContext.Current.Request["recid"] == null ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["recid"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string jsonStr = "";

            Entities.TField model = BLL.TField.Instance.GetTField(int.Parse(RecID));

            if (model != null)
            {
                jsonStr = BLL.Common.JsonHelper.ToJsonString(model);
            }

            context.Response.Write(jsonStr);
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