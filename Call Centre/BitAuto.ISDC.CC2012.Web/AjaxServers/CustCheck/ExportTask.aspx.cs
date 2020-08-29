using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck
{
    public partial class ExportTask : PageBase
    {
        private string TaskIDS
        {
            get
            {
                return HttpContext.Current.Request["SelectPTIDs"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SelectPTIDs"].ToString());
            }
        }

        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }

        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        private string RequestCustName
        {
            get { return HttpContext.Current.Request["custName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["custName"].ToString()); }
        }

        private string RequestStatuss
        {
            get { return HttpContext.Current.Request["status_s"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status_s"].ToString()); }
        }
        private string RequestOperationStatus
        {
            get { return HttpContext.Current.Request["operstatus_s"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["operstatus_s"].ToString()); }
        }
        private string RequestGroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        private string RequestCategory
        {
            get { return HttpContext.Current.Request["category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["category"].ToString()); }
        }
        private string RequestCreater
        {
            get { return HttpContext.Current.Request["creater"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["creater"].ToString()); }
        }
        private string RequestUserId
        {
            get { return HttpContext.Current.Request["selUserId"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["selUserId"].ToString()); }
        }

        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }
        private string RequestOptUserId
        {
            get { return HttpContext.Current.Request["optUserId"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["optUserId"].ToString()); }
        }

        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pagesize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pagesize"].ToString());
            }
        }

        public string CRMBrandIDs
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["CRMBrandIDs"]) ? "" :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["CRMBrandIDs"].ToString());
            }
        }

        public string NoCRMBrand
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["NoCRMBrand"]) ? "" :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["NoCRMBrand"].ToString());
            }
        }

        private string TaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }

        private string CustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }
        private string CustType
        {
            get { return HttpContext.Current.Request["CustType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustType"].ToString()); }
        }
        private string LastOperStartTime
        {
            get { return HttpContext.Current.Request["LastOperStartTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperStartTime"].ToString()); }
        }
        private string LastOperEndTime
        {
            get { return HttpContext.Current.Request["LastOperEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOperEndTime"].ToString()); }
        }
        private string additionalStatus;
        public string AdditionalStatus
        {
            get
            {
                if (additionalStatus == null)
                {
                    additionalStatus = HttpUtility.UrlDecode((Request["AdditionalStatus"] + "").Trim());
                }
                return additionalStatus;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TaskIDS != "")
            {
                //选中了任务，就按选中的导出

                ExportByIDs();
            }
            else
            {
                //没有选中的，就按条件来导出
                Entities.QueryProjectTaskInfo query = BLL.ProjectTaskInfo.Instance.GetQuery(RequestName, RequestCustName, RequestStatuss, additionalStatus,
             RequestGroup, RequestCategory, RequestCreater, RequestUserId, RequestBeginTime, RequestEndTime, RequestOptUserId,
             CRMBrandIDs, NoCRMBrand, TaskID, CustID, RequestOperationStatus, CustType, LastOperStartTime, LastOperEndTime
              );

                int totalCount = 0;
                int userID = BLL.Util.GetLoginUserID();
                DataTable dt = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(query, 1, 999999, out totalCount, userID);

                string taskIDStr = "";

                foreach (DataRow dr in dt.Rows)
                {
                    taskIDStr = taskIDStr + "'" + dr["PTID"].ToString() + "',";
                }
                if (taskIDStr != "")
                {
                    taskIDStr = taskIDStr.Substring(0, taskIDStr.Length - 1);
                }
                else
                {
                    return;
                }
                DataTable taskDt = BLL.ProjectTaskInfo.Instance.GetExportTaskList(taskIDStr);
                if (taskDt != null)
                {
                    ExportData(taskDt);
                }

            }
        }

        private void ExportByIDs()
        {
            #region ID列表串

            string taskIDStr = "";

            string[] tidList = TaskIDS.Split(',');

            foreach (string item in tidList)
            {
                taskIDStr += "'" + item + "',";
            }
            if (taskIDStr != "")
            {
                taskIDStr = taskIDStr.Substring(0, taskIDStr.Length - 1);
            }
            else
            {
                return;
            }
            #endregion

            DataTable taskDt = BLL.ProjectTaskInfo.Instance.GetExportTaskList(taskIDStr);
            if (taskDt != null)
            {
                ExportData(taskDt);
            }
        }

        private void ExportData(DataTable dt)
        {
            BLL.Util.ExportToCSV("任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"),dt);
        }      
    }
}