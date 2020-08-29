using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class UserActionLogList : System.Web.UI.Page
    {
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }
        /// <summary>
        /// 日志内容
        /// </summary>
        private string LogInfo
        {
            get { return HttpContext.Current.Request["LogInfo"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LogInfo"].ToString()); }
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        private string selLogInType
        {
            get { return HttpContext.Current.Request["selLogInType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["selLogInType"].ToString()); }
        }
        /// <summary>
        /// 操作人类型
        /// </summary>
        private string OperUserType
        {
            get { return HttpContext.Current.Request["OperUserType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["OperUserType"].ToString()); }
        }
        /// <summary>
        /// 操作人名称
        /// </summary>
        private string OperUserName
        {
            get { return HttpContext.Current.Request["OperUserName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["OperUserName"].ToString()); }
        }
        /// <summary>
        /// 操作时间开始
        /// </summary>
        private string QueryStarttime
        {
            get { return HttpContext.Current.Request["QueryStarttime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString()); }
        }
        /// <summary>
        /// 操作时间结束
        /// </summary>
        private string QueryEndTime
        {
            get { return HttpContext.Current.Request["QueryEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString()); }
        }


        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int CurrentUserID = 0;
        public int MinCSID = 0;
        public int MaxCSID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                CurrentUserID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        private void BindData()
        {
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }
            //PageSize = 1;
            Entities.QueryUserActionLog query = new Entities.QueryUserActionLog();

            if (!string.IsNullOrEmpty(LogInfo))
            {
                query.LogInfo = LogInfo;
            }
            if (!string.IsNullOrEmpty(selLogInType) && selLogInType != "-1")
            {
                int _selLogInType = 0;
                if (int.TryParse(selLogInType, out _selLogInType))
                {
                    query.LogInType = _selLogInType;
                }
            }
            if (!string.IsNullOrEmpty(OperUserType) && OperUserType != "-1")
            {
                int _operusertype = 0;
                if (int.TryParse(OperUserType, out _operusertype))
                {
                    query.OperUserType = _operusertype;
                }
            }
            if (!string.IsNullOrEmpty(OperUserName))
            {
                query.OperUserName = OperUserName;
            }
            if (!string.IsNullOrEmpty(QueryStarttime))
            {
                query.StartTime = QueryStarttime;
            }
            if (!string.IsNullOrEmpty(QueryEndTime))
            {
                query.EndTime = QueryEndTime;
            }

            int RecordCount = 0;

            DataTable dt = BLL.UserActionLog.Instance.GetUserActionLog(query, "a.createtime desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);//BLL.OtherTaskInfo.Instance.GetOtherTaskInfoByList(query, "OtherTaskInfo.LastOptTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }

        public string GetLoginfotype(string logintype)
        {
            string logintypestr = string.Empty;
            switch (logintype)
            {
                case "1":
                    logintypestr = "坐席断网超时";
                    break;
                case "2":
                    logintypestr = "网友不发消息超时";
                    break;
                case "3":
                    logintypestr = "系统异常记录";
                    break;
                case "4":
                    logintypestr = "客户登录";
                    break;
                case "5":
                    logintypestr = "客户退出";
                    break;
                case "6":
                    logintypestr = "坐席登录";
                    break;
                case "7":
                    logintypestr = "坐席退出";
                    break;
                case "8":
                    logintypestr = "给坐席新增聊天客户";
                    break;
                case "9":
                    logintypestr = "网友断网超时";
                    break;
                case "10":
                    logintypestr = "坐席修改状态";
                    break;
                default:
                    break;
            }
            return logintypestr;
        }
    }
}