using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// GetTpageByCID 的摘要说明
    /// </summary>
    public class GetTpageByCID : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CID
        {
            get
            {
                if (Request["CID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "gettpage"://
                    GetTPage(out msg);
                    break;

            }
            context.Response.Write(msg);
        }

        private void GetTPage(out string msg)
        {
            msg = string.Empty;
            int cid = -1;
            if (int.TryParse(CID, out cid))
            {
                Entities.QueryTPage query = new Entities.QueryTPage();
                query.SCID = cid;
                query.Statuss = "1,2";
                query.IsUsed = 1;   //“是否可用”为启用

                int totalCount = 0;
                DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 1000, out totalCount);
                string path = "/upload/" + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "/");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (i == 0)
                    {
                        msg += "[{RecID:'" + dr["RecID"] + "',TPName:'" + dr["TPName"] + "','GenTempletPath':'" + path + dr["GenTempletPath"] + "','TTCode':'" + dr["TTCode"] + "'}";
                    }
                    if (i > 0)
                    {
                        msg += ",{RecID:'" + dr["RecID"] + "',TPName:'" + dr["TPName"] + "','GenTempletPath':'" + path + dr["GenTempletPath"] + "','TTCode':'" + dr["TTCode"] + "'}";
                    }
                    if (i == dt.Rows.Count - 1)
                    {
                        msg += "]";
                    }
                }
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