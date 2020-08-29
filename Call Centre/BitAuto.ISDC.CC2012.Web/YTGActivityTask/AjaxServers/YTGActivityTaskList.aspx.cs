using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers
{
    public partial class YTGActivityTaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        private string RequestAssignID
        {
            get { return HttpContext.Current.Request["AssignID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AssignID"].ToString()); }
        }
        private string RequestProjectName
        {
            get { return HttpContext.Current.Request["ProjectName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectName"].ToString()); }
        }
        private string RequestStatus
        {
            get { return HttpContext.Current.Request["Status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString()); }
        }
        private string RequestIsSuccess
        {
            get { return HttpContext.Current.Request["IsSuccess"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsSuccess"].ToString()); }
        }
        //private string RequestBeginDealTime
        //{
        //    get { return HttpContext.Current.Request["BeginDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginDealTime"].ToString()); }
        //}
        //private string RequestEndDealTime
        //{
        //    get { return HttpContext.Current.Request["EndDealTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndDealTime"].ToString()); }
        //}

        private string RequestTaskCBeginTime
        {
            get { return HttpContext.Current.Request["TaskCBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskCBeginTime"].ToString()); }
        }
        private string RequestTaskCEndTime
        {
            get { return HttpContext.Current.Request["TaskCEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskCEndTime"].ToString()); }
        }
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        //为选择分页记录数
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }
        private bool isAssign = false;
        private bool isRecede = false;
        private bool isProcess = false;
        private int userID = 0;

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                isAssign = BLL.Util.CheckRight(userID, "SYS024MOD101503");
                isRecede = BLL.Util.CheckRight(userID, "SYS024MOD101504");
                isProcess = BLL.Util.CheckRight(userID, "SYS024MOD101505");

                bindData();
            }
        }

        protected string GetView(string source, string taskid)
        {

            string strview = string.Empty;
            return "<a href='YTGActivityTaskView.aspx?TaskID=" + taskid + "' target='_blank'>" + taskid + "</a>&nbsp;";
            //if (source == "5")
            //{
            //    return "<a href='LeadTaskView.aspx?TaskID=" + taskid + "' target='_blank'>" + taskid + "</a>&nbsp;";
            //}
            //else
            //{
            //    return "<a href='CSLeadTaskView.aspx?TaskID=" + taskid + "' target='_blank'>" + taskid + "</a>&nbsp;";
            //}
        }

        private void bindData()
        {
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = PageSize;
            }
            var query = new Entities.QueryYTGActivityTaskInfo();
            int assid = 0;
            if (int.TryParse(RequestAssignID, out assid))
            {
                query.AssignUserID = assid;
            }
            if (RequestProjectName != "")
            {
                query.ProjectName = RequestProjectName;
            }
            int status = 0;
            if (int.TryParse(RequestStatus, out status))
            {
                query.Status = status;
            }
            int success = 0;
            if (int.TryParse(RequestIsSuccess, out success))
            {
                query.IsSuccess = success;
            }
            //if (RequestBeginDealTime != "")
            //{
            //    query.BeginDealTime = RequestBeginDealTime;
            //}
            //if (RequestEndDealTime != "")
            //{
            //    query.EndDealTime = RequestEndDealTime;
            //}
            query.LoginID = userID;

            if (!string.IsNullOrEmpty(RequestTaskCBeginTime))
            {
                query.TaskCBeginTime = RequestTaskCBeginTime;
            }
            if (!string.IsNullOrEmpty(RequestTaskCEndTime))
            {
                query.TaskCEndTime = RequestTaskCEndTime;
            }
            if (!string.IsNullOrEmpty(RequestTaskID))
            {
                query.TaskID = RequestTaskID;
            }


            int RecordCount = 0;

            DataTable dt = BLL.YTGActivityTask.Instance.GetYTGTask(query, "lt.CreateTime ASC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //得到最晚处理时间
        protected string lastTime(string time)
        {
            DateTime _time;
            if (DateTime.TryParse(time, out _time))
            {
                DateTime today = DateTime.Now;
                string html = _time.ToShortDateString();
                if (_time <= today)
                {
                    html = "<span style='color:red'>" + html + "</span>";
                }
                return html;
            }
            else
            {
                return "";
            }
        }

        //得到状态名称
        protected string getStatus(string status)
        {
            int _status = 0;
            if (!int.TryParse(status, out _status))
            {
                return string.Empty;
            }
            return BLL.Util.GetEnumOptText(typeof(Entities.YTGActivityTaskStatus), _status);
        }

        //得到操作
        protected string getOperLink(string status, string taskid, string assignid, string source)
        {
            int _status = 0;
            string link = string.Empty;
            if (!int.TryParse(status, out _status)) return link;

            switch (_status)
            {
                //待分配
                case (int)Entities.YTGActivityTaskStatus.NoAllocation:
                    link = getAssignLink(taskid);
                    break;
                //待处理
                case (int)Entities.YTGActivityTaskStatus.NoProcess:
                    link = getAssignLink(taskid) + getRecedeLink(taskid) + getProcessLink(taskid, assignid, source);
                    break;
                //处理中
                case (int)Entities.YTGActivityTaskStatus.Processing:
                    link = getRecedeLink(taskid) + getProcessLink(taskid, assignid, source);
                    break;
                //已处理
                case (int)Entities.YTGActivityTaskStatus.Processed:
                    break;
                //已撤销
                case (int)Entities.YTGActivityTaskStatus.ReBack:
                    break;
            }
            return link;
        }

        //分配链接
        private string getAssignLink(string taskid)
        {
            return isAssign ? "<a href='javascript:;' onclick='operAssign(\"" + taskid + "\")'>分配</a>&nbsp;" : string.Empty;
        }

        //收回链接
        private string getRecedeLink(string taskid)
        {
            return isRecede ? "<a href='javascript:;' onclick='operRecede(\"" + taskid + "\")'>收回</a>&nbsp;" : string.Empty;
        }

        //处理链接
        private string getProcessLink(string taskid, string userid, string source)
        {
            if (isProcess && userid == userID.ToString() && source == "7")
            {
                return "<a href='YTGActivityTaskDeal.aspx?TaskID=" + taskid + "&number=1000' target='_leadsTaskList'>处理</a>&nbsp;";
            }
            else
            {
                return string.Empty;
            };
        }

    }
}