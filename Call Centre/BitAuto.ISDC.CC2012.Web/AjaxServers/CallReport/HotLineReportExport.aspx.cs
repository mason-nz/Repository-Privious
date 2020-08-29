using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class HotLineReportExport : PageBase
    {
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get { return BLL.Util.GetCurrentRequestStr("BusinessType"); } }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartTime { get { return BLL.Util.GetCurrentRequestStr("StartTime"); } }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndTime { get { return BLL.Util.GetCurrentRequestStr("EndTime"); } }
        /// <summary>
        /// 统计方式
        /// </summary>
        public string ShowTime { get { return BLL.Util.GetCurrentRequestStr("ShowTime"); } }

        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            //增加 热线数据 【导出】操作权限
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT4081"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
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
                ExportData();
            }
        }

        private void ExportData()
        {
            QueryRoutepoint query = new QueryRoutepoint();
            query.BusinessType = this.BusinessType;
            query.StartTime = this.StartTime;
            query.EndTime = this.EndTime;
            query.ShowTime = this.ShowTime;
            DataTable hzdt = new DataTable();
            DataTable dt = BLL.Routepoint.Instance.GetRoutepointData(query, 1, -1, out RecordCount, out hzdt);
            if (dt != null)
            {
                if (hzdt != null && hzdt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["begintime"] = "合计（共" + RecordCount + "项）";
                    dr["objecttype"] = "--";
                    dr["n_entered"] = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_entered"]).ToString();
                    dr["n_entered_out"] = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_entered_out"]).ToString();//修改转人工量指标 2015-3-4 强斐
                    dr["n_answered"] = CommonFunction.ObjectToInteger(hzdt.Rows[0]["n_answered"]).ToString();
                    dr["pc_n_answered"] = DecimalToStr(hzdt.Rows[0]["pc_n_answered"]) + "%";
                    dr["pc_n_distrib_in_tr"] = DecimalToStr(hzdt.Rows[0]["pc_n_distrib_in_tr"]) + "%";
                    dr["av_t_answered"] = DecimalToStr(hzdt.Rows[0]["av_t_answered"]);//队列平均排队时长 qiangfei 2015-3-5

                    dt.Rows.Add(dr);
                }
                dt.Columns.Remove("RowNumber");

                dt.Columns["begintime"].ColumnName = "日期";
                dt.Columns["objecttype"].ColumnName = "业务类型";
                dt.Columns["n_entered"].ColumnName = "呼入量";
                dt.Columns["n_entered_out"].ColumnName = "转人工量";//修改转人工量指标 2015-3-4 强斐
                dt.Columns["n_answered"].ColumnName = "电话总接通量";
                dt.Columns["pc_n_answered"].ColumnName = "接通率";
                dt.Columns["pc_n_distrib_in_tr"].ColumnName = "30秒内服务水平";
                dt.Columns["av_t_answered"].ColumnName = "平均等待时长（秒）";//队列平均排队时长 qiangfei 2015-3-5

                BLL.Util.ExportToCSV("热线数据报表" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }

        /// 数字转换
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string DecimalToStr(object o)
        {
            return CommonFunction.ObjectToDecimal(o).ToString("0.00");
        }
    }
}