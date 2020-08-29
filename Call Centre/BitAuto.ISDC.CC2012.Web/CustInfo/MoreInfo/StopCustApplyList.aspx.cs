using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class StopCustApplyList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
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

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
            }
        }


        /// <summary>
        /// 短信状态
        /// </summary>
        public string SMSStatus
        {
            get
            {
                return Request["SMSStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["SMSStatus"].ToString().Trim());
            }
        }
        //手机号
        private string HandNum
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["HandNum"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["HandNum"].ToString()); }
        }
        //发送内容
        private string SendContent
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["SendContent"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SendContent"].ToString()); }
        }
        //接收人
        private string Reservicer
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["Reservicer"]) ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Reservicer"].ToString()); }
        }
        //坐席
        private int RequestAgentUserID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Request["AgentUserID"]) ? -2 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["AgentUserID"])); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 5;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        CustInfoHelper ch = new CustInfoHelper();

        private void BindData()
        {
            Entities.QueryStopCustApply query = new Entities.QueryStopCustApply();
            int _loginID = -2;
            _loginID = userID;
            query.LoginID = userID;
            query.CustID = ch.CustID;
            DataTable dt = BLL.StopCustApply.Instance.GetStopCustApplyNoDR(query, "sca.ApplyTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            //分页控件
            AjaxPager_ApplyRecord.PageSize = 5;
            AjaxPager_ApplyRecord.InitPager(RecordCount);
        }

        public string BindApplyReason(string v)
        {
            string sretval = "--";
            int itemp = 0;
            if (Int32.TryParse(v, out itemp))
            {
                sretval = BLL.Util.GetEnumOptText(typeof(Entities.StopApplyReason), itemp);
            }
            return sretval;
        }
        public string BindStopRemark(string v)
        {
            string sretval = "--";
            int itemp = 0;
            if (Int32.TryParse(v, out itemp))
            {
                sretval = BLL.Util.GetEnumOptText(typeof(Entities.StopRemark), itemp);
            }

            return sretval;
        }

        public string BindStopStatus(string v, string applytype)
        {
            string result = "--";
            int itemp = 0;
            if (Int32.TryParse(v, out itemp))
            {
                result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustStopStatus), itemp);
                //停用类型
                if (applytype == "1")
                {
                    if (itemp == 2)
                    {
                        result = "待停用";
                    }
                    else if (itemp == 3)
                    {
                        result = "已停用";
                    }
                }
                //启用类型
                else if (applytype == "2")
                {
                    if (itemp == 2)
                    {
                        result = "待启用";
                    }
                    else if (itemp == 3)
                    {
                        result = "已启用";
                    }
                }
            }
            return result;
        }
    }
}