using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_DMS2014.Entities;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_DMS2014.BLL;

namespace BitAuto.DSC.IM_DMS2014.Web
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
        public string GetLoginID()
        {
            string loginStr = "";
            string key = ConfigurationUtil.GetAppSettingValue("EPDESEncryptorKey");
            EP_DESEncryptor code = new EP_DESEncryptor(key);
            if (Request.Cookies["LoginID"] != null)
            {
                string eWord = string.Empty;
                eWord = code.DesDecrypt(Request.Cookies["LoginID"].Value);
                if (eWord.IndexOf('_') > 0)
                {
                    loginStr = eWord.Split('_')[0];
                }
            }
            return loginStr;
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
            string loginID = GetLoginID();
            int intLoginID = -100;
            if (int.TryParse(loginID, out intLoginID))
            {

            }
            query.LoginID = intLoginID == Constant.INT_INVALID_VALUE ? -100 : intLoginID;
          
            DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
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

            litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}