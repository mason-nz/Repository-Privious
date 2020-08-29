using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamPaperStorage
{
    public partial class QuestionSelect : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        /// <summary>
        /// 小题IDs
        /// </summary>
        public string SmallQIDs
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["SmallQIDs"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["SmallQIDs"]);
            }
        }

        /// <summary>
        /// 试题类型
        /// </summary>
        public string QustionType
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["QustionType"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QustionType"]);
            }
        }

        /// <summary>
        /// 试题名称
        /// </summary>
        public string QustionName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["QustionName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QustionName"]);
            }
        }

        /// <summary>
        /// 知识点分类
        /// </summary>
        public string KCID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["KCID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["KCID"]);
            }
        }
        
        /// <summary>
        /// 最大题量
        /// </summary>
        public string MaxCount
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MaxCount"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MaxCount"]);
            }
        }

        /// <summary>
        /// 已选择题数
        /// </summary>
        public string SelCount
        {
            get;
            set;
        }
         

        #endregion
        public int GroupLength = 5;
        public int PageSize = 10;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                SelCount = "0";
                DataBindData();
                DataBindsByIDs();
            }
            
            var a = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID( BLL.Util.GetLoginUserID());
            if (a.RegionID.HasValue)
            {
                RegionID = a.RegionID.Value;
            }
        }
        public int RegionID = -2;
        private void DataBindData()
        {
            int totalCount = 0;

            DataTable dt = BLL.KLQuestion.Instance.GetQuestionByIDs(KCID, QustionName, "", QustionType, "CreateTime desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                rptQuestionList.DataSource = dt;
                rptQuestionList.DataBind();
            }
            litPagerDown.Text = PageCommon.Instance.LinkStringByPost(GetWhere(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 1);

        }

        private void DataBindsByIDs()
        {
            if (!string.IsNullOrEmpty(SmallQIDs))
            {
                int totalCount = 0;

                DataTable dt = BLL.KLQuestion.Instance.GetQuestionByIDs("", "", SmallQIDs, QustionType, "CreateTime desc", PageCommon.Instance.PageIndex, PageSize, out totalCount);
                if (dt != null && dt.Rows.Count > 0)
                {
                    rptIDsList.DataSource = dt;
                    rptIDsList.DataBind();
                    this.SelCount = dt.Rows.Count.ToString();
                }
                else
                {
                    this.SelCount = "0";
                }
                
            }
        }

        private string GetWhere()
        {
            string where = "";
            string query = Request.Url.Query;

            if ((!SmallQIDs.Equals("")) || SmallQIDs != null)
            {
                where += "&SmallQIDs=" + BLL.Util.EscapeString(SmallQIDs);
            }
            if ((!QustionType.Equals("")) || QustionType != null)
            {
                where += "&QustionType=" + BLL.Util.EscapeString(QustionType);
            }
            if ((!QustionName.Equals("")) || QustionName != null)
            {
                where += "&QustionName=" + BLL.Util.EscapeString(QustionName);
            }
            if ((!KCID.Equals("")) || KCID != null)
            {
                where += "&KCID=" + BLL.Util.EscapeString(KCID);
            }

            where += "&random=" + (new Random()).Next().ToString();
            return where;
        }


        public string GetTypeNameByID(string typeid)
        {
            switch (typeid)
            {
                case "1": return "单选题";
                case "2": return "复选题";
                case "3": return "主观题";
                case "4": return "判断题";
                default: return "";
            }
        }
    }
}