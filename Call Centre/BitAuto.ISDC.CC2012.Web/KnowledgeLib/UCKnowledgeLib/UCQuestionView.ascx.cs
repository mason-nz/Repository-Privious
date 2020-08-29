using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib
{
    public partial class UCQuestionView : System.Web.UI.UserControl
    {
        public int KLID;
        public UCQuestionView()
        {
        }
        public UCQuestionView(int klId)
        {
            this.KLID = klId;
        }

        private string[] LetterArry = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" };
        private string[] YesNoArry = { "错", "对" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
                query.KLID = this.KLID;
                int totalCount = 0;
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
                int num = e.Item.ItemIndex + 1;
                string ask = DataBinder.Eval(e.Item.DataItem, "Ask").ToString();
                StringBuilder sbOption = new StringBuilder();
                sbOption.Append("<a name=" + klqid + "><ul name='ulQuestion' id='ulQuestion" + num + "'>");
                sbOption.Append("<li class='xzt'></a><label>" + num + "、</label><span class='xzbt'>" + ask + "</span>&nbsp;&nbsp;&nbsp;");
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(klqid);
                int i = 0;
                string answerStr = string.Empty;
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
                                sbOption.Append("<li><input name='' " + checkStr + " type='radio' disabled='disabled' value='' /><span class='redColor'>" + LetterArry[i] + "、" + dr["answer"] + "</span></li>");
                            }
                            else
                            {
                                sbOption.Append("<li><input name='' " + checkStr + " type='radio' disabled='disabled' value='' /><span>" + LetterArry[i] + "、" + dr["answer"] + "</span></li>");
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
                                sbOption.Append("<li><input name='' " + checkStr + " disabled='disabled' type='checkbox' value='' /><span class='redColor'>" + LetterArry[i] + "、" + dr["answer"] + "</span></li>");
                            }
                            else
                            {
                                sbOption.Append("<li><input name='' " + checkStr + " disabled='disabled' type='checkbox' value='' /><span>" + LetterArry[i] + "、" + dr["answer"] + "</span></li>");
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
                                sbOption.Append("<li><input name='' " + checkStr + " disabled='disabled' type='radio' value='' /><span class='redColor'>" + dr["answer"] + "</span></li>");
                            }
                            else
                            {
                                sbOption.Append("<li><input name='' " + checkStr + " disabled='disabled' type='radio' value='' /><span>" + dr["answer"] + "</span></li>");
                            }

                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                }
                if (askCategory != 3)
                {
                    answerStr = answerStr.Substring(0, answerStr.Length - 1);
                }
                sbOption.Append("<div class='title bold conrect'>正确答案：<span>" + answerStr + "</span></div>");
                sbOption.Append("</ul>");

                Literal lOption = e.Item.FindControl("LOption") as Literal;
                lOption.Text = sbOption.ToString();

            }
        }
    }
}