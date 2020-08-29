using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class Display : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region
        public string SPIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SPIID"));
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SurveyProjectInfoBind();
            }
        }

        private void SurveyProjectInfoBind()
        {
            int spIid = 0;
            if (int.TryParse(SPIID, out spIid))
            {
                Entities.QuerySurveyProjectInfo query = new QuerySurveyProjectInfo();
                //query.BGIDStr = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRightBgIDStrByUserID(BLL.Util.GetLoginUserID());
                query.LoginUserID = BLL.Util.GetLoginUserID();
                query.SPIID = spIid;
                int totalCount = 0;
                BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, "", 1, 1, out totalCount);
                if (totalCount > 0)
                {
                    Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spIid);
                    if (info != null)
                    {

                        spanProjectName.InnerText = info.Name;
                        spanDescription.InnerText = info.Description;
                        DataTable bgBt = BLL.BusinessGroup.Instance.GetBusinessGroupByBGID((int)info.BGID);
                        if (bgBt != null && bgBt.Rows.Count > 0)
                        {
                            spanBGID.InnerText = bgBt.Rows[0]["Name"].ToString();
                        }
                        Entities.SurveyCategory categoryInfo = BLL.SurveyCategory.Instance.GetSurveyCategory((int)info.SCID);
                        if (categoryInfo != null)
                        {
                            spanBCID.InnerText = categoryInfo.Name;
                        }
                        Entities.SurveyInfo surveyInfo = BLL.SurveyInfo.Instance.GetSurveyInfo((int)info.SIID);
                        spanSIID.InnerText = surveyInfo.Name;
                        spanBusinessGroup.InnerText = info.BusinessGroup;
                        spanBeginTime.InnerText = info.SurveyStartTime.ToString();
                        spanEndTime.InnerText = info.SurveyEndTime.ToString();

                        DataTable dt = BLL.SurveyPerson.Instance.GetSurveyPersonBySPIID(info.SPIID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string personNames = string.Empty;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                int userId=int.Parse(dt.Rows[i]["ExamPersonID"].ToString());
                                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userId);
                                if (i > 0)
                                {
                                    personNames += ",";
                                }
                                Entities.QuerySurveyAnswer queryAnswer = new QuerySurveyAnswer();
                                queryAnswer.CreateUserID = userId;
                                queryAnswer.SPIID = spIid;
                                int answerCount=0;
                                BLL.SurveyAnswer.Instance.GetSurveyAnswer(queryAnswer, "", 1, 1000, out answerCount);
                                if (answerCount > 0)
                                {
                                    personNames += "<a href='/SurveyInfo/PersonalSurveyInfoView.aspx?SPIID=" + spIid + "&UserID=" + userId + "' target='_blank'>" + userName + "</a>";
                                }
                                else
                                {
                                    personNames += userName;
                                }
                            }

                            spanPersons.InnerHtml = personNames;
                            spanEstimateNumber.InnerText = dt.Rows.Count.ToString();
                        }
                        int userCount = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySPIID(spIid);
                        Entities.QuerySurveyQuestion questionQuery = new QuerySurveyQuestion();
                        questionQuery.AskCategory = 5;
                        questionQuery.SIID = (int)info.SIID;

                        string scoreStr = string.Empty;
                        int questionTotalCount=0;
                        DataTable dtQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(questionQuery, "", 1, 1000, out questionTotalCount);
                        int row = 0;
                        foreach (DataRow dr in dtQuestion.Rows)
                        {
                            if (row > 0)
                            {
                                scoreStr += "；";
                            }
                            int sqId = int.Parse(dr["SQID"].ToString());
                            Entities.QuerySurveyMatrixTitle queryRow = new QuerySurveyMatrixTitle();
                            queryRow.SQID = sqId;
                            queryRow.Type = 1;
                            int rowCount = 0;
                            BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(queryRow, "", 1, 1000, out rowCount);

                            int colCount = 0;
                            queryRow.Type = 2;
                            BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(queryRow, "", 1, 1000, out colCount);

                            int sumScore = BLL.SurveyQuestion.Instance.GetQuestionForMatrixDropdownSumScore(sqId, spIid);
                            if (userCount > 0)
                            {
                                decimal score = (decimal)sumScore / userCount;
                                scoreStr += (int.Parse(dr["OrderNum"].ToString())+ 1) + "(" + score.ToString("0.00") + ")";
                            }
                        }
                        spanScore.InnerText = scoreStr;
                        spanFinshTime.InnerText = info.SurveyEndTime.ToString();
                       
                        spanTrueNumber.InnerText = userCount.ToString();
                        spanCreateUserName.InnerText = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName((int)info.CreateUserID);
                        spanCreateTime.InnerText = info.CreateTime.ToString();
                        row++;
                    }
                }
                else
                {
                    Response.Write("<script>location.href='about:blank'</script>;");
                }
            }
        }
    }
}