using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Entities;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage
{
    public partial class CSData : System.Web.UI.Page
    {
        private string MemberName
        {
            get
            {
                return HttpContext.Current.Request["MemberName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MemberName"].ToString());
            }
        }
         private string District
        {
            get
            {
                return HttpContext.Current.Request["District"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["District"].ToString());
            }
        }
        private string CityGroup
        {
            get
            {
                return HttpContext.Current.Request["CityGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CityGroup"].ToString());
            }
        }
        private string UserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString());
            }
        }
         private string QueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
         private string QueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString());
            }
        }
         private string OrderID
        {
            get
            {
                return HttpContext.Current.Request["OrderID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["OrderID"].ToString());
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
            query.MemberName = MemberName =="" ? Constant.STRING_INVALID_VALUE:MemberName;
            query.District = District == "" ? Constant.STRING_INVALID_VALUE : District;
            //query.CityGroup = CityGroup == "" ? Constant.STRING_INVALID_VALUE : CityGroup;
            query.UserName = UserName == "" ? Constant.STRING_INVALID_VALUE : UserName;
            query.QueryStarttime = QueryStarttime == "" ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(QueryStarttime);
            query.QueryEndTime = QueryEndTime == "" ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(QueryEndTime).AddDays(1);
            query.OrderID = OrderID == "" ? Constant.STRING_INVALID_VALUE : OrderID;

            DataTable dt = BLL.Conversations.Instance.GetCSData(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            Rt_CSData.DataSource = dt;
            Rt_CSData.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex,2);
        }
    }
}