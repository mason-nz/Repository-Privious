using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage
{
    public partial class AnswerList : PageBase
    {
        #region 参数

        private string IVRScore
        {
            get
            {
                return Request["IVRScore"] == null ? "" :
                HttpUtility.UrlDecode(Request["IVRScore"].ToString().Trim());
            }
        }
        private string SelSolve
        {
            get
            {
                return Request["selSolve"] == null ? "" :
                HttpUtility.UrlDecode(Request["selSolve"].ToString().Trim());
            }
        }

        private string IncomingSource
        {
            get
            {
                return Request["IncomingSource"] == null ? "" :
                HttpUtility.UrlDecode(Request["IncomingSource"].ToString().Trim());
            }
        }

        private string Name
        {
            get
            {
                return Request["Name"] == null ? "" :
                HttpUtility.UrlDecode(Request["Name"].ToString().Trim());
            }
        }

        private string ANI
        {
            get
            {
                return Request["ANI"] == null ? "" :
                HttpUtility.UrlDecode(Request["ANI"].ToString().Trim());
            }
        }

        private string Agent
        {
            get
            {
                return Request["Agent"] == null ? "" :
                HttpUtility.UrlDecode(Request["Agent"].ToString().Trim());
            }
        }

        private string TaskID
        {
            get
            {
                return Request["TaskID"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskID"].ToString().Trim());
            }
        }

        private string CallID
        {
            get
            {
                return Request["CallID"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallID"].ToString().Trim());
            }
        }

        private string BeginTime
        {
            get
            {
                return Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }

        private string EndTime
        {
            get
            {
                return Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }

        private string AgentNum
        {
            get
            {
                return Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentNum"].ToString().Trim());
            }
        }
        private string PhoneNum
        {
            get
            {
                return Request["PhoneNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["PhoneNum"].ToString().Trim());
            }
        }

        private string TaskCategory
        {
            get
            {
                return Request["TaskCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["TaskCategory"].ToString().Trim());
            }
        }

        private string SpanTime1
        {
            get
            {
                return Request["SpanTime1"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime1"].ToString().Trim());
            }
        }

        private string SpanTime2
        {
            get
            {
                return Request["SpanTime2"] == null ? "" :
                HttpUtility.UrlDecode(Request["SpanTime2"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
            }
        }

        public string Category
        {
            get
            {
                return Request["selCategory"] == null ? "" :
                HttpUtility.UrlDecode(Request["selCategory"].ToString().Trim());
            }
        }

        /// <summary>
        /// 电话状态（1-呼入，2-呼出）
        /// </summary>
        public string CallStatus
        {
            get
            {
                return Request["CallStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["CallStatus"].ToString().Trim());
            }
        }

        /// <summary>
        /// 业务线
        /// </summary>
        public string selBusinessType
        {
            get
            {
                return Request["selBusinessType"] == null ? "" :
                HttpUtility.UrlDecode(Request["selBusinessType"].ToString().Trim());
            }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;
        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                userID = BLL.Util.GetLoginUserID();
                BindData();

                BLL.Loger.Log4Net.Info(string.Format("【已接来电数据列表总耗时——总耗时】：{0}毫秒", stopwatch.Elapsed.TotalMilliseconds));
                stopwatch.Stop();
            }
        }

        private void BindData()
        {
            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;

            _loginID = userID;
            Entities.QueryCallRecordInfo query = BLL.CallRecordInfo.Instance.GetQueryModel(
                Name, ANI, Agent, TaskID, CallID, BeginTime, EndTime, AgentNum, PhoneNum, TaskCategory,
                SpanTime1, SpanTime2, AgentGroup, CallStatus, _loginID, _ownGroup, _oneSelf, Category, IVRScore, IncomingSource, selBusinessType, SelSolve
                );

            string tableEndName = BLL.Util.CalcTableNameByMonth(3, query.BeginTime.Value);
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        private Random R = new Random();
        public string GetViewUrl(string TaskID, string BGID, string SCID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
        }

        public string DateTimeToString(string value)
        {
            DateTime date = CommonFunction.ObjectToDateTime(value, new DateTime());
            if (date == new DateTime())
            {
                return "";
            }
            else
            {
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}