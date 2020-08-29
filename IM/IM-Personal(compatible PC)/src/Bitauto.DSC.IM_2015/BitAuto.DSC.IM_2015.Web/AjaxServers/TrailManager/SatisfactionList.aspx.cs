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
    public partial class SatisfactionList : System.Web.UI.Page
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
        //导出
        private void Export()
        {
            RecordCount = 0;
            DataTable dt = GetData(1, 20000);
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("日期");
            dtNew.Columns.Add("客服");
            dtNew.Columns.Add("工号");
            dtNew.Columns.Add("总对话量", typeof(Int32));
            dtNew.Columns.Add("总参评量", typeof(Int32));
            dtNew.Columns.Add("参评率");
            dtNew.Columns.Add("产品评价非常满意", typeof(Int32));
            dtNew.Columns.Add("产品评价满意", typeof(Int32));
            dtNew.Columns.Add("产品评价一般", typeof(Int32));
            dtNew.Columns.Add("产品评价不满意", typeof(Int32));
            dtNew.Columns.Add("产品评价非常不满意", typeof(Int32));
            dtNew.Columns.Add("产品评价满意率");
            dtNew.Columns.Add("服务评价非常满意", typeof(Int32));
            dtNew.Columns.Add("服务评价满意", typeof(Int32));
            dtNew.Columns.Add("服务评价一般", typeof(Int32));
            dtNew.Columns.Add("服务评价不满意", typeof(Int32));
            dtNew.Columns.Add("服务评价非常不满意", typeof(Int32));
            dtNew.Columns.Add("服务评价满意率");
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtNew.NewRow();
                dr["日期"] = item["DatePeriod"].ToString() == "" ? "合计（共" + RecordCount.ToString() + "项）" : item["DatePeriod"];
                dr["客服"] = item["TrueName"].ToString() == "" ? "--" : item["TrueName"];
                dr["工号"] = item["AgentNum"].ToString() == "" ? "--" : item["AgentNum"];
                dr["总对话量"] = item["sumduihua"];
                dr["总参评量"] = item["chanping"];
                dr["参评率"] = BLL.Util.GetLv(item["chanping"].ToString(), item["sumduihua"].ToString());
                dr["产品评价非常满意"] = item["profcmy"];
                dr["产品评价满意"] = item["promy"];
                dr["产品评价一般"] = item["proyb"];
                dr["产品评价不满意"] = item["probmy"];
                dr["产品评价非常不满意"] = item["profcbmy"];
                dr["产品评价满意率"] = GetAvg(item["profcmy"].ToString(), item["promy"].ToString(), item["chanping"].ToString());
                dr["服务评价非常满意"] = item["perfcmy"];
                dr["服务评价满意"] = item["permy"];
                dr["服务评价一般"] = item["peryb"];
                dr["服务评价不满意"] = item["perbmy"];
                dr["服务评价非常不满意"] = item["perfcbmy"];
                dr["服务评价满意率"] = GetAvg(item["perfcmy"].ToString(), item["permy"].ToString(), item["chanping"].ToString());
                dtNew.Rows.Add(dr);
            }
            BLL.Util.ExportToSCV("满意度统计", dtNew, false);

        }
        //数据
        private DataTable GetData(int index, int pageSize)
        {
            RecordCount = 0;
            int userID = BLL.Util.GetLoginUserID();
            var orderBy = SelectType == "2" ? "weekb desc" : (SelectType == "3" ? "monthb desc" : "daytime desc");
            DataTable dt = BLL.UserSatisfaction.Instance.UserSatisfaction_Total_Select(Get(), orderBy, index, pageSize, out RecordCount, userID);
            dt.Columns.Add("DatePeriod");
            DataTable dtSum = BLL.UserSatisfaction.Instance.UserSatisfaction_Total_Select(Get(), userID);
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
                if (!string.IsNullOrEmpty(item[0].ToString()))
                {
                    dt.ImportRow(item);
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

        //满意度
        public string GetAvg(string fcmy, string my, string chanping)
        {
            if (!string.IsNullOrEmpty(fcmy) && !string.IsNullOrEmpty(my) && !string.IsNullOrEmpty(chanping) && chanping != "0")
            {
                return ((float.Parse(fcmy) + float.Parse(my)) / float.Parse(chanping) * 100).ToString("N2") + "%";
            }
            return "-";
        }



    }
}