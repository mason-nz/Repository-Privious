using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCQuestionEdit : System.Web.UI.UserControl
    {
        private int KLID;
        private string[] LetterArry = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private string[] YesNoArry = { "错", "对" };

        public UCQuestionEdit()
        {
        }
        public UCQuestionEdit(int klId)
        {
            this.KLID = klId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (BLL.KnowledgeLib.Instance.IsExistQuestion(KLID))
                {
                    Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
                    query.KLID = KLID;
                    int totalCount = 0;
                    DataTable dt = BLL.KLQuestion.Instance.GetKLQuestion(query, "", 1, 1000, out totalCount);
                    rptQuestion.DataSource = dt;
                    rptQuestion.DataBind();
                }
            }
        }

        protected void rptQuestion_ItemDataBind(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int klqid = int.Parse(DataBinder.Eval(e.Item.DataItem, "KLQID").ToString().Trim());
                if (klqid > 0)
                {
                    bool isUsed = false;
                    string ulStyle = "";
                    isUsed = BLL.KLQuestion.Instance.IsUsed(klqid);
                    if (isUsed)
                    {
                        ulStyle = "disabled=disabled";
                    }
                    int askCategory = int.Parse(DataBinder.Eval(e.Item.DataItem, "AskCategory").ToString().Trim());
                    int num = e.Item.ItemIndex + 1;
                    string ask = DataBinder.Eval(e.Item.DataItem, "Ask").ToString();
                    StringBuilder sbOption = new StringBuilder();

                    sbOption.Append("<ul name='ulQuestion' id='ulQuestion" + num + "'>");
                    sbOption.Append("<li class='xzt'><label name='QuestionOrder'>" + num + "、</label><span><textarea  style='width:590px; height:50px;' name='questionAsk' type='text' class='w600'>" + ask + "</textarea></span><input type='hidden' name='hdnAskCategory' value=" + askCategory + "><input type='hidden' name='hdnQuestionID' value='" + klqid + "'>");
                    if (!isUsed)
                    {
                        sbOption.Append("<span class='addst2'><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteQuestion(" + num + ")'></a></span>");
                    }
                    DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(klqid);
                    int i = 0;
                    string answerStr = string.Empty;
                    string answerOptionIndex = string.Empty;
                    switch (askCategory)
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
                                    answerStr += LetterArry[i] + "、";
                                    answerOptionIndex += i + ",";
                                }
                                if (i < dt.Rows.Count - 1)
                                {
                                    sbOption.Append("<li><input name='liOption" + num + "' " + ulStyle + " type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "'  class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></li>");
                                }
                                else
                                {
                                    sbOption.Append("<li><input name='liOption" + num + "' " + ulStyle + " type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text' value='" + dr["Answer"] + "' class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'>");
                                    if (!isUsed)
                                    {
                                        sbOption.Append("<span class='addst3' ><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this)'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span></li>");
                                    }
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
                                    answerOptionIndex += i + ",";
                                }
                                if (i < dt.Rows.Count - 1)
                                {
                                    sbOption.Append("<li><input name='liOption" + num + "' " + ulStyle + " type='checkbox' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text'  value='" + dr["Answer"] + "'  class='w550'/></span><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></li>");
                                }
                                else
                                {
                                    sbOption.Append("<li><input name='liOption" + num + "' " + ulStyle + " type='checkbox' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + LetterArry[i] + "、</em><span><input name='txtOption' type='text'  value='" + dr["Answer"] + "'  class='w550'/><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></span>");
                                    if (!isUsed)
                                    {
                                        sbOption.Append("<span class='addst3' ><span><a href='javascript:void(0)' class='add' onclick='javascript:uCEditQuestionHelper.AddOption(this)'></a></span><span><a href='javascript:void(0)' class='delete' onclick='javascript:uCEditQuestionHelper.DeleteOption(this," + num + ")'></a></span></span></li>");
                                    }
                                }
                                i++;
                            }
                            sbOption.Append("</ul>");
                            break;
                        case 3://主观题
                            foreach (DataRow dr in dt.Rows)
                            {
                                answerStr = dr["Answer"].ToString();
                                sbOption.Append("<input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'>");
                            }
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
                                    answerOptionIndex += i + ",";
                                }
                                sbOption.Append("<li><input name='liOption" + num + "' " + ulStyle + " type='radio' " + checkStr + " value='' onclick='javascript:uCEditQuestionHelper.SelectOption(" + num + ")'/><em class='dx'>" + YesNoArry[i] + "、</em><input name='hdnOptionID' type='hidden' value='" + dr["KLAOID"] + "'></li>");
                                i++;
                            }
                            sbOption.Append("</ul>");
                            break;
                    }
                    if (askCategory == 3)
                    {
                        sbOption.Append("<div name='divAnswer' class='title bold conrect'>正确答案：<span><textarea id='AnswerOptionText" + num + "'  type='text'  cols='' rows=''  class='w500'>" + answerStr + "</textarea></span></div><input type='hidden' name='hdnAnswerOptionIndex' value=''></li>");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(answerStr))
                        {
                            answerStr = answerStr.Substring(0, answerStr.Length - 1);
                        }
                        if (!string.IsNullOrEmpty(answerOptionIndex))
                        {
                            answerOptionIndex = answerOptionIndex.Substring(0, answerOptionIndex.Length - 1);
                        }
                        sbOption.Append("<div name='divAnswer' class='title bold conrect'>正确答案：<span><input id='AnswerOptionText" + num + "' type='text' value='" + answerStr + "' disabled='disabled'   class='w90'/></span></div><input type='hidden' name='hdnAnswerOptionIndex' value='" + answerOptionIndex + "'></li>");
                    }

                    sbOption.Append("</ul>");

                    Literal lOption = e.Item.FindControl("LOption") as Literal;
                    lOption.Text = sbOption.ToString();
                }
            }
        }


    }
}