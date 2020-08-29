using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class ExportProjectTask1 : PageBase
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
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
        private string tasksubstart
        {
            get
            {

                return HttpContext.Current.Request["tasksubstart"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tasksubstart"].ToString());
            }
        }
        private string tasksubend
        {
            get
            {

                return HttpContext.Current.Request["tasksubend"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tasksubend"].ToString());
            }
        }
        private string taskcreatestart
        {
            get
            {

                return HttpContext.Current.Request["taskcreatestart"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["taskcreatestart"].ToString());
            }
        }
        private string taskcreateend
        {
            get
            {

                return HttpContext.Current.Request["taskcreateend"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["taskcreateend"].ToString());
            }
        }
        string msg = "";//错误信息
        protected void Page_Load(object sender, EventArgs e)
        {
            int projectID;
            if (!int.TryParse(ProjectID, out projectID))
            {
                msg += "项目ID错误，导出失败！";
                return;
            }

            ExportProjectTask ept = new ExportProjectTask();

            DataTable DbdataDt = new DataTable();//自定义数据表中的数据
            string exportName = string.Empty;

            //modify by qizq 2014-11-25 给导出任务加任务创建时间，任务提交时间过滤
            //DbdataDt = ept.ExportTask(projectID, out msg, out exportName);
            DbdataDt = ept.ExportTask(projectID, out msg, out exportName,taskcreatestart,taskcreateend,tasksubstart,tasksubend);

            if (msg != "")
            {
                Response.Write(msg);
                return;
            }

            #region 导出

            BLL.Util.ExportToCSV(exportName + "的任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), DbdataDt);

            #endregion

        }

    }
}