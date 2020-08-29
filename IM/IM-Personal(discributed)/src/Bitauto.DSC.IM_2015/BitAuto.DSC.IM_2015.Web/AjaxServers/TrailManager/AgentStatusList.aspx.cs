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
    public partial class AgentStatusList : System.Web.UI.Page
    {
        //客服
        private string UserName
        {
            get
            {
                return HttpContext.Current.Request["UserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UserName"].ToString());
            }
        }
        //客服状态
        private string Status
        {
            get
            {
                return HttpContext.Current.Request["Status"] == null ? "-2" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Status"].ToString());
            }
        }
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
        private string Code
        {
            get
            {
                return HttpContext.Current.Request["Code"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Code"].ToString());
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


        protected QueryAgentStatusDetail Get()
        {
            RecordCount = 0;
            var date = DateTime.Now;
            QueryAgentStatusDetail query = new QueryAgentStatusDetail() { AgentNum = Code, UserID = Convert.ToInt32(UserID), Starttime = Convert.ToDateTime(Starttime), Status = Convert.ToInt32(Status) };
            return query;
        }


        //导出
        private void Export()
        {
            RecordCount = 0;
            DataTable dt = BLL.AgentStatusDetail.Instance.Get(Get(), " StartTime ", 1, 20000, out RecordCount);
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("日期");
            dtNew.Columns.Add("所属分组");
            dtNew.Columns.Add("客服");
            dtNew.Columns.Add("工号");
            dtNew.Columns.Add("状态");
            dtNew.Columns.Add("状态开始时间");
            dtNew.Columns.Add("状态结束时间");
            dtNew.Columns.Add("状态持续时长");
            foreach (DataRow item in dt.Rows)
            {
                DataRow dr = dtNew.NewRow();
                dr["日期"] = Convert.ToDateTime(item["StartTime"]).ToString("yyyy-MM-dd");
                dr["所属分组"] = item["BGName"];
                dr["客服"] = item["TrueName"];
                dr["工号"] = item["AgentNum"];
                dr["状态"] = GetStatusText(item["Status"].ToString());
                dr["状态开始时间"] = Convert.ToDateTime(item["StartTime"]).ToString("yyyy-MM-dd  HH:mm:ss");
                dr["状态结束时间"] = item["EndTime"].ToString() == "" ? "" : Convert.ToDateTime(item["EndTime"]).ToString("yyyy-MM-dd  HH:mm:ss");
                dr["状态持续时长"] = GetTimeLongText(item["TimeLong"].ToString());
                dtNew.Rows.Add(dr);
            }
            BLL.Util.ExportToSCV("客服状态明细", dtNew, true);

        }

        //绑定数据
        private void BindData()
        {
            RecordCount = 0;
            DataTable dt = BLL.AgentStatusDetail.Instance.Get(Get(), " StartTime ", BLL.PageCommon.Instance.PageIndex, 20, out RecordCount);
            rpeList.DataSource = dt;
            rpeList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 20, BLL.PageCommon.Instance.PageIndex, 1);
        }

        /// <summary>
        /// 状态值
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected string GetStatusText(string status)
        {
            return BLL.Util.GetEnumOptText(typeof(AgentStatus), int.Parse(status));
        }
        //格式化在线时长
        protected string GetTimeLongText(string timeLong)
        {
            int mm = int.Parse(timeLong);
            TimeSpan span = new TimeSpan(0, 0, mm);
            return (span.Hours + (span.Days * 24)).ToString().PadLeft(2, '0') + ":" + span.Minutes.ToString().PadLeft(2, '0') + ":" + span.Seconds.ToString().PadLeft(2, '0');

        }
    }
}