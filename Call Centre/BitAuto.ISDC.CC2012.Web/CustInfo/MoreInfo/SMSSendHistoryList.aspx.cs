using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class SMSSendHistoryList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// 客户ID
        /// <summary>
        /// 客户ID
        /// </summary>
        private string CustID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CustID");
            }
        }

        private string CRMCustID
        {
            get
            {
                if (CustID.ToUpper().StartsWith("CB"))
                {
                    return "";
                }
                else
                {
                    return CustID;
                }
            }
        }
        private string CBID
        {
            get
            {
                if (CustID.ToUpper().StartsWith("CB"))
                {
                    return CustID;
                }
                else
                {
                    return "";
                }
            }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
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
            Entities.QuerySMSSendHistory query = new Entities.QuerySMSSendHistory();
            query.CRMCustID = CRMCustID;
            query.CBID = CBID;
            query.LoginID = null;
            DataTable dt = BLL.SMSSendHistory.Instance.GetSMSSendHistory(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            //分页控件
            AjaxPager_SMSRecord.PageSize = 10;
            AjaxPager_SMSRecord.InitPager(RecordCount);
        }
        /// 获取crm客户链接
        /// <summary>
        /// 获取crm客户链接
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <returns></returns>
        public string GetCrmUrl(object CRMCustID)
        {
            if (CRMCustID == null || CRMCustID.ToString() == "")
            {
                return "&nbsp;";
            }
            else
            {
                return "<a href='../../CustCheck/CrmCustSearch/CustDetail.aspx?CustID= " + CRMCustID + "'  target='_blank'>" + CRMCustID + "</a>&nbsp;";
            }
        }
        /// 获取任务链接
        /// <summary>
        /// 获取任务链接
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public string GetTaskUrl(string TaskID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, "", "");
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return "<a target='_blank' href='" + url + "' class='linkBlue'>" + TaskID + "</a>";
        }
    }
}