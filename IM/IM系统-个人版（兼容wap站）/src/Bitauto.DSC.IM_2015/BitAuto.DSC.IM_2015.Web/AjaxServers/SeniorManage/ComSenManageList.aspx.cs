using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    public partial class ComSenManageList : System.Web.UI.Page
    {
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }
        /// <summary>
        /// 常用语
        /// </summary>
        private string RequestCSName
        {
            get { return HttpContext.Current.Request["CSName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CSName"].ToString()); }
        }
        /// <summary>
        /// 标签
        /// </summary>
        private string RequestLTName
        {
            get { return HttpContext.Current.Request["LTName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["LTName"].ToString()); }
        }

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int CurrentUserID = 0;
        public int MinCSID = 0;
        public int MaxCSID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

                CurrentUserID = BLL.Util.GetLoginUserID();
                //right_process = BLL.Util.CheckRight(userID, "SYS024BUT150103");

                BindData();
            }
        }

        private void BindData()
        {
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }
            //PageSize = 1;
            Entities.QueryComSentence query = new Entities.QueryComSentence();

           
            if (RequestCSName != "")
            {
                query.Name = RequestCSName;
            }
            if (RequestLTName != "")
            {
                query.LTName = RequestLTName;
            }
            query.DataRight = BLL.BaseData.Instance.GetAgentRegionByUserID(BLL.Util.GetLoginUserID().ToString());

            int RecordCount = 0;

            DataTable dt = BLL.ComSentence.Instance.GetComSentenceList(query, "cs.SortNum asc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);//BLL.OtherTaskInfo.Instance.GetOtherTaskInfoByList(query, "OtherTaskInfo.LastOptTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);

            if (dt.Rows.Count > 0)
            {
                MinCSID = CommonFunc.ObjectToInteger(dt.Rows[0]["CSID"]);
                MaxCSID = CommonFunc.ObjectToInteger(dt.Rows[dt.Rows.Count - 1]["CSID"]);
            }

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        
        }
    }
}