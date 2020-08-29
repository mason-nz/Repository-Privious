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
        #region

        //客服ID
        private string UserID
        {
            get
            {
                return HttpContext.Current.Request["UserID"] == null ? "-2" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserID"].ToString());
            }
        }
        //员工编号
        private string AgentNum
        {
            get
            {
                return HttpContext.Current.Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString());
            }
        }
        //日期
        private string Starttime
        {
            get
            {
                return HttpContext.Current.Request["Starttime"] == null ? "1900-01-01" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Starttime"].ToString());
            }
        }
        //日期
        private string EndTime
        {
            get
            {
                return HttpContext.Current.Request["EndTime"] == null ? "1900-01-01" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["EndTime"].ToString());
            }
        }
        //分组Id
        private string BGID
        {
            get
            {
                return HttpContext.Current.Request["GroupID"] == null ? "-1" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["GroupID"].ToString());
            }
        }
        //统计维度
        private string SelectType
        {
            get
            {
                return HttpContext.Current.Request["SelectType"] == null ? "1" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SelectType"].ToString());
            }
        }
        #endregion
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
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

        protected QueryUserSatisfactionTotal Get()
        {
            QueryUserSatisfactionTotal query = new QueryUserSatisfactionTotal();
            query.AgentNum = AgentNum;
            int userid = -2;
            int.TryParse(UserID, out userid);
            query.UserID = userid;
            query.BeginTime = DateTime.Parse(Starttime);
            query.EndTime = DateTime.Parse(EndTime);
            query.BGID = int.Parse(BGID);
            query.SelectType = int.Parse(SelectType);
            return query;
        }

        //数据
        private DataTable GetData(int index, int pageSize)
        {
            RecordCount = 0;
            int userID = BLL.Util.GetLoginUserID();
            var orderBy = SelectType == "2" ? "weekb desc" : (SelectType == "3" ? "monthb desc" : "daytime desc");
            DataTable dt = BLL.Conversations.Instance.S_Agent_Total_Select(Get(), orderBy, index, pageSize, out RecordCount, userID); ;
            dt.Columns.Add("DatePeriod");
            DataTable dtSum = BLL.Conversations.Instance.S_Agent_Total_Select(Get(), userID);
            foreach (DataRow item in dt.Rows)
            {
                if (SelectType == "1")
                {
                    item["DatePeriod"] = Convert.ToDateTime(item["daytime"].ToString()).ToString("yyyy-MM-dd");
                }
                else if (SelectType == "2")
                {
                    item["DatePeriod"] = Convert.ToDateTime(item["weekb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["weeke"].ToString()).ToString("yyyy-MM-dd");
                }
                else if (SelectType == "3")
                {
                    item["DatePeriod"] = Convert.ToDateTime(item["monthb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["monthe"].ToString()).ToString("yyyy-MM-dd");
                }
            }
            foreach (DataRow item in dtSum.Rows)
            {
                dt.ImportRow(item);
            }
            return dt;
        }
        //导出
        public void Export()
        {
            DataTable dt = GetData(1, 20000);
            DataTable dtNew = new DataTable();
            string[] arr = new string[13] { "日期", "客服","分组","工号", "在线时长", "总对话时长", "平均对话时长", "首次平均响应时长(秒)", "总对话量", "总接待量", "响应率", "客服消息发送量", "访客消息发送量" };
            for (int i = 0; i < arr.Length; i++)
            {
                dtNew.Columns.Add(arr[i]);
            }
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtNew.NewRow();
                dr["日期"] = item["DatePeriod"].ToString() == "" ? "合计（共" + RecordCount.ToString() + "项）" : item["DatePeriod"];
                dr["客服"] = item["truename"].ToString() == "" ? "--" : item["truename"];
                dr["分组"] = item["BGName"].ToString() == "" ? "--" : item["BGName"];
                dr["工号"] = item["AgentNum"].ToString() == "" ? "--" : item["AgentNum"];
                dr["在线时长"] = ConvertDate(item["sumonlinetime"].ToString());
                dr["总对话时长"] = ConvertDate(item["SumConversationTimeLong"].ToString());
                dr["平均对话时长"] = GetAvgConver(item["SumConversations"].ToString(), item["SumConversationTimeLong"].ToString());
                dr["首次平均响应时长(秒)"] = GetAvgNum(item["SumReception"].ToString(), item["SumFRTimeLong"].ToString());
                dr["总对话量"] = item["SumConversations"].ToString();
                dr["总接待量"] = item["SumReception"].ToString();
                dr["响应率"] = GetAvg(item["SumConversations"].ToString(), item["SumReception"].ToString());
                dr["客服消息发送量"] = item["SumAgentDailog"].ToString();
                dr["访客消息发送量"] = item["SumNetFriendDailog"].ToString();
                dtNew.Rows.Add(dr);
            }
            BLL.Util.ExportToSCV("客服统计导出", dtNew, true);

        }

        //绑定数据
        public void BindData()
        {
            RecordCount = 0;
            DataTable dt = GetData(BLL.PageCommon.Instance.PageIndex, 20);
            rpeList.DataSource = dt;
            rpeList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 20, BLL.PageCommon.Instance.PageIndex, 1);

        }



        //平均计算方法
        public string GetAvgNum(string count, string conver)
        {
            if (count != "0" && count != "" && conver != "")
            {
                return (int.Parse(conver) / int.Parse(count)).ToString();
            }
            return "0";
        }
        //平均时间计算方法
        public string GetAvgConver(string count, string conver)
        {
            if (count != "0" && count != "" && conver != "")
            {
                return ConvertDate((int.Parse(conver) / int.Parse(count)).ToString());
            }
            return "0";
        }

        //率计算方法
        public string GetAvg(string str1, string str2)
        {
            if (str1 != "0" && str1 != "" && str2 != "")
            {
                return (float.Parse(str2) / float.Parse(str1) * 100).ToString("N2") + "%";
            }
            return "-";
        }

        //把秒转换为时间形式
        public string ConvertDate(string mm)
        {
            if (string.IsNullOrEmpty(mm))
                return "-";
            if (mm == "0")
                return "0";
            TimeSpan span = new TimeSpan(0, 0, int.Parse(mm));
            return (span.Hours + (span.Days * 24)).ToString().PadLeft(2, '0') + ":" + span.Minutes.ToString().PadLeft(2, '0') + ":" + span.Seconds.ToString().PadLeft(2, '0');
        }
    }
}