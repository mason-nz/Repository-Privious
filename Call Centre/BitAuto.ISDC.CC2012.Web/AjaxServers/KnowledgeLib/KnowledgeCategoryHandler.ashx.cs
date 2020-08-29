using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    /// <summary>
    /// KnowledgeCategoryHandler 的摘要说明
    /// </summary>
    public class KnowledgeCategoryHandler : IHttpHandler, IRequiresSessionState
    {
        #region
        public string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString().Trim()); }
        }
        public string RequestKCID
        {
            get { return HttpContext.Current.Request["KCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCID"].ToString()); }
        }
        public string RequestLevel
        {
            get { return HttpContext.Current.Request["Level"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Level"].ToString()); }
        }
        public string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        public string RequestStatus
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }
        public string SortId
        {
            get { return HttpContext.Current.Request["SortId"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SortId"].ToString()); }
        }
        public string SortType
        {
            get { return HttpContext.Current.Request["SortType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SortType"].ToString()); }
        }
        public string SortInfo
        {
            get { return HttpContext.Current.Request["SortInfo"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SortInfo"].ToString()); }
        }
        public int RegionID
        {
            get
            {
                string r = HttpContext.Current.Request["RegionID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RegionID"].ToString());
                return BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToInteger(r);
            }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            string msg = string.Empty;

            switch (RequestAction.ToLower())
            {
                case "bindknowledgecategory": BindKnowledgeCategory(out msg);
                    break;
                case "bindchildrencategoryinfo": BindChildrenCategoryInfo(out msg);
                    break;
                case "knowledgecategoryupdate": KnowledgeCategoryUpdate(out msg);
                    break;
                case "knowledgecategoryinsert": KnowledgeCategoryInsert(out msg);
                    break;
                case "knowledgecategorydelete": KnowledgeCategoryDelete(out msg);
                    break;
                case "knowledgecategorystatuschange": KnowledgeCategoryStatusChange(out msg);
                    break;
                case "deleteknowledgecategoryandchildren": DeleteKnowledgeCategoryAndChildren(out msg);
                    break;
                case "sortnumupordown": SortNumUpOrDown(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void SortNumUpOrDown(out string msg)
        {
            BLL.KnowledgeCategory.Instance.SortNumUpOrDown(SortId, SortType, SortInfo, out msg);
        }

        private void DeleteKnowledgeCategoryAndChildren(out string msg)
        {
            BLL.KnowledgeCategory.Instance.DeleteKnowledgeCategoryAndChildren(RequestKCID, out msg);
        }
        private void KnowledgeCategoryStatusChange(out string msg)
        {
            BLL.KnowledgeCategory.Instance.ChangeKnowledgeCategoryStatus(RequestKCID, out msg);
        }

        private void KnowledgeCategoryDelete(out string msg)
        {
            BLL.KnowledgeCategory.Instance.DeleteKnowledgeCategory(RequestKCID, out msg);
        }

        private void KnowledgeCategoryInsert(out string msg)
        {
            BLL.KnowledgeCategory.Instance.InsertKnowledgeCategory(RequestName, RequestKCID, RequestLevel, RegionID, out msg);
        }

        private void KnowledgeCategoryUpdate(out string msg)
        {
            BLL.KnowledgeCategory.Instance.UpdateKnowledgeCategory(RequestName, RequestKCID, out msg);
        }

        private void BindKnowledgeCategory(out string msg)
        {
            BLL.KnowledgeCategory.Instance.BindKnowledgeCategory(RequestLevel, RequestKCID, RegionID, out msg);
        }
        /// <summary>
        /// 根据父分类id和级别，获取子分类详细信息
        /// </summary>
        /// <param name="msg"></param>
        private void BindChildrenCategoryInfo(out string msg)
        {
            BLL.KnowledgeCategory.Instance.BindChildrenCategoryInfo(RequestKCID, RequestLevel, RequestName, RegionID, out msg);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}