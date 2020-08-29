using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class CSData : System.Web.UI.Page
    {
        private string ReqVisitorName
        {
            get
            {
                return HttpContext.Current.Request["VisitorName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["VisitorName"].ToString());
            }
        }
        private string ReqPhone
        {
            get
            {
                return HttpContext.Current.Request["Phone"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Phone"].ToString());
            }
        }
        private string ReqTagBGID
        {
            get
            {
                return HttpContext.Current.Request["TagBGID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TagBGID"].ToString());
            }
        }
        private string ReqTagID
        {
            get
            {
                return HttpContext.Current.Request["TagID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TagID"].ToString());
            }
        }
        private string ReqAgentName
        {
            get
            {
                return HttpContext.Current.Request["AgentName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AgentName"].ToString());
            }
        }
        private string ReqOrderID
        {
            get
            {
                return HttpContext.Current.Request["OrderID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["OrderID"].ToString());
            }
        }
        private string ReqQueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
        private string ReqQueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString());
            }
        }



        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            RecordCount = 0;

            QueryConversations query = new QueryConversations();
            if (!string.IsNullOrEmpty(ReqVisitorName))
            {
                query.VisitorName = ReqVisitorName;
            }
            if (!string.IsNullOrEmpty(ReqPhone))
            {
                query.VisitorPhone = ReqPhone;
            }
            int tagbgid = Constant.INT_INVALID_VALUE;
            int tagid = Constant.INT_INVALID_VALUE;
            if (int.TryParse(ReqTagBGID, out tagbgid))
            {
                if (tagbgid != -2)
                {
                    query.TagBGID = tagbgid;

                    if (int.TryParse(ReqTagID, out tagid))
                    {
                        query.TagId = tagid;
                        DataTable dtTag = BLL.Conversations.Instance.GetConversationTagData(tagbgid);

                        string leafids = "";
                        if (query.TagId != -2)
                        {
                            BLL.Conversations.Instance.GetChildNode(dtTag,query.TagId.ToString(),ref leafids);
                        }
                        else
                        {
                            foreach (DataRow rw in dtTag.Rows)
                            {
                                leafids += "," + rw["RecID"].ToString();
                            }
                        }
                        query.TagIds = (leafids.Length > 1 ? leafids.Substring(1) : "-2");
                        query.TagId = Constant.INT_INVALID_VALUE;
                    }
                }
            }
            //if (int.TryParse(ReqTagID, out tagid))
            //{
            //    query.TagId = tagid;
            //}
            if (!string.IsNullOrEmpty(ReqAgentName))
            {
                query.UserName = ReqAgentName;
            }
            if (!string.IsNullOrEmpty(ReqOrderID))
            {
                query.OrderID = ReqOrderID;
            }
            DateTime dtStart = Constant.DATE_INVALID_VALUE;
            DateTime dtEnd = Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(ReqQueryStarttime, out dtStart))
            {
                query.QueryStarttime = dtStart;
            }
            if (DateTime.TryParse(ReqQueryEndTime, out dtEnd))
            {
                query.QueryEndTime = dtEnd.AddSeconds(1);
            }

            query.UserID = BLL.Util.GetLoginUserID();


            DataTable dt = BLL.Conversations.Instance.GetCSData(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);

            Rt_CSData.DataSource = dt;
            Rt_CSData.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 2);
        }

        
    }
}