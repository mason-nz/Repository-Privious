using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class ExportProjectForYTG : PageBase
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024BUT500605"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                Entities.ProjectInfo info = BLL.ProjectInfo.Instance.GetProjectInfo(CommonFunction.ObjectToInteger(ProjectID));
                if (info == null)
                {
                    return;
                }

                QueryYTGActivityTaskInfo query = new QueryYTGActivityTaskInfo();
                query.ProjectID = CommonFunction.ObjectToInteger(ProjectID);
                query.LoginID = BLL.Util.GetLoginUserID();

                query.TaskCBeginTime = taskcreatestart;
                query.TaskCEndTime = taskcreateend;

                query.Subbegintime = tasksubstart;
                query.Subendtime = tasksubend;

                DataTable dt = BLL.YTGActivityTask.Instance.GetYTGProjectTasksForExport(query);
                if (dt != null)
                {
                    BLL.Util.ExportToCSV(info.Name + "的任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
                }
            }
        }
    }
}