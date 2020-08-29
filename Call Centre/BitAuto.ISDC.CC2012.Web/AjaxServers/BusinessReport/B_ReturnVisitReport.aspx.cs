using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.BusinessReport;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.BusinessReport
{
    public partial class B_ReturnVisitReport : PageBase
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
        public string Year
        {
            get { return BLL.Util.GetCurrentRequestStr("Year"); }
        }
        public string Month
        {
            get { return BLL.Util.GetCurrentRequestStr("Month"); }
        }
        public string BGID
        {
            get { return BLL.Util.GetCurrentRequestStr("BGID"); }
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
            QueryReturnVisitReport model = new QueryReturnVisitReport();
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
            if (!string.IsNullOrEmpty(BGID))
            {
                int _bgid = 0;
                if (int.TryParse(BGID, out _bgid))
                {
                    model.BGID = _bgid;
                }
            }
            if (!string.IsNullOrEmpty(Year))
            {
                int _year = 0;
                if (int.TryParse(Year, out _year))
                {
                    model.Year = _year;
                }
            }
            if (!string.IsNullOrEmpty(Month))
            {
                int _month = 0;
                if (int.TryParse(Month, out _month))
                {
                    model.Month = _month;
                }
            }
            DataTable dt = null;
            int count = 0;
            dt = BLL.ProjectInfo.Instance.GetB_ReturnVisitReport(model, " ", BLL.PageCommon.Instance.PageIndex, PageSize, out count);
            //DataTable dt1 = BLL.ProjectInfo.Instance.GetB_ReturnVisitReportSum(model);


            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["dyfzmembercount"] = row["dyfzmembercount"] == DBNull.Value ? "0" : row["dyfzmembercount"];
                    row["hfmembercount"] = row["hfmembercount"] == DBNull.Value ? "0" : row["hfmembercount"];
                    row["fglv"] = BLL.Util.ProduceLv(row["hfmembercount"].ToString(), row["dyfzmembercount"].ToString());
                    row["hfcount"] = row["hfcount"] == DBNull.Value ? "0" : row["hfcount"];
                    row["wjtcount"] = row["wjtcount"] == DBNull.Value ? "0" : row["wjtcount"];
                }
            }

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();


            this.litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, count, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }
       
    }
}