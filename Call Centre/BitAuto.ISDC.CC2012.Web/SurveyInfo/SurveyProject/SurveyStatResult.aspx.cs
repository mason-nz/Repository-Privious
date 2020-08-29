using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class SurveyStatResult : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性定义
        private string SPIID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("SPIID");
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                QuestionBind();
            }
        }
        private void QuestionBind()
        {
            int spIid = -1;
            if (int.TryParse(SPIID, out spIid))
            {
                Entities.QuerySurveyProjectInfo query = new Entities.QuerySurveyProjectInfo();
                query.LoginUserID = BLL.Util.GetLoginUserID();
                query.SPIID = spIid;
                int count = 0;
                DataTable dtProject = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, "", 1, 1000, out count);
                if (count > 0)
                {
                    Entities.SurveyProjectInfo projectInfo = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spIid);
                    if (projectInfo != null)
                    {
                        if (projectInfo.SurveyStartTime <= DateTime.Now)
                        {
                            Entities.QuerySurveyQuestion queryQuestion = new Entities.QuerySurveyQuestion();
                            queryQuestion.SIID = (int)projectInfo.SIID;
                            queryQuestion.Status = 0;
                            int totalCount = 0;
                            DataTable dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, "OrderNum", 1, 1000, out totalCount);
                            rptSurveyQuestion.DataSource = dt;
                            rptSurveyQuestion.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptSurveyQuestion_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int sqId = int.Parse(DataBinder.Eval(e.Item.DataItem, "SQID").ToString().Trim());
                Entities.SurveyQuestion info = BLL.SurveyQuestion.Instance.GetSurveyQuestion(sqId);
                Literal lblStatHtml = e.Item.FindControl("lblStatHtml") as Literal;
                Literal lblOtherHtml = e.Item.FindControl("lblOtherHtml") as Literal;
                if (info != null)
                {
                    switch ((int)info.AskCategory)
                    {
                        case 1:
                        case 2:
                            lblStatHtml.Text = StatQuestionForMultipleChoice(sqId,int.Parse(SPIID));
                            break;
                        case 3:
                            lblStatHtml.Text = StatQuestionAnswer(int.Parse(SPIID),sqId);
                            break;
                        case 4:
                            lblStatHtml.Text = StatQuestionForMatrixRadio(sqId, int.Parse(SPIID));
                            lblOtherHtml.Text = MatrixRadioOtherStatHtml(sqId, int.Parse(SPIID));
                            break;
                        case 5:
                            lblStatHtml.Text = StatQuestionForMatrixDropdown(sqId, int.Parse(SPIID));
                            lblOtherHtml.Text = "<div class=\"tip\">提示：表格里的数值为平均分</div>";
                            break;
                    }
                }

            }
        }


        //单选统计
        private string StatQuestionForMultipleChoice(int sqId,int spiId)
        {
            StringBuilder strSB = new StringBuilder();
            bool isStatByScore = false;
            Entities.SurveyQuestion question = BLL.SurveyQuestion.Instance.GetSurveyQuestion(sqId);
            if (question.IsStatByScore == 1)
            {
                isStatByScore = true;
            }
            DataTable dt = BLL.SurveyQuestion.Instance.StatQuestionForMultipleChoice(sqId, spiId);
            strSB.Append("<table  width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><thead  style=\"background:#F2F2F2;\">");
            strSB.Append("<th width=\"50%\">选项</th><th width=\"23%\">小计</th><th width=\"27%\">比例</th></thead>");
            int surveyPersonNum = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySQID(sqId, spiId);
            int j = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (j % 2 == 0)
                {
                    strSB.Append("<tr>");
                }
                else
                {
                    strSB.Append("<tr style='background:#F2F2F2;'>");
                }
               
                Entities.SurveyOption optionInfo = BLL.SurveyOption.Instance.GetSurveyOption(int.Parse(dr["SOID"].ToString()));
                string blankStr = string.Empty;
                if ((int)optionInfo.IsBlank==1)
                {
                    blankStr = "<a href=\"TextQuestionDetail.aspx?SPIID=" + spiId + "&SOID=" + dr["SOID"].ToString() + "\" target=\"_blank\">【详情】</a>";
                }
                //如果是按分数统计，每个选项后边显示分数
                if (isStatByScore)
                {
                    strSB.Append("<td>" + dr["OptionName"].ToString() + blankStr +"("+dr["Score"]+"分)"+"</td>");
                }
                else
                {
                    strSB.Append("<td>" + dr["OptionName"].ToString() + blankStr + "</td>");
                }
                int num = int.Parse(dr["ExamNum"].ToString());
                strSB.Append("<td>" + num.ToString() + "</td>");
                decimal perOf = 0;
                if (surveyPersonNum > 0)
                {
                    perOf = ((decimal)num / (decimal)surveyPersonNum) * 100;
                }
                strSB.Append("<td>" + perOf.ToString("0.00") + "%</td>");

                strSB.Append("</tr>");
                j++;
            }

            strSB.Append("<tr style=\"background:#F2F2F2;\"><td style=\"font-weight:bold\">本题有效填写人次</td><td>" + surveyPersonNum + "</td><td>&nbsp;</td></tr>");
            //如果是按分数统计，计算此试题的平均分
            if (isStatByScore)
            {
                int totalScore = BLL.SurveyQuestion.Instance.GetChoiceTotalScoreBySQID(sqId);
                decimal avgScore = 0;
                if (surveyPersonNum > 0)
                {
                    avgScore = (decimal)totalScore / (decimal)surveyPersonNum;
                }
                strSB.Append("<tr style=\"background:#F2F2F2;\"><td style=\"font-weight:bold\">本题平均分</td><td>" + avgScore.ToString("0.00") + "</td><td>&nbsp;</td></tr>");
            }
            strSB.Append("</table>");
            return strSB.ToString();
        }

        //问答题
        private string StatQuestionAnswer(int spiId,int sqId)
        {
            string showHtml = "<div class=\"\" style=\"margin:10px 20px;\"><a href=\"TextQuestionDetail.aspx?SPIID=" + spiId + "&SQID=" + sqId + "\" target=\"_blank\">查看详情</a></div>";
            return showHtml;
        }
        //矩阵单选
        private string StatQuestionForMatrixRadio(int sqId,int spiId)
        {
            StringBuilder strSB = new StringBuilder();
            DataTable dt = BLL.SurveyQuestion.Instance.StatQuestionForMatrixRadio(sqId, spiId);
            strSB.Append("<table  width=\"100%\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            Entities.QuerySurveyOption query = new Entities.QuerySurveyOption();
            query.SQID = sqId;
            int totalCount = 0;
            DataTable dtOption = BLL.SurveyOption.Instance.GetSurveyOption(query,"",1,1000,out totalCount);
            strSB.Append("<thead  style=\"background:#F2F2F2;\">");
            strSB.Append("<th>题目\\选项</th>");
            foreach (DataRow dr in dtOption.Rows)
            {
                strSB.Append("<th>" + dr["OptionName"] + "</th>");
            }
            strSB.Append("</thead>");
            int j = 0;
            int totalNum = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySQID(sqId,spiId);
            foreach (DataRow dr in dt.Rows)
            {
                if (j % 2 == 0)
                {
                    strSB.Append("<tr>");
                }
                else
                {
                    strSB.Append("<tr style='background:#F2F2F2;'>");
                }
                int colCount = dt.Columns.Count;
                for (int i = 0; i < colCount; i++)
                {
                    decimal perOf = 0;
                    if (i > 0)
                    {
                        if (totalNum > 0)
                        {
                            perOf = (decimal.Parse(dr[i].ToString()) / (decimal)totalNum)*100;
                        }
                        strSB.Append("<td>" + perOf.ToString("0.00") + "%</td>");
                    }
                    else
                    {
                        strSB.Append("<td>" + dr[i] + "</td>");
                    }

                }
                strSB.Append("</tr>");
                j++;
            }
            strSB.Append("</table>");
            return strSB.ToString();
        }

        //矩阵下拉
        private string StatQuestionForMatrixDropdown(int sqId, int spiId)
        {
            StringBuilder strSB = new StringBuilder();
            DataTable dt = BLL.SurveyQuestion.Instance.StatQuestionForMatrixDropdown(sqId, spiId);
            strSB.Append("<table  width=\"100%\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
            Entities.QuerySurveyMatrixTitle query = new Entities.QuerySurveyMatrixTitle();
            query.SQID = sqId;
            query.Type = 2;
            int totalCount = 0;
            DataTable dtOption = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query, "", 1, 1000, out totalCount);
            strSB.Append("<thead  style=\"background:#F2F2F2;\">");
            strSB.Append("<th>题目\\选项</th>");
            foreach (DataRow dr in dtOption.Rows)
            {
                strSB.Append("<th>" + dr["TitleName"] + "</th>");
            }
            strSB.Append("<th>列平均</th><th>列小计</th></thead>");
            int j = 0;
            int totalNum = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySQID(sqId,spiId);
            int colCount = dt.Columns.Count;
            decimal[] rolSumArry = new decimal[colCount - 1];
            foreach (DataRow dr in dt.Rows)
            {
                if (j % 2 == 0)
                {
                    strSB.Append("<tr>");
                }
                else
                {
                    strSB.Append("<tr style='background:#F2F2F2;'>");
                }
                
                decimal colSum = 0;
                
                for (int i = 0; i < colCount; i++)
                {
                    if (i > 0)
                    {
                        decimal perOf = 0;
                        if (totalNum > 0)
                        {
                            perOf = decimal.Parse(dr[i].ToString()) / (decimal)totalNum;
                            colSum = colSum + perOf;
                        }
                        strSB.Append("<td>" + perOf.ToString("0.00") + "</td>");
                        for (int r = 0; r < rolSumArry.Length; r++)
                        {
                            if ((i-1) == r)
                            {
                                rolSumArry[r] = rolSumArry[r] + perOf;
                            }
                        }
                    }
                    else
                    {
                        strSB.Append("<td>" + dr[i] + "</td>");
                    }
                }
                decimal colPerOf = colSum / (decimal)(colCount - 1);
                strSB.Append("<td>" + colPerOf.ToString("0.00") + "</td><td>" + colSum.ToString("0.00") + "</td></tr>");
               
                j++;
            }
            //行平均
            strSB.Append("<tr><td>行平均</td>");
            decimal sum = 0;
            for (int i = 0; i < rolSumArry.Length; i++)
            {
                sum = sum + rolSumArry[i];
                decimal rolPerOf = (rolSumArry[i] / (decimal)(dt.Rows.Count));
                strSB.Append("<td>" + rolPerOf.ToString("0.00") + "</td>");
            }
            strSB.Append("<td style='background-color:red;'>" + ((sum / (decimal)(dt.Rows.Count))/ (decimal)(colCount - 1)).ToString("0.00") + "</td><td>N/A</td></tr>");
            //行小计
            strSB.Append("<tr><td>行小计</td>");
            for (int i = 0; i < rolSumArry.Length; i++)
            {
                strSB.Append("<td>" + rolSumArry[i].ToString("0.00") + "</td>");
            }
            strSB.Append("<td>N/A</td><td style='background-color:red;'>" + sum.ToString("0.00") + "</td></tr>");
            strSB.Append("</table>");
            return strSB.ToString();
        }

        private string MatrixRadioOtherStatHtml(int sqId,int spiId)
        {
            StringBuilder strSb = new StringBuilder();
            strSb.Append("<div class=\"jiXh\"><ul id=\"faq\" class=\"clearfix\">");
            Entities.QuerySurveyMatrixTitle query = new Entities.QuerySurveyMatrixTitle();
            query.SQID = sqId;
            int totalCount = 0;
            DataTable dt = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query,"",1,1000,out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSb.Append("<li><dl><dt onclick='OptionStatShowHide(this)'>（" + (i + 1) + "）" + dt.Rows[i]["TitleName"] + "<span title=\"收缩\" style=\"margin-left:50px; color:#CCC8C0\">∧</span></dt>");
                    strSb.Append("<dd><div class=\"left chartC\"><table  width=\"100%\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">");
                    DataTable dtResult = BLL.SurveyMatrixTitle.Instance.StatOptionForMatrixRadio(int.Parse(dt.Rows[i]["SMTID"].ToString()),spiId);
                    int surveyPersonNum = BLL.SurveyAnswer.Instance.GetAnswerUserCountBySQID(sqId,spiId);
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        strSb.Append("<thead  style=\"background:#F2F2F2;\"><th width=\"50%\">选项</th><th width=\"23%\">小计</th><th width=\"27%\">比例</th></thead>");
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            strSb.Append("<tr>");

                            strSb.Append("<td>" + dr["OptionName"].ToString() + "</td>");
                            int num = int.Parse(dr["ExamNum"].ToString());
                            strSb.Append("<td>" + num.ToString() + "</td>");
                            decimal perOf = 0;
                            if (surveyPersonNum > 0)
                            {
                                perOf = ((decimal)num / (decimal)surveyPersonNum) * 100;
                            }
                            strSb.Append("<td>" + perOf.ToString("0.00") + "%</td>");

                            strSb.Append("</tr>");
                        }

                        strSb.Append("<tr style=\"background:#F2F2F2;\"><td style=\"font-weight:bold\">本题有效填写人次</td><td>" + surveyPersonNum + "</td><td>&nbsp;</td></tr></table>");

                    }

                    strSb.Append(" </table></div><div class=\"right chart\"></div></dd></dl></li>");
                }
            }
            strSb.Append("</ul></div>");

            return strSb.ToString();
        }
    }
}