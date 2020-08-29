using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class CallRecordList : BitAuto.ISDC.CC2012.Web.Base.PageBase
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

        #endregion

        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID

        public int PageSize
        {
            get
            {
                return this.AjaxPager_CallRecord.PageSize;
            }
        }
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        CustInfoHelper ch = new CustInfoHelper();
        private void BindData()
        {
            Entities.QueryCallRecordInfo query = new QueryCallRecordInfo();
            query.LoginID = -1;
            query.CRMCustID = ch.CustID;
            string tableEndName = ""; //查询现在表数据
            DataTable dt = BLL.CallRecordInfo.Instance.GetCallRecordInfo(query, "c.CreateTime desc", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            //分页控件
            AjaxPager_CallRecord.PageSize = 5;
            AjaxPager_CallRecord.InitPager(RecordCount);
        }
        private Random R = new Random();
        public string GetViewUrl(string TaskID, string BGID, string SCID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
        }
    }
}