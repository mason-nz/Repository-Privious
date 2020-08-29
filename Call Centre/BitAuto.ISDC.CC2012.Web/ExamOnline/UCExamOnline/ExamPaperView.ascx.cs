using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.UCExamOnline
{
    public partial class ExamPaperView : System.Web.UI.UserControl
    {
        //试卷混合实体
        private Entities.ExamPaperInfo _exampaperinfo;
        //大题ID 字符串
        public string ExamPaperID = string.Empty;
        public Entities.ExamPaperInfo ExamPaperInfo
        {
            set
            {
                _exampaperinfo = value;
            }
            get
            {
                return _exampaperinfo;
            }
        }
        //在线考试id
        private long eolid = 0;
        public long EOLID
        {
            get
            {
                return eolid;
            }
            set
            {
                eolid = value;
            }
        }
        public string GetAskCategory(string category)
        {
            if (category == "1")
            {
                return "单选题：";
            }
            else if (category == "2")
            {
                return "复选题：";
            }
            else if (category == "3")
            {
                return "主观题：";
            }
            return "判断题：";
        }
        public string ConvertStrForNeed(string strold, string need)
        {
            if (need == "-")
            {
                if (strold == "1")
                {
                    return "一、";
                }
                else if (strold == "2")
                {
                    return "二、";
                }
                else if (strold == "3")
                {
                    return "三、";
                }
                else if (strold == "4")
                {
                    return "四、";
                }
                else if (strold == "5")
                {
                    return "五、";
                }
                else if (strold == "6")
                {
                    return "六、";
                }
                else if (strold == "7")
                {
                    return "七、";
                }
                else if (strold == "8")
                {
                    return "八、";
                }
                else if (strold == "9")
                {
                    return "九、";
                }
                else if (strold == "10")
                {
                    return "十、";
                }
            }
            else if (need == "a")
            {
                if (strold == "1")
                {
                    return "A、";
                }
                else if (strold == "2")
                {
                    return "B、";
                }
                else if (strold == "3")
                {
                    return "C、";
                }
                else if (strold == "4")
                {
                    return "D、";
                }
                else if (strold == "5")
                {
                    return "E、";
                }
                else if (strold == "6")
                {
                    return "F、";
                }
                else if (strold == "7")
                {
                    return "G、";
                }
                else if (strold == "8")
                {
                    return "H、";
                }
                else if (strold == "9")
                {
                    return "J、";
                }
                else if (strold == "10")
                {
                    return "K、";
                }
            }

            return strold + "、";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ExamPaperInfo != null)
            {
                ExamPaperID = ExamPaperInfo.ExamPaper.EPID.ToString();
                if (ExamPaperInfo.ExamBigQuestioninfoList != null)
                {
                    repeaterTableList.DataSource = ExamPaperInfo.ExamBigQuestioninfoList;
                    repeaterTableList.DataBind();
                }
            }
        }

        protected void repeaterRadio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string KLQID = DataBinder.Eval(e.Item.DataItem, "KLQID").ToString().Trim();
                string BQID = DataBinder.Eval(e.Item.DataItem, "BQID").ToString().Trim();
                Repeater repeatersonradio = e.Item.FindControl("repeatersonradio") as Repeater;
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(Convert.ToInt32(KLQID), BQID);
                repeatersonradio.DataSource = dt;
                repeatersonradio.DataBind();
            }
        }
        protected void repeaterCheckbox_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string KLQID = DataBinder.Eval(e.Item.DataItem, "KLQID").ToString().Trim();
                string BQID = DataBinder.Eval(e.Item.DataItem, "BQID").ToString().Trim();
                Repeater repeatersonCheckbox = e.Item.FindControl("repeatersonCheckbox") as Repeater;
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(Convert.ToInt32(KLQID), BQID);
                repeatersonCheckbox.DataSource = dt;
                repeatersonCheckbox.DataBind();
            }
        }
        protected void repeaterTableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取试题类型，如果是客观题显示客观题，如果是主观题显示主观题
                Label lblAskCategory = e.Item.FindControl("lblAskCategory") as Label;
                string AskCategory = lblAskCategory.Text.Trim();

                Label lblBQID = e.Item.FindControl("lblBQID") as Label;
                string BQID = lblBQID.Text.Trim();

                DataTable dtquestion = null;
                dtquestion = BLL.ExamBSQuestionShip.Instance.GetKLQuestionData(Convert.ToInt32(BQID));


                if (AskCategory == "1" || AskCategory == "4")
                {
                    Repeater repeaterRadio = e.Item.FindControl("repeaterRadio") as Repeater;
                    repeaterRadio.Visible = true;
                    repeaterRadio.DataSource = dtquestion;
                    repeaterRadio.DataBind();
                }
                else if (AskCategory == "2")
                {
                    Repeater repeaterCheckbox = e.Item.FindControl("repeaterCheckbox") as Repeater;
                    repeaterCheckbox.Visible = true;
                    repeaterCheckbox.DataSource = dtquestion;
                    repeaterCheckbox.DataBind();
                }
                else if (AskCategory == "3")
                {
                    Repeater repeaterask = e.Item.FindControl("repeaterask") as Repeater;
                    repeaterask.Visible = true;
                    repeaterask.DataSource = dtquestion;
                    repeaterask.DataBind();
                }

            }
        }
        /// <summary>
        /// 取答案
        /// </summary>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string Getanswer(string KLQID, string BQID)
        {
            string anwers = "";
            if (EOLID != 0)
            {
                anwers = BLL.ExamOnlineAnswer.Instance.GetSelected(EOLID.ToString(), BQID, KLQID);
            }
            return anwers;
        }


        //是否选择
        public string GetChecked(string KLQID, string BQID, string selectitem)
        {
            string flag = "";
            if (EOLID != 0)
            {
                string answers = BLL.ExamOnlineAnswer.Instance.GetSelected(EOLID.ToString(), BQID, KLQID);
                if (answers != "")
                {
                    if (answers.IndexOf(',') > 0)
                    {
                        for (int i = 0; i < answers.Split(',').Length; i++)
                        {
                            string selected = answers.Split(',')[i];
                            if (selected == selectitem)
                            {
                                flag = "checked";
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (answers == selectitem)
                        {
                            flag = "checked";
                        }
                    }
                }
            }
            return flag;
        }
    }
}