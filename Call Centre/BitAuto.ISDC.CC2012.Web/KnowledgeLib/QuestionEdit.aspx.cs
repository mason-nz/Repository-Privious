using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class QuestionEdit : System.Web.UI.Page
    {
        private string[] LetterArry={"A","B","C","D","E","F","G","H","I","J","K","L","M"};
        private string[] YesNoArry = { "错","对"};

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
                query.KLID = 51;
                int totalCount=0;
                DataTable dt = BLL.KLQuestion.Instance.GetKLQuestion(query, "", 1, 1000, out totalCount);
                rptQuestion.DataSource = dt;
                rptQuestion.DataBind();
            }
        }

        protected void rptQuestion_ItemDataBind(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int klqid = int.Parse(DataBinder.Eval(e.Item.DataItem, "KLQID").ToString().Trim());
                int askCategory = int.Parse(DataBinder.Eval(e.Item.DataItem, "AskCategory").ToString().Trim());
                int num = e.Item.ItemIndex+1;
                string ask=DataBinder.Eval(e.Item.DataItem,"Ask").ToString();
                StringBuilder sbOption = new StringBuilder();
                sbOption.Append("<ul name='ulQuestion' id='ulQuestion" + num + "'>");
                sbOption.Append("<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><input name='questionAsk' type='text' value='" + ask + "'  class='w600'/></span><input type='hidden' name='hdnAskCategory' value=1><input type='hidden' name='hdnQuestionID' value='"+klqid+"'>");
                sbOption.Append("<span><select  name='sltQuestionType' class='w100' ><option value='radio'>单选题</option><option value='checkbox'>复选题</option><option value='text'>主观题</option><option value='select'>判断题</option></select></span>");
                sbOption.Append("<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddQuestionSelectType(this)'></a></span>");
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(klqid);
                int i = 0;
                string answerStr = string.Empty;
                switch(askCategory)
                {
                    case 1://单选
                        sbOption.Append("<ul>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            string checkStr = string.Empty;
                            Entities.KLQAnswer answerInfo = BLL.KLQAnswer.Instance.GetKLQAnswer(klqid, int.Parse(dr["KLAOID"].ToString()));
                            if (answerInfo != null)
                            {
                                checkStr = "checked=true";
                                answerStr += LetterArry[i]+"、";
                            }
                            if (i < dt.Rows.Count - 1)
                            {
                                sbOption.Append("<li><input name='liOption" + num + "' type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "'  class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></li>");
                            }
                            else
                            {
                                sbOption.Append("<li><input name='liOption" + num + "' type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "'  class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'>");
                                sbOption.Append("<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddOption(this)'></a></span></li>");
                            }
                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                    case 2://多选
                        sbOption.Append("<ul>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            string checkStr = string.Empty;
                            Entities.KLQAnswer answerInfo = BLL.KLQAnswer.Instance.GetKLQAnswer(klqid, int.Parse(dr["KLAOID"].ToString()));
                            if (answerInfo != null)
                            {
                                checkStr = "checked=true";
                                answerStr += LetterArry[i] + "、";
                            }
                            if (i < dt.Rows.Count - 1)
                            {
                                sbOption.Append("<li><input name='liOption" + num + "' type='checkbox' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "'  class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></li>");
                            }
                            else
                            {
                                sbOption.Append("<li><input name='liOption" + num + "' type='checkbox' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "'  class='w550'/><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></span>");
                                sbOption.Append("<span class='delete'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span>&nbsp;<span class='add'><a href='javascript:void(0)' onclick='javascript:uCEditQuestionHelper.AddOption(this)'></a></span></li>");
                            }
                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                    case 3://主观题
                        answerStr = dt.Rows[0]["Answer"].ToString();
                        break;
                    case 4://判断题
                        sbOption.Append("<ul>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            string checkStr = string.Empty;
                            Entities.KLQAnswer answerInfo = BLL.KLQAnswer.Instance.GetKLQAnswer(klqid, int.Parse(dr["KLAOID"].ToString()));
                           
                            if (answerInfo != null)
                            {
                                checkStr = "checked=true";
                                answerStr += YesNoArry[i] + "、";
                            }
                            sbOption.Append("<li><input name='liOption" + num + "' type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + YesNoArry[i] + "、</em></li>");
                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                }
                if (askCategory == 3)
                {
                    sbOption.Append("<div name='divAnswer' class='title bold conrect'>正确答案：<span><textarea id='AnswerOptionText" + num + "' type='text' value='" + answerStr + "' cols='' rows=''  class='w500'></textarea></span></div><input type='hidden' name='hdnAnswerOptionIndex'></li>");
                }
                else
                {
                    answerStr = answerStr.Substring(0, answerStr.Length - 1);
                    sbOption.Append("<div name='divAnswer' class='title bold conrect'>正确答案：<span><input id='AnswerOptionText" + num + "' type='text' value='" + answerStr + "' disabled='disabled'   class='w90'/></span></div><input type='hidden' name='hdnAnswerOptionIndex'></li>");
                }
                
                sbOption.Append("</ul>");

                Literal lOption = e.Item.FindControl("LOption") as Literal;
                lOption.Text = sbOption.ToString();
                
            }
        }

    }
}