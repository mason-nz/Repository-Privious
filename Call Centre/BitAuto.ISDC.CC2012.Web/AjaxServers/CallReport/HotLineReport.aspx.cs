using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class HotLineReport : PageBase
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get { return BLL.Util.GetCurrentRequestQueryStr("BusinessType"); } }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get { return BLL.Util.GetCurrentRequestQueryStr("StartTime"); } }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get { return BLL.Util.GetCurrentRequestQueryStr("EndTime"); } }
        /// <summary>
        /// 统计方式
        /// </summary>
        public string ShowTime { get { return BLL.Util.GetCurrentRequestQueryStr("ShowTime"); } }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userID);
                //不是西安
                if (employeeagent == null || employeeagent.RegionID != 2)
                {
                    total_count.Text = "0";
                    objecttype.Text = "--";
                    n_entered.Text = "0";
                    n_entered_out.Text = "0";
                    n_answered.Text = "0";
                    pc_n_answered.Text = "0.00%";
                    pc_n_distrib_in_tr.Text = "0.00%";
                    av_t_answered.Text = "0.00";
                    return;
                }

                if (string.IsNullOrEmpty(BusinessType) ||
                    string.IsNullOrEmpty(StartTime) ||
                    string.IsNullOrEmpty(EndTime) ||
                    string.IsNullOrEmpty(ShowTime))
                {
                    return;
                }
                else
                {
                    BindData();
                }
            }
        }
        /// 查询数据
        /// <summary>
        /// 查询数据
        /// </summary>
        private void BindData()
        {
            QueryRoutepoint query = new QueryRoutepoint();
            query.BusinessType = this.BusinessType;
            query.StartTime = this.StartTime;
            query.EndTime = this.EndTime;
            query.ShowTime = this.ShowTime;
            DataTable hzdt = new DataTable();
            DataTable dt = BLL.Routepoint.Instance.GetRoutepointData(query, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, out hzdt);
            if (dt != null)
            {
                repeaterList.DataSource = dt;
                repeaterList.DataBind();
            }
            if (hzdt != null && hzdt.Rows.Count > 0)
            {
                total_count.Text = RecordCount.ToString();
                objecttype.Text = "--";
                n_entered.Text = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_entered"]).ToString();
                n_entered_out.Text = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_entered_out"]).ToString();
                n_answered.Text = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_answered"]).ToString();
                pc_n_answered.Text = DecimalToStr(hzdt.Rows[0]["pc_n_answered"]) + "%";
                pc_n_distrib_in_tr.Text = DecimalToStr(hzdt.Rows[0]["pc_n_distrib_in_tr"]) + "%";
                av_t_answered.Text = DecimalToStr(hzdt.Rows[0]["av_t_answered"]);
            }
            TotalCount.Value = RecordCount.ToString();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }
        /// 数字转换
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string DecimalToStr(object o)
        {
            return CommonFunction.ObjectToDouble(o).ToString("0.00");
        }
    }
}