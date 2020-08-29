using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class MessageHistoryForAgent : System.Web.UI.Page
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
                return HttpContext.Current.Request["vid"] == null ? "" :
                HttpContext.Current.Request["vid"].ToString();
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
                return HttpContext.Current.Request["vid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["vid"].ToString());
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
            query.VisitID = VisitID == "" ? Constant.STRING_INVALID_VALUE : VisitID;
            query.LoginID = LoginID == "" ? Constant.STRING_INVALID_VALUE : LoginID;
            if (query.VisitID == Constant.STRING_INVALID_VALUE && query.LoginID == Constant.STRING_INVALID_VALUE)
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