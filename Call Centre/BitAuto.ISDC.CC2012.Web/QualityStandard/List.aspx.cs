using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.QualityScoring
{
    public partial class List : PageBase
    {
        #region 属性

        public string RequestScoreBeginTime
        {
            get { return HttpContext.Current.Request["ScoreBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ScoreBeginTime"].ToString()); }
        }
        public string RequestScoreEndTime
        {
            get { return HttpContext.Current.Request["ScoreEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ScoreEndTime"].ToString()); }
        }
        public string RequestRecordBeginTime
        {
            get { return HttpContext.Current.Request["RecordBeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecordBeginTime"].ToString()); }
        }
        public string RequestRecordEndTime
        {
            get { return HttpContext.Current.Request["RecordEndTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RecordEndTime"].ToString()); }
        }
        public string RequestAgent
        {
            get { return HttpContext.Current.Request["Agent"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Agent"].ToString()); }
        }
        public string RequestUserID
        {
            get { return HttpContext.Current.Request["UserID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["UserID"].ToString()); }
        }
        public string RequestScoreCreater
        {
            get { return HttpContext.Current.Request["ScoreCreater"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ScoreCreater"].ToString()); }
        }
        public string RequestScoreTable
        {
            get { return HttpContext.Current.Request["ScoreTable"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ScoreTable"].ToString()); }
        }
        public string RequestScoreStatus
        {
            get { return HttpContext.Current.Request["ScoreStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ScoreStatus"].ToString()); }
        }
        public string RequestStateResult
        {
            get { return HttpContext.Current.Request["StateResult"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["StateResult"].ToString()); }
        }
        public string RequestGroup
        {
            get { return HttpContext.Current.Request["SelGroup"] == null ? "-1" : HttpContext.Current.Request["SelGroup"].ToString(); }
        }
        /// <summary>
        /// 质检成绩统计传过来的合格量参数
        /// </summary>
        public string RequestIsQualified
        {
            get { return HttpContext.Current.Request["IsQualified"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["IsQualified"].ToString()); }
        }

        #endregion
        public bool right_Export = false;//导出的功能权限
        public bool isXiAn = false;//当前登录人所属区域

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int currentUserID = BLL.Util.GetLoginUserID();
                right_Export = BLL.Util.CheckRight(currentUserID, "SYS024BUT600105");

                Entities.EmployeeAgent agent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(currentUserID);
                if (agent != null)
                {
                    if (agent.RegionID == 2)
                    {
                        isXiAn = true;
                    }
                }
            }
        }

        public string DateToPara(string dt)
        {
            DateTime a = new DateTime();
            if (DateTime.TryParse(dt, out a))
            {
                return a.ToString("yyyy-MM-dd");
            }
            return dt;
        }
    }
}