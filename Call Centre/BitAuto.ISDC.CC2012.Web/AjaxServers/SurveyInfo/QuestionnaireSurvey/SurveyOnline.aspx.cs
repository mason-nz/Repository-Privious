using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.QuestionnaireSurvey
{
    public partial class SurveyOnline : PageBase
    {
        #region 属性

        public string RequestCategory
        {
            get { return HttpContext.Current.Request["Category"] == null ? "" : HttpContext.Current.Request["Category"].ToString(); }
        }
        public string RequestName
        {
            get { return HttpContext.Current.Request["Name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Name"].ToString()); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3003"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }
        public void BindData()
        {
            Entities.QuerySurveyProjectInfo query = new Entities.QuerySurveyProjectInfo();
            if (RequestCategory != "")
            {
                query.SCIDStr = RequestCategory;
            }
            if (RequestName != "")
            {
                query.Name = RequestName;
            }

            query.StatusStr = "1,2";//状态包括1-进行中；2-已结束；
            query.CreaterType = 1;
            query.UserID = userID;//将当前登录者赋值给UserID

            DataTable dt = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, "spi.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            hidSurveyAll.Value = RecordCount.ToString();  //全部

            query.StatusStr = "1";
            //query.SurveyEndTime = DateTime.Now;  //正在进行中
            int count;
            DataTable dt_surveying = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(query, " spi.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out count);
            hidSurveying.Value = count.ToString();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

        }

        protected void repeaterTableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
        }

        //判断是否已提交过或时间是否已过，已提交或时间已过则返回false，用在前台控制 字体不加粗
        public bool surveyIsSubmit(string spiid)
        {
            bool result = true;

            int _spiid;
            if (int.TryParse(spiid, out  _spiid))
            {
                //判断是否已提交过
                Entities.QuerySurveyAnswer query = new Entities.QuerySurveyAnswer();
                query.CreateUserID = userID;
                query.SPIID = _spiid;
                int count;

                DataTable dt = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query, "", 1, 10000, out count);
                if (dt.Rows.Count > 0)
                {
                    result = false;
                }
                else
                {
                    //判断时间是否已过
                    Entities.SurveyProjectInfo query_projectInfo = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(_spiid);
                    if (query_projectInfo != null)
                    {
                        if (query_projectInfo.SurveyEndTime < DateTime.Now)
                        {
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        //显示按钮
        public string showOperBtn(string spiid)
        {
            string result = string.Empty;

            int _spiid;
            if (int.TryParse(spiid, out _spiid))
            {
                Entities.SurveyProjectInfo model = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(_spiid);
                if (model != null)
                {
                    if (model.SurveyStartTime <= DateTime.Now && model.SurveyEndTime >= DateTime.Now)//时间没过，显示按钮
                    {
                        //判断是否已提交过，已提交则不显示按钮 返回false，且字体不需要加粗
                        Entities.QuerySurveyAnswer query = new Entities.QuerySurveyAnswer();
                        query.CreateUserID = userID;
                        query.SPIID = _spiid;
                        int count;

                        DataTable dt = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query, "", 1, 10000, out count);

                        //没提交过，显示进入调查按钮
                        if (dt.Rows.Count == 0)
                        {
                            result = "<span class='right btnsearch'><input name='' type='button'  value='进入调查' onclick='openpage(" + _spiid + ")'/></span>";
                        }

                    }
                }
            }

            return result;
        }

        //标题字数控制
        public string getTitle(string title)
        {
            string Title = string.Empty;

            if (title.Length > 40)
            {
                Title = title.Substring(0, 40) + "......";
            }
            else
            {
                Title = title;
            }

            return Title;
        }

        //内容字数控制
        public string getContent(string content)
        {
            string Content = string.Empty;

            if (content.Length > 200)
            {
                Content = content.Substring(0, 200) + "......";
            }
            else
            {
                Content = content;
            }

            return Content;
        }
    }
}