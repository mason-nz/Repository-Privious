using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class StatesDetailsExport : PageBase
    {
        #region 定义属性


        public string AgentID
        {
            get
            {
                return Request.QueryString["u"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["u"].Trim());
            }
        }

        public string AgentNum
        {
            get
            {
                return Request.QueryString["n"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["n"].Trim());
            }
        }

        public string StartTime
        {
            get
            {
                return Request.QueryString["t"] == null
                    ? string.Empty
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["t"]);
            }
        }

        public string State
        {
            get
            {
                return Request.QueryString["s"] == null
                    ? "-1"
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["s"]);
            }
        }

        public string AgentAuxState
        {
            get
            {
                return Request.QueryString["as"] == null
                    ? "-1"
                    : HttpContext.Current.Server.UrlDecode(Request.QueryString["as"]);
            }
        }

        #endregion

        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加状态明细 【导出】操作权限
                userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024MOD400901"))
                {
                    ExprotExcel(BindData());
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private DataTable BindData()
        {
            Entities.QueryAgentStateDetail query = new Entities.QueryAgentStateDetail() { LoginID = BLL.Util.GetLoginUserID().ToString() };
            if (!string.IsNullOrEmpty(AgentID))
            {
                query.AgentID = AgentID.Trim();
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                query.AgentNum = AgentNum.Trim();
            }

            if (!string.IsNullOrEmpty(StartTime))
            {
                query.StartTime = StartTime;
            }
            if (!string.IsNullOrEmpty(State))
            {
                query.State = State;
            }
            if (!string.IsNullOrEmpty(AgentAuxState))
            {
                query.AgentAuxState = AgentAuxState;
            }

            int count;
            return BitAuto.ISDC.CC2012.BLL.AgentStateDetail.Instance.GetStateDetail(query, 1, -1, BLL.Util.GetLoginUserID(), out count);
        }

        private void ExprotExcel(DataTable dt)
        {

            //要导出的字段
            Dictionary<string, string> exportColums = new Dictionary<string, string>();

            exportColums.Add("rq", "日期");
            exportColums.Add("usedGroup", "所属分组");
            exportColums.Add("TrueName", "客服");
            exportColums.Add("AgentNum", "工号");
            exportColums.Add("state", "状态");
            exportColums.Add("auxState", "辅助状态");
            exportColums.Add("startTime", "状态开始时间");
            exportColums.Add("endTime", "状态结束时间");
            exportColums.Add("dur", "持续时长");


            //字段排序
            dt.Columns["rq"].SetOrdinal(0);
            dt.Columns["usedGroup"].SetOrdinal(1);
            dt.Columns["TrueName"].SetOrdinal(2);
            dt.Columns["AgentNum"].SetOrdinal(3);
            dt.Columns["state"].SetOrdinal(4);
            dt.Columns["auxState"].SetOrdinal(5);
            dt.Columns["startTime"].SetOrdinal(6);
            dt.Columns["endTime"].SetOrdinal(7);
            dt.Columns["dur"].SetOrdinal(8);


            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (exportColums.ContainsKey(dt.Columns[i].ColumnName))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = exportColums[dt.Columns[i].ColumnName];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                }
            }

            BLL.Util.ExportToCSV("状态明细记录" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), dt);

        }

    }
}