using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamScoreManagement
{
    public partial class MarkExamPaper : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string ViewType = "阅卷";
        /// <summary>
        /// 查看涞源
        /// </summary>
        public string come
        {
            get { return Request["come"] == null ? "" : BLL.Util.DecryptString(Request["come"].ToString()); }
        }
        public string IsMarking = "";
        public string username = "";



        public string ExamPersonID
        {
            get
            {
                if (come == "1")
                {
                    return BLL.Util.GetLoginUserID().ToString();
                }
                else
                {
                    return Request["ExamPersonID"] == null ? "0" : BLL.Util.DecryptString(Request["ExamPersonID"].ToString());
                }
            }
        }


        //考试项目id
        public string Type
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["type"]))
                {
                    return BLL.Util.DecryptString(Request["type"]);
                }
                else
                {
                    return "";
                }
            }
        }
        private string eiid = string.Empty;
        //考试项目id
        public string RequestEIID
        {
            get
            {
                if (!string.IsNullOrEmpty(eiid))
                {
                    return eiid;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                eiid = value;
            }
        }
        //考卷id
        public string RequestEPID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["epid"]))
                {
                    return BLL.Util.DecryptString(Request["epid"]);
                }
                else
                {
                    return "";
                }
            }
        }
        //在线考试ＩＤ
        public string EOLID;
        public string SumScore;
        public string ExamperName;

        public string GetEOLID()
        {
            string EIID = "";
            string isMakeup = "";
            if (!string.IsNullOrEmpty(RequestEIID))
            {
                EIID = RequestEIID;
            }
            if (!string.IsNullOrEmpty(Type))
            {
                isMakeup = Type;
            }

            return BLL.ExamOnlineAnswer.Instance.GetEOLID(EIID, Type, ExamPersonID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3004"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                RequestEIID = BLL.Util.DecryptString(Request["eiid"]);
                if (come == "1" && Type == "1")
                {
                    RequestEIID = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfo(Convert.ToInt32(BLL.Util.DecryptString(Request["eiid"]))).EIID.ToString();
                }

                //come为1是查看个人成绩，come为2是管理员，质检员，质检经理查看成绩，come为3是阅卷
                username = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(ExamPersonID));

                //取是否已阅

                SumScore = GetSumScore(out IsMarking);
                EOLID = GetEOLID();
                fenshu.Visible = true;
                if (come == "1")
                {
                    ViewType = "成绩查看";
                    //ExamPersonID = BLL.Util.GetLoginUserID().ToString();
                    username = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(BLL.Util.GetLoginUserID());
                    //fenshu.Visible = true;
                }
                else if (come == "2")
                {
                    ViewType = "成绩查看";
                    //fenshu.Visible = true;
                }
                else if (come == "3" && IsMarking != "1")
                {
                    ViewType = "阅卷";
                    //fenshu.Visible = false;
                }
                else if (come == "3" && IsMarking == "1")
                {
                    ViewType = "成绩查看";
                    //fenshu.Visible = true;
                }
                if (!string.IsNullOrEmpty(RequestEPID))
                {
                    Entities.ExamPaper model = BLL.ExamPaper.Instance.GetExamPaper(Convert.ToInt32(RequestEPID));
                    ExamperName = model.Name;
                    Entities.ExamPaperInfo ExamPaperInfo = BLL.ExamPaper.Instance.GetExamPaperInfo(Convert.ToInt32(RequestEPID));
                    if (ExamPaperInfo.ExamBigQuestioninfoList != null)
                    {
                        repeaterTableList.DataSource = ExamPaperInfo.ExamBigQuestioninfoList;
                        repeaterTableList.DataBind();
                    }
                }
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
            else if (need == "A")
            {
                if (strold == "1")
                {
                    return "A";
                }
                else if (strold == "2")
                {
                    return "B";
                }
                else if (strold == "3")
                {
                    return "C";
                }
                else if (strold == "4")
                {
                    return "D";
                }
                else if (strold == "5")
                {
                    return "E";
                }
                else if (strold == "6")
                {
                    return "F";
                }
                else if (strold == "7")
                {
                    return "G";
                }
                else if (strold == "8")
                {
                    return "H";
                }
                else if (strold == "9")
                {
                    return "J";
                }
                else if (strold == "10")
                {
                    return "K";
                }
            }

            return strold + "、";

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
        /// 取用户小题得分
        /// </summary>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string Getfenshu(string BQID, string KLQID)
        {
            string EIID = "";
            string isMakeup = "";
            string personid = "";
            if (!string.IsNullOrEmpty(RequestEIID))
            {
                EIID = RequestEIID;
            }
            if (!string.IsNullOrEmpty(Type))
            {
                isMakeup = Type;
            }
            if (come == "1")
            {
                personid = BLL.Util.GetLoginUserID().ToString();
            }
            else
            {
                personid = ExamPersonID;
            }

            return BLL.ExamOnlineAnswer.Instance.Getfenshu(EIID, Type, personid, BQID, KLQID);
        }

        /// <summary>
        /// 取用户选择的答案
        /// </summary>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string GetSelectedByID(string BQID, string KLQID)
        {
            string EIID = "";
            string isMakeup = "";
            string personid = "";
            if (!string.IsNullOrEmpty(RequestEIID))
            {
                EIID = RequestEIID;
            }
            if (!string.IsNullOrEmpty(Type))
            {
                isMakeup = Type;
            }
            if (come == "1")
            {
                personid = BLL.Util.GetLoginUserID().ToString();
            }
            else
            {
                personid = ExamPersonID;
            }

            return BLL.ExamOnlineAnswer.Instance.GetSelected(EIID, Type, personid, BQID, KLQID);
        }
        /// <summary>
        /// 小题正确答案
        /// </summary>
        /// <param name="BQID"></param>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string GetRightByID(string KLQID)
        {
            DataTable dt = BLL.KLQAnswer.Instance.GetKLQAnswerByKLQID(Convert.ToInt32(KLQID));
            string sqlstr = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlstr += dt.Rows[i]["KLAOID"].ToString() + ",";
                }
                sqlstr = sqlstr.Substring(0, sqlstr.Length - 1);
            }
            return sqlstr;
        }
        /// <summary>
        /// 问答题答案
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string GetRightByIDWenda(string KLQID)
        {
            DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(Convert.ToInt32(KLQID));
            string sqlstr = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                sqlstr = dt.Rows[0]["Answer"].ToString();
            }
            return sqlstr;
        }


        /// <summary>
        /// 取考生总成绩
        /// </summary>
        /// <returns></returns>
        public string GetSumScore(out string Marking)
        {
            string EIID = "";
            string isMakeup = "";
            string personid = "";
            if (!string.IsNullOrEmpty(RequestEIID))
            {
                EIID = RequestEIID;
            }
            if (!string.IsNullOrEmpty(Type))
            {
                isMakeup = Type;
            }
            if (come == "1")
            {
                personid = BLL.Util.GetLoginUserID().ToString();
            }
            else
            {
                personid = ExamPersonID;
            }

            return BLL.ExamOnlineAnswer.Instance.GetSumScore(EIID, Type, personid, out Marking);
        }
        public string GetMaxFen(string BQID)
        {
            string maxfen = "0";
            maxfen = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(Convert.ToInt32(BQID)).EachQuestionScore.ToString();
            return maxfen;

        }
    }
}