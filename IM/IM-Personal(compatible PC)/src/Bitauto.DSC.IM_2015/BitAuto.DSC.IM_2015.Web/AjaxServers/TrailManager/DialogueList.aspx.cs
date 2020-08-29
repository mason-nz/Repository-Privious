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
    public partial class DialogueList : System.Web.UI.Page
    {
        #region

        //访客来源
        private string SourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "-2" :
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
            dtNew.Columns.Add("总对话量");
            dtNew.Columns.Add("有效对话量");
            dtNew.Columns.Add("无效对话量");
            dtNew.Columns.Add("无效对话量占比");
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtNew.NewRow();
                dr["日期"] = item["DatePeriod"];
                dr["访客来源"] = item["SourceTypeName"];
                dr["总对话量"] = item["SumConversation"];
                dr["有效对话量"] = item["SumEffective"];
                dr["无效对话量"] = item["SumNoEffective"];
                dr["无效对话量占比"] = item["NoPercent"];
                dtNew.Rows.Add(dr);
            }
            BLL.Util.ExportToSCV("对话统计", dtNew, true);

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
        //数据
        private DataTable GetData(int index, int pageSize)
        {
            RecordCount = 0;
            var orderBy = SelectType == "2" ? "weekb desc" : (SelectType == "3" ? "monthb desc" : "daytime desc");
            DataTable dt = BLL.Conversations.Instance.S_BussinessLine_Total_Select(Get(), orderBy, index, pageSize, out RecordCount);
            dt.Columns.Add("DatePeriod");
            dt.Columns.Add("NoPercent");
            dt.Columns.Add("SourceTypeName");
            DataTable dtSum = BLL.Conversations.Instance.S_BussinessLine_Total_Select(Get());
            foreach (DataRow item in dtSum.Rows)
            {
                if (!string.IsNullOrEmpty(item[0].ToString()))
                {
                    dt.ImportRow(item);
                }
            }
            foreach (DataRow item in dt.Rows)
            {
                if (SelectType == "1")
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
                item["NoPercent"] = GetAvg(item["SumConversation"].ToString(), item["SumNoEffective"].ToString());

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