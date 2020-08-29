using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class MainList : PageBase
    {
        #region 属性
        private string RequestTaskID  //任务id
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        private string RequestCustName  //客户姓名
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestYpOrderID //易湃订单
        {
            get { return HttpContext.Current.Request["YpOrderID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["YpOrderID"].ToString()); }
        }
        private string RequestDealPerson //处理人
        {
            get { return HttpContext.Current.Request["DealPerson"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DealPerson"].ToString()); }
        }
        private string RequestOrderType    //订单类型
        {
            get { return HttpContext.Current.Request["OrderType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["OrderType"].ToString()); }
        }
        private string RequestIsDealerHave   //是否推荐经销商
        {
            get { return HttpContext.Current.Request["DealerHave"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DealerHave"].ToString()); }
        }
        private string RequestTaskStatus //任务状态
        {
            get { return HttpContext.Current.Request["TaskStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskStatus"].ToString()); }
        }
        private string RequestCreatBeginTime //任务创建时间 （开始时间）
        {
            get { return HttpContext.Current.Request["CreatBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreatBeginTime"].ToString()); }
        }
        private string RequestCreatEndTime //任务创建时间 （结束时间）
        {
            get { return HttpContext.Current.Request["CreatEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CreatEndTime"].ToString()); }
        }
        private string RequestSubBeginTime //任务提交时间 （开始时间）
        {
            get { return HttpContext.Current.Request["SubBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SubBeginTime"].ToString()); }
        }
        private string RequestSubEndTime //任务提交时间 （结束时间）
        {
            get { return HttpContext.Current.Request["SubEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SubEndTime"].ToString()); }
        }

        private string Reason
        {
            get { return HttpContext.Current.Request["Reason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Reason"].ToString()); }
        }
        private string ddlArea
        {
            get { return HttpContext.Current.Request["ddlArea"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ddlArea"].ToString()); }
        }
        private string ProvinceID
        {
            get { return HttpContext.Current.Request["ProvinceID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].ToString()); }
        }
        private string CityID
        {
            get { return HttpContext.Current.Request["CityID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CityID"].ToString()); }
        }
        //任务类型，0是无主订单，不等于0是免费订单
        private string TaskType
        {
            get { return HttpContext.Current.Request["TaskType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskType"].ToString()); }
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

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            //string fields = Entities.CustHistoryInfo.SelectFieldStr;

            //QueryCustHistoryInfo query = BLL.CustHistoryInfo.Instance.GetQueryModel(RequestTaskID, RequestCustName, RequestBeginTime, RequestEndTime,
            //                          RequestConsultID, RequestQuestionType, RequestQuestionQuality, RequestIsComplaint, RequestProcessStatus, RequestStatus);

            //DataTable dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query, "chi.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, fields, out RecordCount);
            //repeaterTableList.DataSource = dt;
            //repeaterTableList.DataBind();
            try
            {
                if (!int.TryParse(RequestPageSize, out PageSize))
                {
                    PageSize = 20;
                }
                DateTime dateVal = new DateTime();
                Entities.QueryOrderTask query = new QueryOrderTask();
                if (!string.IsNullOrEmpty(RequestCustName))
                {
                    query.UserName = RequestCustName;
                }
                if (!string.IsNullOrEmpty(RequestTaskID))
                {
                    query.TaskID = Convert.ToInt32(RequestTaskID);
                }
                if (!string.IsNullOrEmpty(RequestYpOrderID))
                {
                    query.YpOrderID = Convert.ToInt32(RequestYpOrderID);
                }

                if (!string.IsNullOrEmpty(RequestDealPerson) && RequestDealPerson != "-1")
                {
                    query.AssignUserID = Convert.ToInt32(RequestDealPerson);
                }
                if (!string.IsNullOrEmpty(RequestOrderType))
                {
                    query.TypeStr = RequestOrderType;
                }
                if (!string.IsNullOrEmpty(RequestIsDealerHave))
                {
                    query.IsSelectdMsmemberstr = RequestIsDealerHave;
                }
                if (!string.IsNullOrEmpty(RequestTaskStatus))
                {
                    query.StatuStr = RequestTaskStatus;
                }
                if (!string.IsNullOrEmpty(RequestCreatBeginTime) && DateTime.TryParse(RequestCreatBeginTime, out dateVal))
                {
                    query.CreateTimeBegin = dateVal;
                }
                if (!string.IsNullOrEmpty(RequestCreatEndTime) && DateTime.TryParse(RequestCreatEndTime, out dateVal))
                {
                    query.CreateTimeEnd = dateVal;
                }
                if (!string.IsNullOrEmpty(RequestSubBeginTime) && DateTime.TryParse(RequestSubBeginTime, out dateVal))
                {
                    query.SubmitTimeBegin = dateVal;
                }
                if (!string.IsNullOrEmpty(RequestSubEndTime) && DateTime.TryParse(RequestSubEndTime, out dateVal))
                {
                    query.SubmitTimeEnd = dateVal;
                }
                if (!string.IsNullOrEmpty(Reason) && Reason != "-1")
                {
                    query.NoDealerReasonID = Convert.ToInt32(Reason);

                }
                if (!string.IsNullOrEmpty(ddlArea) && ddlArea != "-1")
                {
                    query.Area = ddlArea;
                }

                if (!string.IsNullOrEmpty(ProvinceID) && ProvinceID != "-1")
                {
                    query.ProvinceID = Convert.ToInt32(ProvinceID);
                }
                if (!string.IsNullOrEmpty(CityID) && CityID != "-1")
                {
                    query.CityID = Convert.ToInt32(CityID);
                }
                //任务类型，0是无主订单，不等于0是免费订单
                if (!string.IsNullOrEmpty(TaskType))
                {
                    query.TaskType = TaskType;
                }

                DataTable dt = BLL.OrderTask.Instance.GetOrderTaskList(query, "case a.source when 2 then b.OrderCreateTime when 1 then c.OrderCreateTime when 3 then c.OrderCreateTime  end desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();

                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
            catch (Exception ex)
            {

            }
        }







        //操作
        public string getOperator(string taskID, string status, string AssignUserID)
        {
            //分配
            bool AssignTask = false;
            //回收
            bool RevokeTask = false;
            //当前登录人
            int userid = BLL.Util.GetLoginUserID();

            AssignTask = BLL.Util.CheckButtonRight("SYS024BUT1202");
            RevokeTask = BLL.Util.CheckButtonRight("SYS024BUT1203");
            string operatorStr = string.Empty;
            if (!string.IsNullOrEmpty(taskID) && !string.IsNullOrEmpty(status))
            {
                //未分配
                if (Convert.ToInt32(status) == (int)TaskStatus.NoAllocation)
                {
                    if (AssignTask == true)
                    {
                        operatorStr += "<a href='javascript:void(0)' onclick='javascript:AssignTaskOne(" + taskID + ");'  class='linkBlue'>分配</a>";
                    }
                }
                //未处理
                else if (Convert.ToInt32(status) == (int)TaskStatus.NoProcess || Convert.ToInt32(status) == (int)TaskStatus.Processing)
                {
                    if (userid.ToString() == AssignUserID)
                    {
                        operatorStr += "<a href='../../TaskManager/NoDealerOrder/NoDealerOrderEdit.aspx?TaskID=" + taskID + "' class='linkBlue' target='_bank'>处理</a>";
                    }
                }
                else if (Convert.ToInt32(status) == (int)TaskStatus.Processed)
                {
                    operatorStr += "<a href='../../TaskManager/NoDealerOrder/NoDealerOrderView.aspx?TaskID=" + taskID + "' class='linkBlue' target='_bank'>查看</a>";
                }

            }
            return operatorStr;
        }
    }
}