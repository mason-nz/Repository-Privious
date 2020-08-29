using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage
{
    public partial class ExamPaperPreview : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string ExamperName;

        public ExamPaperPageInfo ExamPaperInfo;
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
      
        public string ExamPaperInfoStr
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ExamPaperInfoStr") == null ? string.Empty : HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ExamPaperInfoStr"));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (!string.IsNullOrEmpty(ExamPaperInfoStr))
            {
                ExamPaperInfo = (ExamPaperPageInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(ExamPaperInfoStr, typeof(ExamPaperPageInfo));
                if (ExamPaperInfo != null)
                {
                    //考试名称
                    ExamperName = ExamPaperInfo.ExamPaper.Name;
                    if (ExamPaperInfo.ExamBigQuestioninfoList != null)
                    {
                        repeaterTableList.DataSource = ExamPaperInfo.ExamBigQuestioninfoList;
                        repeaterTableList.DataBind();
                    }
                }
            }
        }
        /// <summary>
        /// 根据小题id取小题name
        /// </summary>
        /// <param name="KLQID"></param>
        /// <returns></returns>
        public string GetSmallquestionName(string KLQID)
        {
            Entities.KLQuestion Model = null;
            Model=BLL.KLQuestion.Instance.GetKLQuestion(Convert.ToInt32(KLQID));
            if (Model != null)
            {
                return Model.Ask;
            }
            else
            {
                return "";
            }
        }

        protected void repeaterRadio_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblAskCategory = e.Item.FindControl("lblKLQID") as Label;
                string KLQID = lblAskCategory.Text.ToString().Trim();
                Repeater repeatersonradio = e.Item.FindControl("repeatersonradio") as Repeater;
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(Convert.ToInt32(KLQID));
                repeatersonradio.DataSource = dt;
                repeatersonradio.DataBind();
            }
        }
        protected void repeaterCheckbox_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblAskCategory = e.Item.FindControl("lblKLQID") as Label;
                string KLQID = lblAskCategory.Text.ToString().Trim();
                Repeater repeatersonCheckbox = e.Item.FindControl("repeatersonCheckbox") as Repeater;
                DataTable dt = BLL.KLAnswerOption.Instance.GetKLAnswerOptionByKLQID(Convert.ToInt32(KLQID));
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
                ExamBigQuestionPageinfo ExamBigQuestioninfo = ExamPaperInfo.ExamBigQuestioninfoList[e.Item.ItemIndex];
                if (ExamBigQuestioninfo!=null&&ExamBigQuestioninfo.shipList != null)
                {
                    if (AskCategory == "1" || AskCategory == "4")
                    {
                        Repeater repeaterRadio = e.Item.FindControl("repeaterRadio") as Repeater;
                        repeaterRadio.Visible = true;
                        repeaterRadio.DataSource = ExamBigQuestioninfo.shipList;
                        repeaterRadio.DataBind();
                    }
                    else if (AskCategory == "2")
                    {
                        Repeater repeaterCheckbox = e.Item.FindControl("repeaterCheckbox") as Repeater;
                        repeaterCheckbox.Visible = true;
                        repeaterCheckbox.DataSource = ExamBigQuestioninfo.shipList;
                        repeaterCheckbox.DataBind();
                    }
                    else if (AskCategory == "3")
                    {
                        Repeater repeaterask = e.Item.FindControl("repeaterask") as Repeater;
                        repeaterask.Visible = true;
                        repeaterask.DataSource = ExamBigQuestioninfo.shipList;
                        repeaterask.DataBind();
                    }
                }
            }
        }
    }
}