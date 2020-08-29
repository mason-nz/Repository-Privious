using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage.UCTraffic
{
    /// <summary>
    /// TrafficHandler 的摘要说明
    /// </summary>
    public class TrafficHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpContext.Current.Request["Action"].ToString(); }
        }
        private string RequestProjectId
        {
            get { return HttpContext.Current.Request["ProjectId"] == null ? "" : HttpContext.Current.Request["ProjectId"].ToString(); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            if (RequestAction == "TaskCategory")
            {
                getTaskCategory(out msg);
            }
            else if (RequestAction == "GetProjectNotSuccessReason")
            {
                GetProjectNotSuccessReason(out msg);
            }
            context.Response.Write("{" + msg + "}");
        }

        private void GetProjectNotSuccessReason(out string msg)
        {
            msg = string.Empty; 
            long projectid;
            if (long.TryParse(RequestProjectId, out projectid))
            {
                string strReason = BLL.CallRecordInfo.Instance.GetNotSuccessReason(projectid);
                string reasonids = ""; 
                string[] strItems = strReason.Split(';');
                foreach (string item in strItems)
                {
                   reasonids += item.Split('|').Length == 2 ? ("," + item.Split('|')[0]) : "";
                }
                if (reasonids != "")
                {
                    reasonids = reasonids.Substring(1);
                    DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.NotSuccessReason));
                    DataRow[] rows = dt.Select("value in (" + reasonids + ")");
                    if (rows.Length > 0)
                    {
                        msg += "'error':'',reasons:[";
                        for (int i = 0; i < rows.Length; i++)
                        {
                            msg += "{name:'" + rows[i]["name"].ToString() + "',value:'" + rows[i]["value"].ToString() + "'},";
                            if (i == rows.Length - 1)
                            {
                                msg = msg.TrimEnd(',') + "]";
                            }
                        }
                    }
                }
                else
                {
                    msg += "'error':'',reasons:''";
                }
                
            }
            else
            {
                msg = "'error':'参数异常'";
            }
        }

        //任务分类
        private void getTaskCategory(out string msg)
        {
            msg = string.Empty;
            DataTable dt = null;
            dt = BLL.Util.GetEnumDataTable(typeof(Entities.TaskTypeID));
            if (dt != null && dt.Rows.Count > 0)
            {
                msg += "category:[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["name"].ToString() + "',value:'" + dt.Rows[i]["value"].ToString() + "'},";
                    if (i == dt.Rows.Count - 1)
                    {
                        msg = msg.TrimEnd(',') + "]";
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