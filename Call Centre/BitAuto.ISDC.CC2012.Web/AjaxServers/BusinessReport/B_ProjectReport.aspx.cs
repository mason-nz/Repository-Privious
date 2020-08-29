using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    public partial class B_ProjectReport : PageBase
    {
        #region
        public string AgentID  //即UserID
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentID"); }
        }
        public string AgentNum
        {
            get { return BLL.Util.GetCurrentRequestStr("AgentNum"); }
        }
        public string StartTime
        {
            get { return BLL.Util.GetCurrentRequestStr("StartTime"); }
        }
        public string EndTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndTime"); }
        }
        public string BusinessType
        {
            get { return BLL.Util.GetCurrentRequestStr("BusinessType"); }
        }
        public string ProjectID
        {
            get { return BLL.Util.GetCurrentRequestStr("ProjectID"); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        public int userID = 0;
        public int GroupLength = 8;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        private void BindData()
        {
            QueryProjectReport model = new QueryProjectReport();
            if (!string.IsNullOrEmpty(AgentID))
            {
                int _userid = 0;
                if (int.TryParse(AgentID, out _userid))
                {
                    model.UserID = _userid;
                }
            }
            if (!string.IsNullOrEmpty(AgentNum))
            {
                model.AgentNum = AgentNum;
            }
            if (!string.IsNullOrEmpty(ProjectID))
            {
                int _projectid = 0;
                if (int.TryParse(ProjectID, out _projectid))
                {
                    model.ProjectID = _projectid;
                }
            }
            if (!string.IsNullOrEmpty(BusinessType))
            {
                int _businesstype = 0;
                if (int.TryParse(BusinessType, out _businesstype))
                {
                    model.BusinessType = _businesstype;
                }
            }
            else
            {//默认是其他任务
                model.BusinessType = 4;
            }
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                DateTime _begintime = DateTime.Now;
                DateTime.TryParse(StartTime, out _begintime);
                DateTime _endtime = DateTime.Now;
                DateTime.TryParse(EndTime, out _endtime);
                model.BeginTime = _begintime;
                model.EndTime = _endtime;

            }
            DataTable dt = null;
            int count = 0;
            dt = BLL.ProjectInfo.Instance.GetB_ProjectReport(model, " ", BLL.PageCommon.Instance.PageIndex, PageSize, out count, userID);
           


            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["assigncount"] = row["assigncount"] == DBNull.Value ? "-" : row["assigncount"];
                    row["tjcount"] = row["tjcount"] == DBNull.Value ? "0" : row["tjcount"];
                    row["jtcount"] = row["jtcount"] == DBNull.Value ? "0" : row["jtcount"];
                    row["successcount"] = row["successcount"] == DBNull.Value ? "0" : row["successcount"];
                    row["wjtcount"] = row["wjtcount"] == DBNull.Value ? "0" : row["wjtcount"];
                    row["jtfailcount"] = row["jtfailcount"] == DBNull.Value ? "0" : row["jtfailcount"];
                }
            }
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();


            this.litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }
        /// <summary>
        /// 生成百分比
        /// </summary>
        /// <param name="aDividend"></param>
        /// <param name="bDivisor"></param>
        /// <returns></returns>
        public string producelv(string aDividend, string bDivisor)
        {
            return BLL.Util.ProduceLv(aDividend, bDivisor);
        }
    }
}