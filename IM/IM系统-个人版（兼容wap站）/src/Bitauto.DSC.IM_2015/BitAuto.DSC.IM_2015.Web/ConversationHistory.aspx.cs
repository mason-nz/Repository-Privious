using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Web.Security;
using Newtonsoft.Json;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class ConversationHistory : System.Web.UI.Page
    {
        public string realLoginID
        {
            get
            {
                return HttpContext.Current.Request["LoginID"] == null ? "" :
               HttpContext.Current.Request["LoginID"].ToString();
            }
        }
        public string realVisitID
        {
            get
            {
                return "";
            }
        }
        private string LoginID
        {
            get
            {
                return HttpContext.Current.Request["LoginID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LoginID"].ToString());
            }
        }
        private string VisitID
        {
            get
            {
                return "";
            }
        }
        private string QueryStartTime
        {
            get
            {
                return HttpContext.Current.Request["QueryStartTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStartTime"].ToString());
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
        public int thePageIndex
        {
            get
            {
                return HttpContext.Current.Request["page"] == null || string.IsNullOrEmpty(HttpContext.Current.Request["page"].ToString()) ? 1 :
                int.Parse(HttpContext.Current.Request["page"].ToString());
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
        //取登录用户userid
        public string GetLoginID()
        {
            string strUid = string.Empty;
            //如果是登录用户
            try
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //取uid
                    FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = identity.Ticket;
                    //userId以json字符串的格式存储在ticket.UserData中，将其反序列化后可获取，一下为思路，具体可自行处理
                    var userData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ticket.UserData);
                    if (userData != null)
                    {
                        if (userData.TryGetValue("UserId", out strUid))
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (string.IsNullOrEmpty(strUid))
            {
                if (Request.Cookies["hmc_loginid"] != null)
                {
                    strUid = HttpContext.Current.Server.UrlDecode(Request.Cookies["hmc_loginid"].Value);
                }
            }

            return strUid;
        }
        public void BindData()
        {
            RecordCount = 0;

            QueryConversations query = new QueryConversations();
            query.LoginID = Constant.STRING_INVALID_VALUE;
            //没有_
            if (!string.IsNullOrEmpty(LoginID))
            {
                string strUid = string.Empty;
                strUid = GetLoginID();
                if (LoginID.IndexOf("_") < 0)
                {
                    query.LoginID = strUid == string.Empty ? Constant.STRING_INVALID_VALUE : strUid;
                }
                else
                {
                    Guid g = Guid.Empty;
                    if (Guid.TryParse(LoginID.Split('_')[1], out g))
                    {
                        query.LoginID = LoginID;
                    }
                    else
                    {
                        query.LoginID = LoginID.Split('_')[0] + "_" + strUid;
                    }

                }
            }
            if (query.LoginID == Constant.STRING_INVALID_VALUE)
            {
                query.VisitID = "-1";
            }
            DateTime dateStart = Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(QueryStartTime, out dateStart))
            {
                query.QueryStarttime = dateStart;
            }
            DateTime dateEnd = Constant.DATE_INVALID_VALUE;
            if (DateTime.TryParse(QueryEndTime, out dateEnd))
            {
                query.QueryEndTime = dateEnd.AddDays(1);
            }
            try
            {
                DataTable dt = BLL.Conversations.Instance.GetConversationHistoryDataNew(query, "c.CreateTime asc", thePageIndex, 10, out RecordCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataColumn col1 = new DataColumn("newName", typeof(string));
                    dt.Columns.Add(col1);
                    try
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            switch (row["Sender"].ToString())
                            {
                                case "1": row["newName"] = "客服代表" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                                case "2": row["newName"] = row["UserName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                                default: row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                            }
                        }

                        Rt_CSHistoryData.DataSource = dt;
                        Rt_CSHistoryData.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        Response.End();
                    }
                }
                else
                {
                    DataTable dt2 = new DataTable();
                    DataColumn col1 = new DataColumn("newName");
                    DataColumn col2 = new DataColumn("Content");
                    dt2.Columns.Add(col1);
                    dt2.Columns.Add(col2);

                    DataRow row = dt2.NewRow();
                    row[0] = "";
                    row[1] = "没有找到符合查询条件的数据！";
                    dt2.Rows.Add(row);

                    Rt_CSHistoryData.DataSource = dt2;
                    Rt_CSHistoryData.DataBind();
                }

                litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, thePageIndex, 1);
            }
            catch (Exception ex)
            {
                DataTable dt2 = new DataTable();
                DataColumn col1 = new DataColumn("newName");
                DataColumn col2 = new DataColumn("Content");
                dt2.Columns.Add(col1);
                dt2.Columns.Add(col2);

                DataRow row = dt2.NewRow();
                row[0] = "";
                row[1] = "没有找到符合查询条件的数据！";
                dt2.Rows.Add(row);

                Rt_CSHistoryData.DataSource = dt2;
                Rt_CSHistoryData.DataBind();
            }
        }
    }
}