using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using System.Collections;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyMapping
{
    public partial class SurveyMappingList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestSName
        {
            get { return HttpContext.Current.Request["SName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SName"].ToString()); }
        }
        private string RequestSBGID
        {
            get { return HttpContext.Current.Request["SBGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SBGID"].ToString()); }
        }
        private string RequestSSCID
        {
            get { return HttpContext.Current.Request["SSCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SSCID"].ToString()); }
        }
        private string RequestPName
        {
            get { return HttpContext.Current.Request["PName"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PName"].ToString()); }
        }
        private string RequestPBGID
        {
            get { return HttpContext.Current.Request["PBGID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PBGID"].ToString()); }
        }
        private string RequestPSCID
        {
            get { return HttpContext.Current.Request["PSCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PSCID"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT5010"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                BindData();
            }
        }

        private void BindData()
        {
            Entities.QueryProjectSurveyMapping query = new QueryProjectSurveyMapping();
            if (RequestSName != "")
            {
                query.SName = StringHelper.SqlFilter(RequestSName);
            }
            if (RequestSBGID != "" && RequestSBGID != "-1")
            {
                query.SBGID = int.Parse(RequestSBGID);
            }
            if (RequestSSCID != "" && RequestSSCID != "-1")
            {
                query.SSCID = int.Parse(RequestSSCID);
            }
            if (RequestPName != "")
            {
                query.PName = StringHelper.SqlFilter(RequestPName);
            }
            if (RequestPBGID != "" && RequestPBGID != "-1")
            {
                query.PBGID = int.Parse(RequestPBGID);
            }
            if (RequestPSCID != "" && RequestPSCID != "-1")
            {
                query.PSCID = int.Parse(RequestPSCID);
            }
            query.LoginID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMappingForList(query, " psm.CreateTime DESC ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="siid"></param>
        /// <param name="projectID"></param>
        /// <param name="source">1、2-代表是（数据清洗）任务的导出；3-代表客户回访的问卷导出；4-代表是其他任务的问卷导出</param>
        /// <returns></returns>
        public string getOperator(string status, string siid, string projectID, string source)
        {
            string oper = string.Empty;

            int _siid;
            int _projectID;

            if (int.TryParse(siid, out _siid) && int.TryParse(projectID, out _projectID))
            {
                oper = "<a class='hrefExport' onclick=\"exportdata(" + _siid + "," + projectID + "," + source + ")\" href='javascript:void(0);' class='linkBlue'>导出</a>";
            }

            return oper;
        }
    }
}