using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Diagnostics;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.OtherTask
{
    public partial class OtherTaskList : PageBase
    {
        #region 属性
        private string RequestProjectName
        {
            get { return HttpContext.Current.Request["projectName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["projectName"].ToString()); }
        }
        private string RequestStatuss
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }

        private string RequestAgent
        {
            get { return HttpContext.Current.Request["Agent"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Agent"].ToString()); }
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
        private string RequestOper
        {
            get { return HttpContext.Current.Request["oper"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["oper"].ToString()); }
        }
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }
        private string RequestLastOptBeginTime
        {
            get { return HttpContext.Current.Request["LastOptBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOptBeginTime"].ToString()); }
        }
        private string RequestLastOptEndTime
        {
            get { return HttpContext.Current.Request["LastOptEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LastOptEndTime"].ToString()); }
        }
        private string RequestPTID
        {
            get { return HttpContext.Current.Request["PTID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PTID"].ToString()); }
        }
        private string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }
        private string RequestCustName
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }

        #endregion

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        bool right_process;         //处理权限 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                userID = BLL.Util.GetLoginUserID();
                right_process = BLL.Util.CheckRight(userID, "SYS024BUT150103");

                BindData();
                stopwatch.Stop();
                BLL.Loger.Log4Net.Info(string.Format("【其他任务查询Step3——总耗时】：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
            }
        }
        //绑定数据
        public void BindData()
        {
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }

            Entities.QueryOtherTaskInfo query = new Entities.QueryOtherTaskInfo();

            if (RequestCustID != "")
            {
                query.CustID = RequestCustID;
            }
            if (RequestProjectName != "")
            {
                query.ProjectName = RequestProjectName;
            }
            if (RequestStatuss != "")
            {
                query.Statuss = RequestStatuss;
            }

            if (RequestGroup != "")
            {
                int gid = 0;
                if (int.TryParse(RequestGroup, out gid))
                {
                    query.BGID = gid;
                }
            }
            if (RequestCategory != "")
            {
                int cid = 0;
                if (int.TryParse(RequestCategory, out cid))
                {
                    query.SCID = cid;
                }
            }
            if (RequestCreater != "")
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (RequestOper != "")
            {
                query.Oper = int.Parse(RequestOper);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = RequestBeginTime;
            }
            if (RequestEndTime != "")
            {
                query.EndTime = RequestEndTime;
            }
            if (RequestLastOptBeginTime != "")
            {
                query.LastOptBeginTime = RequestLastOptBeginTime;
            }
            if (RequestLastOptEndTime != "")
            {
                query.LastOptEndTime = RequestLastOptEndTime;
            }
            if (RequestPTID != "")
            {
                query.PTID = RequestPTID;
            }
            if (RequestAgent != "")
            {
                int agent = 0;
                if (int.TryParse(RequestAgent, out agent))
                {
                    query.Agent = agent;
                }
            }
            if (RequestCustName != "")
            {
                query.CustName = RequestCustName;
            }

            query.LoginID = userID;

            int RecordCount = 0;

            BLL.Loger.Log4Net.Info("【其他任务查询Step1——数据库Where条件】：" + BLL.OtherTaskInfo.Instance.GetWhereStringForLog(query));
            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            DataTable dt = BLL.OtherTaskInfo.Instance.GetOtherTaskInfoByList(query, "OtherTaskInfo.LastOptTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            stopwatch2.Stop();
            BLL.Loger.Log4Net.Info(string.Format("【其他任务查询Step2——数据库查询耗时】：{0}毫秒", stopwatch2.Elapsed.TotalMilliseconds));

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //状态
        public string getStatusName(string status)
        {
            string name = string.Empty;
            int _status;
            if (!int.TryParse(status, out _status))
            {
                return "";
            }
            name = BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.OtheTaskStatus), _status);

            return name;
        }

        //操作
        public string getOperLink(string status, string PTID, string UserID)
        {
            string operLink = string.Empty;
            int _status;
            if (!int.TryParse(status, out _status))
            {
                return "";
            }

            switch (_status)
            {
                case (int)Entities.OtheTaskStatus.Untreated:
                case (int)Entities.OtheTaskStatus.Processing:
                    operLink = process(PTID, UserID);
                    break;
                case (int)Entities.OtheTaskStatus.Processed:
                    operLink = view(PTID);
                    break;
            }

            return operLink;
        }

        //处理
        private string process(string PTID, string UserID)
        {
            string link = string.Empty;
            if (right_process)
            {
                int _userID;
                if (int.TryParse(UserID, out _userID))
                {
                    //如果当前处理人为登陆者，显示处理按钮
                    if (_userID == userID)
                    {
                        link = "<a onclick=\"javascript:ProcessClick('" + PTID + "')\" href=\"javascript:void(0);\">处理</a>&nbsp;";
                    }
                }
            }
            return link;
        }

        //查看
        private string view(string PTID)
        {
            //return "<a href=\"/OtherTask/OtherTaskDealView.aspx?OtherTaskID=" + PTID + "\" target='_blank'>查看</a>&nbsp;";
            return "";
        }

        //获取操作人
        public string getOperator(string userID)
        {
            string _operator = string.Empty;
            int _userID;
            if (int.TryParse(userID, out _userID))
            {
                _operator = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return _operator;
        }

        public string getACStatusName(string value)
        {
            if (value == "-1")
                return "--";
            return BLL.Util.GetEnumOptText(typeof(ProjectACStatus), CommonFunction.ObjectToInteger(value));
        }
    }
}