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
            if (!IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            RecordCount = 0;
            try
            {
                QueryConversations query = new QueryConversations();
                int csid;
                if (int.TryParse(CSID, out csid))
                {
                    query.CSID = csid;
                }
                else
                {
                    query.CSID = -1;
                }
                DataTable dt = BLL.Conversations.Instance.GetConversationHistoryData(query, "c.CreateTime asc", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);

                if (dt != null && dt.Rows.Count>0)
                {
                    DataColumn col1 = new DataColumn("newName", typeof(string));
                    dt.Columns.Add(col1);

                    foreach (DataRow row in dt.Rows)
                    {
                        switch (row["Sender"].ToString())
                        {
                            case "1": row["newName"] = "客服" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                            case "2": row["newName"] = row["UserName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                            default: row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                        }
                    }

                    Rt_CSHistoryData.DataSource = dt;
                    Rt_CSHistoryData.DataBind();

                    litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 4, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
                }
                else
                {
                    Response.Write("没有找到此对话的详细数据！");
                    Response.End();
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("异常信息:AjaxServers/ContentManage/CSHistoryData.aspx中，CSID=" + CSID + ",异常：" + ex.Message);
            }
        }
    }
}