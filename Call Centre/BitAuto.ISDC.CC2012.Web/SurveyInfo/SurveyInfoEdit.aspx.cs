using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo
{
    public partial class SurveyInfoEdit : PageBase
    {
        /// <summary>
        /// 问卷ID
        /// </summary>
        public string SIID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["SIID"]) ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["SIID"]);
            }
        }

        public string CategoryID = "-1";
        public string BGID = "-1";
        public string IsHaveRole = "0";
        public string status = "";

         public string QuestionLinkId = "1";//当前最大序号

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string msg = "";

                if (BLL.Util.CheckButtonRight("SYS024BUT5003") && BLL.Util.CheckButtonRight("SYS024BUT5001"))
                {
                    IsHaveRole = "1";
                }

                #region 编辑

                if (SIID != string.Empty)
                {
                    //编辑试卷

                    CheckPar(out msg);
                    if (msg == "")
                    {
                        this.Title = "编辑问卷";
                        this.Pagetitle.InnerText = "编辑问卷";

                        BindSurveyInfo();
                        BindSurveyQuestion();
                    }
                }
                #endregion

            }
        }

        private void CheckPar(out string msg)
        {
            msg = "";
            int intVal = 0;
            if (!int.TryParse(SIID, out intVal))
            {
                msg += "参数个数不正确";
            }

        }


        /// <summary>
        /// 绑定调查问卷问题
        /// </summary>
        private void BindSurveyQuestion()
        {
            int totalCount = 0;
            Entities.QuerySurveyQuestion query = new Entities.QuerySurveyQuestion();
            query.SIID = int.Parse(SIID);
            query.Status = 0;

            DataTable dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "OrderNum", 1, 9999, out totalCount);

            if (totalCount > 0)
            {
                this.rpQuestionList.DataSource = dt;
                this.rpQuestionList.DataBind();

                QuestionLinkId = (totalCount+1).ToString();
            }
        }

        /// <summary>
        /// 绑定调查问卷信息
        /// </summary>
        private void BindSurveyInfo()
        {
            Entities.SurveyInfo model = BLL.SurveyInfo.Instance.GetSurveyInfo(int.Parse(SIID));
            if (model != null)
            {
                txtName.Value = model.Name;
                selGroup.Value = model.BGID.ToString();
                CategoryID = model.SCID.ToString();
                BGID = model.BGID.ToString();
                txtDes.Value = model.Description;
                hidSiid.Value = model.SIID.ToString();
                status = model.Status.ToString();
            }
        }

        /// <summary>
        /// 绑定业务分组
        /// </summary>
        private void BindGroup()
        {
            int userId = BLL.Util.GetLoginUserID();
            DataTable dt = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userId);
            this.selGroup.DataValueField = "BGID";
            this.selGroup.DataTextField = "Name";
            this.selGroup.DataSource = dt;
            this.selGroup.DataBind();
            this.selGroup.Items.Insert(0, new ListItem("请选择", "-1"));
        }

        protected void rpQuestionList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int intVal = 0;
                int totalCount = 0;
                string Sqid = "";

                #region 取问题ID

                HtmlInputHidden InputID = (HtmlInputHidden)e.Item.FindControl("hidItemSQID");
                if (InputID != null)
                {
                    Sqid = InputID.Value;
                }

                #endregion

                if (Sqid != "" && int.TryParse(Sqid, out intVal))
                {
                    #region 绑定选项

                    Entities.QuerySurveyOption optionQuery = new Entities.QuerySurveyOption();
                    optionQuery.SQID = int.Parse(Sqid);
                    optionQuery.Status = 0;
                    DataTable optionDt = BLL.SurveyOption.Instance.GetSurveyOption(optionQuery, " OrderNum ", 1, 999, out totalCount);

                    foreach (DataRow dr in optionDt.Rows)
                    {
                        if (dr["linkid"].ToString() != "0")
                        {
                            string linkid = dr["linkid"].ToString();

                            DataTable questDt = (DataTable)this.rpQuestionList.DataSource;
                            for (int i = 0; i < questDt.Rows.Count; i++)
                            {
                                if (questDt.Rows[i]["SQID"].ToString() == linkid)
                                {
                                    dr["linkid"] = i + 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (totalCount > 0)
                    {
                        Repeater rpOptionList = e.Item.FindControl("rpOptionList") as Repeater;
                        if (rpOptionList != null)
                        {
                            rpOptionList.DataSource = optionDt;
                            rpOptionList.DataBind();
                        }
                    }

                    #endregion

                    #region 绑定矩阵

                    Entities.QuerySurveyMatrixTitle matrixQuery = new Entities.QuerySurveyMatrixTitle();
                    matrixQuery.SQID = int.Parse(Sqid);
                    matrixQuery.Status = 0;

                    DataTable matrixDt = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(matrixQuery, " CreateTime ", 1, 999, out totalCount);
                    if (totalCount > 0)
                    {
                        Repeater rpMatrixTitleList = e.Item.FindControl("rpMatrixTitleList") as Repeater;
                        if (rpMatrixTitleList != null)
                        {
                            rpMatrixTitleList.DataSource = matrixDt;
                            rpMatrixTitleList.DataBind();
                        }
                    }

                    #endregion

                }

            }
        }
    }
}