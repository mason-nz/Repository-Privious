using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers
{
    public partial class ConversationHistory : System.Web.UI.Page
    {
        private string LoginID
        {
            get
            {
                return HttpContext.Current.Request["LoginID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LoginID"].ToString());
            }
        }
        private string UserID
        {
            get
            {
                return HttpContext.Current.Request["UserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserID"].ToString());
            }
        }

        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            RecordCount = 0;

            QueryConversations query = new QueryConversations();
            query.UserID = UserID == "" ? Constant.INT_INVALID_VALUE : int.Parse(UserID);
            query.LoginID = LoginID == "" ? Constant.STRING_INVALID_VALUE : LoginID;

            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "", BLL.PageCommon.Instance.PageIndex, 3, out RecordCount);
            DataColumn col1 = new DataColumn("newName", typeof(string));
            dt.Columns.Add(col1);

            foreach (DataRow row in dt.Rows)
            {
                switch (row["Sender"].ToString())
                {
                    case "1": row["newName"] = "客服代表" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    case "2": row["newName"] = row["MemberName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    default: row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                }
            }

            Rt_CSHistoryData.DataSource = dt;
            Rt_CSHistoryData.DataBind();

            litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 3, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}