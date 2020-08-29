using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using System.Collections;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager
{
    public partial class TaskList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }
        private string RequestCustName  //客户姓名
        {
            get { return HttpContext.Current.Request["CustName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustName"].ToString()); }
        }
        private string RequestConsultID //咨询类型
        {
            get { return HttpContext.Current.Request["ConsultID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ConsultID"].ToString()); }
        }
        private string RequestQuestionType  //问题类别
        {
            get { return HttpContext.Current.Request["QuestionType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QuestionType"].ToString()); }
        }
        private string RequestQuestionQuality    //问题性质
        {
            get { return HttpContext.Current.Request["QuestionQuality"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["QuestionQuality"].ToString()); }
        }
        private string RequestIsComplaint   //是否投诉
        {
            get { return HttpContext.Current.Request["IsComplaint"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsComplaint"].ToString()); }
        }
        private string RequestProcessStatus //任务状态
        {
            get { return HttpContext.Current.Request["ProcessStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProcessStatus"].ToString()); }
        }
        private string RequestStatus    //处理状态
        {
            get { return HttpContext.Current.Request["Status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString()); }
        }
        private string RequestBeginTime //来电时间 （开始时间）
        {
            get { return HttpContext.Current.Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BeginTime"].ToString()); }
        }
        private string RequestEndTime    //来电时间 （结束时间）
        {
            get { return HttpContext.Current.Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString()); }
        }
        private string RequestIsForwarding //是否转发
        {
            get { return HttpContext.Current.Request["IsForwarding"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsForwarding"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断登录
                BindData();
            }
        }

        private void BindData()
        {
            string fields = Entities.CustHistoryInfo.SelectFieldStr;
            string _consult = "-2";
            if (RequestConsultID == "")
            {
                _consult = "0";
            }
            else
            {
                _consult = RequestConsultID;
            }

            QueryCustHistoryInfo query = BLL.CustHistoryInfo.Instance.GetQueryModel(RequestTaskID, RequestCustName, RequestBeginTime, RequestEndTime,
                                       _consult, RequestQuestionType, RequestQuestionQuality, RequestIsComplaint, RequestProcessStatus, RequestStatus, RequestIsForwarding);

            DataTable dt = BLL.CustHistoryInfo.Instance.GetCustHistoryInfo(query, "chi.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, fields, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }


        //根据任务ID得到当前受理人ID、姓名
        public string getEmployNameByTaskID(string taskID)
        {
            string employName = string.Empty;
            if (taskID != "")
            {
                QueryTaskCurrentSolveUser query = new QueryTaskCurrentSolveUser();
                query.TaskID = taskID;
                query.Status = 1;
                int count;
                DataTable dt = BLL.TaskCurrentSolveUser.Instance.GetTaskCurrentSolveUser(query, "", 1, 10000, out count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int id;
                    if (int.TryParse(dt.Rows[i]["CurrentSolveUserEID"].ToString(), out id))
                    {
                        string name = string.Empty;
                        name = "暂不实现";
                        if (!employName.Contains(name))
                        {
                            employName += name + ",";
                        }
                    }
                }
            }
            return employName.TrimEnd(',');
        }

        //获取状态名称
        public string getStatusName(string status)
        {
            string name = string.Empty;
            switch (status)
            {
                case "150001": name = "待处理";
                    break;
                case "150002": name = "处理中";
                    break;
                case "150003": name = "已处理";
                    break;
            }
            return name;
        }

        //操作
        public string getOperator(string taskID, string status)
        {
            string operatorStr = string.Empty;
            int _status;
            if (taskID != "" && int.TryParse(status, out _status))
            {
                if (_status == (int)EnumTaskStatus.TaskStatusOver)
                {
                    operatorStr += "<a target='_blank' href='TaskDetail.aspx?TaskID=" + taskID + "' class='linkBlue'>查看</a>";
                }
                else
                {
                    operatorStr += "<a target='_blank' href='TaskProcess.aspx?TaskID=" + taskID + "' class='linkBlue'>处理</a>";
                }
            }
            return operatorStr;
        }
    }
}