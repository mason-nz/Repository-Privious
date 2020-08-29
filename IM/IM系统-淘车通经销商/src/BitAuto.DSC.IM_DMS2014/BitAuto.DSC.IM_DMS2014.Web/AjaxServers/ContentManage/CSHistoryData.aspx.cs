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
    public partial class CSHistoryData : System.Web.UI.Page
    {
        private string CSID
        {
            get
            {
                return HttpContext.Current.Request["CSID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CSID"].ToString());
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
            query.CSID = CSID == "" ? Constant.INT_INVALID_VALUE : int.Parse(CSID);

            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            DataColumn col1 = new DataColumn("newName",typeof(string));
            dt.Columns.Add(col1);

            foreach (DataRow row in dt.Rows)
            {
                switch (row["Sender"].ToString())
                {
                    case "1": row["newName"] = "客服" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    case "2": row["newName"] = row["MemberName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                    default: row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                }
            }

            Rt_CSHistoryData.DataSource = dt;
            Rt_CSHistoryData.DataBind();

            litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}