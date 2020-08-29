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
    public partial class QuestionView : System.Web.UI.Page
    {
        private string[] LetterArry = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" };
        private string[] YesNoArry = { "错", "对" };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Entities.QueryKLQuestion query = new Entities.QueryKLQuestion();
                query.KLID = 51;
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
                sbOption.Append("<div class='st'>");
                sbOption.Append("<ul>");
                sbOption.Append("<li class='xzt'><label>"+num+"、</label><span>"+ask+"</span>&nbsp;&nbsp;&nbsp;");
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(klqid);
                int i = 0;
                string answerStr = string.Empty;
                switch (askCategory)
                {
                    case 1://单选
                        sbOption.Append("<span class='bold'>问题类型：单选</span>");
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
                            sbOption.Append("<li><input name='' " + checkStr + " type='radio' value='' /><span class='redColor'>" + LetterArry[i] + "、"+dr["answer"]+"</span></li>");
                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                    case 2://多选
                        sbOption.Append("<span class='bold'>问题类型：多选</span>");
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
                            sbOption.Append("<li><input name='' " + checkStr + " type='checkbox' value='' /><span class='redColor'>" + LetterArry[i] + "、" + dr["answer"] + "</span></li>");
                            i++;
                        }
                        sbOption.Append("</ul>");
                        break;
                    case 3://主观题
                        sbOption.Append("<span class='bold'>问题类型：主观题</span>");
                        answerStr = dt.Rows[0]["Answer"].ToString();
                        break;
                    case 4://判断题
                        sbOption.Append("<span class='bold'>问题类型：判断题</span>");
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
                            sbOption.Append("<li><input name='' " + checkStr + " type='radio' value='' /><span class='redColor'>" + YesNoArry[i] + "、" + dr["answer"] + "</span></li>");
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