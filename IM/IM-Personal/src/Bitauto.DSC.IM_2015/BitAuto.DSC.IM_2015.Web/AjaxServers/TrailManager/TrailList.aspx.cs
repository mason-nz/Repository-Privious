using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;
using System.Configuration;


namespace BitAuto.DSC.IM_2015.Web.AjaxServers.TrailManager
{
    public partial class TrailList : System.Web.UI.Page
    {
        private string UserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString());
            }
        }
        private string GroupId
        {
            get
            {
                return HttpContext.Current.Request["GroupId"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["GroupId"].ToString());
            }
        }
        private string Code
        {
            get
            {
                return HttpContext.Current.Request["Code"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Code"].ToString());
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
        private string SourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceType"].ToString());
            }
        }
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Request["export"] == "1")
                {
                    Export();
                }
                else
                {
                    BindData();
                }
            }
        }

        public QueryConversations Get()
        {
            RecordCount = 0;
            var date = DateTime.Now;
            QueryConversations query = new QueryConversations() { CreateTime = date, EndTime = date, UserName = UserName };
            query.UserID = BLL.Util.GetLoginUserID();
            if (QueryStarttime != "")
            {
                query.CreateTime = Convert.ToDateTime(QueryStarttime);
            }
            if (QueryEndTime != "")
            {
                query.EndTime = Convert.ToDateTime(QueryEndTime).AddDays(1);
            }
            if (SourceType != "-1")
            {
                query.SourceType = Convert.ToInt32(SourceType);
            }
            if (GroupId != "-1")
            {
                query.BGID = Convert.ToInt32(GroupId);
            }
            if (Code != "")
            {
                query.AgentNum = Convert.ToInt32(Code);
            }
            return query;
        }


        //导出
        public void Export()
        {
            RecordCount = 0;
            DataSet ds = BLL.Conversations.Instance.GetConverStatistics(Get(), " StatDate DESC", 1, 20000, out RecordCount);
            if (RecordCount != 0)
            {
                DataTable dt = ds.Tables[1];
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["allcount"] = item["allcount"];
                    dr["allConver"] = item["allConver"];
                    dr["Ccount"] = item["Ccount"];
                    dr["Pcount"] = item["Pcount"];
                    dr["turmInCount"] = item["turmInCount"];
                    dr["turmOutCount"] = item["turmOutCount"];
                    dr["timelong"] = item["timelong"];
                    dt.Rows.Add(dr);
                }


                DataTable dtNew = new DataTable();
                string[] arr = new string[11] { "日期", "客服", "工号", "总对话量", "总对话时长", "平均对话时长", "总在线时长", "客服消息发送量", "访客消息发送量", "对话转出次数", "对话转入次数" };
                for (int i = 0; i < arr.Length; i++)
                {
                    dtNew.Columns.Add(arr[i]);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtNew.NewRow();
                    dr["日期"] = dt.Rows[i]["statDate"].ToString() == "" ? "合计（共" + RecordCount + "项）" : Convert.ToDateTime(dt.Rows[i]["statDate"]).ToString("yyyy-MM-dd");
                    dr["客服"] = dt.Rows[i]["UserName"];
                    dr["工号"] = dt.Rows[i]["AgentNum"].ToString(); ;
                    dr["总对话量"] = dt.Rows[i]["allCount"];
                    dr["总对话时长"] = ConvertDate(int.Parse(dt.Rows[i]["allConver"].ToString() == "" ? "0" : dt.Rows[i]["allConver"].ToString()));
                    dr["平均对话时长"] = GetAvgConver(dt.Rows[i]["allCount"].ToString(), dt.Rows[i]["allConver"].ToString());
                    dr["总在线时长"] = ConvertDate(int.Parse(dt.Rows[i]["timelong"].ToString() == "" ? "0" : dt.Rows[i]["timelong"].ToString()));
                    dr["客服消息发送量"] = dt.Rows[i]["Ccount"].ToString() == "" ? "0" : dt.Rows[i]["Ccount"].ToString();
                    dr["访客消息发送量"] = dt.Rows[i]["Pcount"].ToString() == "" ? "0" : dt.Rows[i]["Pcount"].ToString();
                    dr["对话转出次数"] = dt.Rows[i]["turmOutCount"];
                    dr["对话转入次数"] = dt.Rows[i]["turmInCount"];
                    dtNew.Rows.Add(dr);
                }
                BLL.Util.ExportToSCV("客服统计导出", dtNew, true);
            }
        }

        //绑定数据
        public void BindData()
        {
            RecordCount = 0;
            DataSet ds = BLL.Conversations.Instance.GetConverStatistics(Get(), " StatDate DESC", BLL.PageCommon.Instance.PageIndex, 20, out RecordCount);
            if (RecordCount != 0)
            {
                DataTable dt = ds.Tables[1];
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["allcount"] = item["allcount"];
                    dr["allConver"] = item["allConver"];
                    dr["Ccount"] = item["Ccount"];
                    dr["Pcount"] = item["Pcount"];
                    dr["turmInCount"] = item["turmInCount"];
                    dr["turmOutCount"] = item["turmOutCount"];
                    dr["timelong"] = item["timelong"];
                    dt.Rows.Add(dr);
                }

                rpeList.DataSource = dt;
                rpeList.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 20, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }

        //平均对话时长
        public string GetAvgConver(string count, string conver)
        {
            if (count != "0" && count != "" && conver != "")
            {
                return ConvertDate(int.Parse(conver) / int.Parse(count));
            }
            return "0";
        }

        //工时利用率
        public string GetAvg(string online, string conver)
        {
            if (online != "0" && online != "" && conver != "")
            {
                return (float.Parse(conver) / float.Parse(online) * 100).ToString("N2") + "%";
            }
            return "0";
        }

        //把秒转换为时间形式
        public string ConvertDate(int mm)
        {
            if (mm == 0)
                return "0";
            TimeSpan span = new TimeSpan(0, 0, mm);
            return (span.Hours + (span.Days * 24)).ToString().PadLeft(2, '0') + ":" + span.Minutes.ToString().PadLeft(2, '0') + ":" + span.Seconds.ToString().PadLeft(2, '0');
        }
    }
}