using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class SelectSingleProjectPop :PageBase
    {
        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }

        private string RequestGroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 5;
        public int RecordCount;
        private int userID = 0;
        public int GroupLength = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            userID = BLL.Util.GetLoginUserID();
            BindData();
        }
        //绑定数据
        private void BindData()
        {
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();
             
            if (!string.IsNullOrEmpty(RequestName))
            {
                query.Name = StringHelper.SqlFilter(RequestName);
            }

            if (RequestGroup != "")
            {
                query.BGID = int.Parse(RequestGroup);
            }
            query.wherePlus = " and ProjectInfo.status>0 ";
                //" and ProjectInfo.status>0  AND NOT EXISTS(SELECT 1 FROM AutoCall_ProjectInfo bb WHERE ProjectInfo.ProjectID=bb.projectid ) ";
   
            DataTable dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, " ProjectInfo.CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, userID);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 100);
        }
    }
}