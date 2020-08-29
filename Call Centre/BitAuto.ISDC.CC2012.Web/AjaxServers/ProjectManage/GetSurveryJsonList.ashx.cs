using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// GetSurveryJsonList 的摘要说明
    /// </summary>
    public class GetSurveryJsonList : IHttpHandler, IRequiresSessionState
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string jsonStr = "";

            GetSurveryList(out jsonStr);

            context.Response.Write(jsonStr);
        }

        private void GetSurveryList(out string jsonStr)
        {
            jsonStr = "";
            int intVal = 0;
            if (int.TryParse(ProjectID, out intVal))
            {
                Entities.QueryProjectSurveyMapping mapquery = new Entities.QueryProjectSurveyMapping();
                mapquery.ProjectID = int.Parse(ProjectID);

                int totalCount = 0;
                DataTable dt = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(mapquery, " dbo.ProjectSurveyMapping.CreateTime ", 1, 999, out totalCount);
                foreach (DataRow dr in dt.Rows)
                {
                    dr["BeginDate"] = dr["BeginDate"].ToString().Length >= 10 ? dr["BeginDate"].ToString().Substring(0, 10) : dr["BeginDate"].ToString();
                    dr["EndDate"] = dr["EndDate"].ToString().Length >= 10 ? dr["EndDate"].ToString().Substring(0, 10) : dr["EndDate"].ToString();
                }

                if (dt != null)
                {
                    jsonStr = BLL.Util.DateTableToJson2(dt);
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