using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.ExamPaperStorage
{
    public partial class ExamPaperEdit : PageBase
    {

        /// <summary>
        /// 试卷ID
        /// </summary>
        public string EPID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["epid"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["epid"]);
            }
        }

        public string ExitAddress;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 int userID = BLL.Util.GetLoginUserID();

                 if (!BLL.Util.CheckRight(userID, "SYS024BUT3201"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                } 
                ExitAddress = ConfigurationManager.AppSettings["ExitAddress"].ToString();
                BindCatage();
                BindBGIDS();
                if (EPID != string.Empty)
                {

                    //编辑试卷

                    this.Title = "编辑试卷";
                    this.Pagetitle.InnerText = "编辑试卷";

                    BindPaper();

                    BindBigQustion();
                }
            }
        }

        private void BindBGIDS()
        {
            //BLL.BusinessGroup.Instance.GetInUseBusinessGroup(userid);
            //BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(BLL.Util.GetLoginUserID()).RegionID;
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userId);// BLL.BusinessGroup.Instance.GetInUseBusinessGroup(userId);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                ddlBGIDs.DataTextField = "Name";
                ddlBGIDs.DataValueField = "BGID";
                ddlBGIDs.DataSource = dt;
                ddlBGIDs.DataBind();
                if (EPID != string.Empty)
                {
                    ddlBGIDs.SelectedValue = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userId).BGID.ToString();
                }
            }
        }

        /// <summary>
        /// 绑定大题信息
        /// </summary>
        private void BindBigQustion()
        {
            long epid = 0;
            if (long.TryParse(EPID, out epid))
            {
                Entities.QueryExamBigQuestion query = new Entities.QueryExamBigQuestion();
                query.EPID = epid;

                int totalCount = 0;
                DataTable dt = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(query, "", 1, 99999, out totalCount);
                if (dt != null)
                {
                    RptBigQuestion.DataSource = dt;
                    RptBigQuestion.DataBind();
                }
            }
        }

        /// <summary>
        /// 绑定分类
        /// </summary>
        private void BindCatage()
        {
            Entities.QueryExamCategory query = new Entities.QueryExamCategory();
            query.Type = 2;
            int totalCount = 0;

            DataTable dt = BLL.ExamCategory.Instance.GetExamCategory(query, "", 1, 999, out totalCount);
            if (dt != null)
            {
                RptCatage.DataSource = dt;
                RptCatage.DataBind();
            }
        }

        /// <summary>
        /// 绑定试卷信息
        /// </summary>
        private void BindPaper()
        {
            long epid = 0;
            if (long.TryParse(EPID, out epid))
            {
                Entities.ExamPaper model = BLL.ExamPaper.Instance.GetExamPaper(epid);
                if (model != null)
                {
                    this.hidEPID.Value = model.EPID.ToString();
                    this.txtName.Value = model.Name;
                    this.txtDes.Value = model.ExamDesc;
                    this.txtTotalScore.Value = model.TotalScore.ToString();
                    ddlBGIDs.SelectedValue = model.BGID.ToString();
                    if (model.Status != 2)
                    {
                        this.btnSave.Visible = false;
                    }


                    #region 给分类赋值

                    foreach (RepeaterItem item in this.RptCatage.Items)
                    {
                        Control ctl = item.FindControl("ecID");
                        if (ctl != null)
                        {
                            HtmlInputRadioButton rdoBut = (HtmlInputRadioButton)ctl;
                            if (rdoBut.Value == model.ECID.ToString())
                            {
                                rdoBut.Checked = true;
                                break;
                            }
                        }
                    }
                    #endregion

                }
            }
        }

        public string GetTypeName(string AskCategory)
        {
            string typeName = "";
            switch (AskCategory)
            {
                case "1": typeName = "单选题"; break;
                case "2": typeName = "复选题"; break;
                case "3": typeName = "主观题"; break;
                case "4": typeName = "判断题"; break;
            }
            return typeName;
        }

        /// <summary>
        /// 获得实际题量
        /// </summary>
        /// <param name="bqId"></param>
        /// <returns></returns>
        public string GetRealQCount(string bqId)
        {
            string realQCount = "";



            return realQCount;
        }
    }
}