using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab
{
    public partial class SMSSendHistoryList : PageBase
    {
        /// 电话号码列表
        /// <summary>
        /// 电话号码列表
        /// </summary>
        private string PhoneNums
        {
            get { return BLL.Util.GetCurrentRequestStr("PhoneNums"); }
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
            PageSize = 10;
            litPagerDown.Visible = true;
            if (PhoneNums != "")
            {
                Entities.QuerySMSSendHistory query = new Entities.QuerySMSSendHistory();
                query.PhoneList = PhoneNums;
                DataTable dt = BLL.SMSSendHistory.Instance.GetSMSSendHistory(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                //分页控件
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1014);
            }
            else
            {
                repeaterTableList.DataSource = null;
                repeaterTableList.DataBind();
                litPagerDown.Text = "";
            }
        }
        /// 获取crm客户链接
        /// <summary>
        /// 获取crm客户链接
        /// </summary>
        /// <param name="CRMCustID"></param>
        /// <returns></returns>
        public string GetCrmUrl(object CRMCustID)
        {
            if (CRMCustID == null || CRMCustID.ToString() == "" || CommonFunction.ObjectToInteger(CRMCustID) <= 0)
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