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
    public partial class AlexaList : System.Web.UI.Page
    {
        #region

        //访客来源
        private string SourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "-1" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceType"].ToString());
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
        //统计维度
        private string SelectType
        {
            get
            {
                return HttpContext.Current.Request["SelectType"] == "0" ? "4" :
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
        protected QueryBussinessLineTotal Get()
        {
            QueryBussinessLineTotal query = new QueryBussinessLineTotal();
            query.SourceType = int.Parse(SourceType);
            query.BeginTime = DateTime.Parse(Starttime);
            query.EndTime = DateTime.Parse(EndTime);
            query.SelectType = int.Parse(SelectType);
            return query;
        }
        //导出
        private void Export()
        {
            RecordCount = 0;
            DataTable dt = GetData(1, 20000);
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("日期");
            dtNew.Columns.Add("访客来源");
            dtNew.Columns.Add("页面访问量");
            dtNew.Columns.Add("总对话量");
            dtNew.Columns.Add("队列放弃量");
            dtNew.Columns.Add("登录访客总量");
            dtNew.Columns.Add("匿名访客总量");

            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtNew.NewRow();
                dr["日期"] = item["DatePeriod"];
                dr["访客来源"] = item["SourceTypeName"];
                dr["页面访问量"] = item["SumVisit"];
                dr["总对话量"] = item["SumConversation"];
                dr["队列放弃量"] = item["SumQueueFail"];
                dr["登录访客总量"] = item["LoginVisit"];
                dr["匿名访客总量"] = item["NoLoginVisit"];
                dtNew.Rows.Add(dr);
            }
            BLL.Util.ExportToSCV("流量统计", dtNew, true);

        }
        //数据
        private DataTable GetData(int index, int pageSize)
        {
            RecordCount = 0;
            var orderBy = "hourtime";
            if (SelectType != "4")
            {
                orderBy = SelectType == "2" ? "weekb desc" : (SelectType == "3" ? "monthb desc" : "daytime desc");
            }
            DataTable dt = BLL.Conversations.Instance.S_Flow_Total_Select(Get(), orderBy, index, pageSize, out RecordCount);
            dt.Columns.Add("DatePeriod");
            dt.Columns.Add("SourceTypeName");
            DataTable dtSum = BLL.Conversations.Instance.S_Flow_Total_Select(Get());
            foreach (DataRow item in dtSum.Rows)
            {
                if (!string.IsNullOrEmpty(item[0].ToString()))
                {
                    dt.ImportRow(item);
                }
            }
            foreach (DataRow item in dt.Rows)
            {
                if (SelectType == "4")
                {
                    item["DatePeriod"] = (item["hourtimename"].ToString() == "" ? "合计（共" + RecordCount + "项）" : item["hourtimename"]);
                }
                else if (SelectType == "1")
                {
                    item["DatePeriod"] = (item["daytime"].ToString() == "" ? "合计（共" + RecordCount + "项）" : Convert.ToDateTime(item["daytime"].ToString()).ToString("yyyy-MM-dd"));
                }
                else if (SelectType == "2")
                {
                    item["DatePeriod"] = (item["weekb"].ToString() == "" ? "合计（共" + RecordCount + "项）" : Convert.ToDateTime(item["weekb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["weeke"].ToString()).ToString("yyyy-MM-dd"));
                }
                else if (SelectType == "3")
                {
                    item["DatePeriod"] = (item["monthb"].ToString() == "" ? "合计（共" + RecordCount + "项）" : Convert.ToDateTime(item["monthb"].ToString()).ToString("yyyy-MM-dd") + "至" + Convert.ToDateTime(item["monthe"].ToString()).ToString("yyyy-MM-dd"));
                }
                if (item["SourceType"].ToString() == "")
                {
                    item["SourceTypeName"] = "--";
                }
                else
                {
                    item["SourceTypeName"] = BLL.Util.GetSourceTypeName(item["SourceType"].ToString());
                }
            }

            return dt;
        }
        //绑定数据
        private void BindData()
        {
            rpeList.DataSource = GetData(BLL.PageCommon.Instance.PageIndex, 20);
            rpeList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 20, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}