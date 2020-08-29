using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class TextQuestionDetail : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        public string RequestSPIID
        {
            get { return HttpContext.Current.Request["SPIID"] == null ? "" : HttpContext.Current.Request["SPIID"].ToString(); }
        }
        public string RequestSQID
        {
            get { return HttpContext.Current.Request["SQID"] == null ? "" : HttpContext.Current.Request["SQID"].ToString(); }
        }
        public string RequestSOID
        {
            get { return HttpContext.Current.Request["SOID"] == null ? "" : HttpContext.Current.Request["SOID"].ToString(); }
        }
        public string RequestAnswerContent
        {
            get { return HttpContext.Current.Request["AnswerContent"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AnswerContent"].ToString()); }
        }
        public string RequestFilterNull
        {
            get { return HttpContext.Current.Request["FilterNull"] == null ? "" : HttpContext.Current.Request["FilterNull"].ToString(); }
        }
        public string Requestpage
        {
            get { return HttpContext.Current.Request["page"] == null ? "" : HttpContext.Current.Request["page"].ToString(); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        public string questionStr = string.Empty;//问题内容 

        //按钮权限
        public bool right_view; //查看试卷

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                right_view = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT3301");

                //绑定问题
                bindQuestion();
            }
        }

        public void bindQuestion()
        {
            int _sqid;
            int _soid;
            int _spiid;
            int questionType = 0;//问题类型

            //1-选项
            if (int.TryParse(RequestSOID, out _soid) && int.TryParse(RequestSPIID, out _spiid))
            {
                Entities.SurveyOption model1_surveyOption = BLL.SurveyOption.Instance.GetSurveyOption(_soid);
                if (model1_surveyOption != null)
                {
                    Entities.SurveyQuestion model1_question = BLL.SurveyQuestion.Instance.GetSurveyQuestion(int.Parse(model1_surveyOption.SQID.ToString()));

                    if (model1_question != null)
                    {
                        questionStr += model1_question.Ask;

                        questionType = int.Parse(model1_question.AskCategory.ToString());

                        //问题类型为单选或复选
                        if (questionType == (int)Entities.AskCategory.RadioT || questionType == (int)Entities.AskCategory.CheckBoxT)
                        {
                            if (model1_surveyOption.IsBlank == 1 && model1_question.AskCategory != (int)Entities.AskCategory.TextT)
                            {
                                questionStr += " -- " + model1_surveyOption.OptionName;
                            }
                        }
                    }

                }
            }

            //2-试题
            if (int.TryParse(RequestSQID, out _sqid) && int.TryParse(RequestSPIID, out _spiid))
            {
                Entities.SurveyQuestion model_question = BLL.SurveyQuestion.Instance.GetSurveyQuestion(_sqid);
                if (model_question != null)
                {
                    questionStr += model_question.Ask;
                }
            }

            if (questionStr != string.Empty)
            {
                //绑定列表内容
                bindData();
            }
        }

        private void bindData()
        {
            Entities.QuerySurveyAnswer query = new Entities.QuerySurveyAnswer();
            if (RequestSOID != "")
            {
                query.SOID = int.Parse(RequestSOID);
            }
            if (RequestSQID != "")
            {
                query.SQID = int.Parse(RequestSQID);
            }
            if (RequestSPIID != "")
            {
                query.SPIID = int.Parse(RequestSPIID);
            }
            if (RequestAnswerContent != "")
            {
                query.AnswerContent = RequestAnswerContent;
            }
            if (RequestFilterNull != "")
            {
                query.FilterNull = int.Parse(RequestFilterNull);
            }

            int page = 1;
            if (int.TryParse(Requestpage, out page))
            {
                page = int.Parse(Requestpage);
            }
            else
            { page = 1; }

            DataTable dt = BLL.SurveyAnswer.Instance.GetSurveyAnswerByTextDetail(query, "", page, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, page, 1);
        }

        //创建人
        public string getCreateUserName(string userID)
        {
            string userName = string.Empty;
            int _userID;
            if (int.TryParse(userID, out _userID))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return userName;
        }


        //内容字数控制
        public string getContent(string content)
        {
            string Content = string.Empty;

            if (content.Length > 50)
            {
                Content = content.Substring(0, 50) + "......";
            }
            else
            {
                Content = content;
            }

            return Content;
        }
    }
}