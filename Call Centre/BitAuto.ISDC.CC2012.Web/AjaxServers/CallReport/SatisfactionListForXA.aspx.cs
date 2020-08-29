using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class SatisfactionListForXA : PageBase
    {
        #region 属性
        /// <summary>
        /// 坐席ID
        /// </summary>
        private string RequestAgentID
        {
            get { return HttpContext.Current.Request["AgentID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentID"].ToString()); }
        }
        /// <summary>
        /// 坐席工号
        /// </summary>
        private string RequestAgentNum
        {
            get { return HttpContext.Current.Request["AgentNum"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString()); }
        }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["TbeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TbeginTime"].ToString()); }
        }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["TendTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TendTime"].ToString()); }
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string RequestBusinessType
        {
            get { return HttpContext.Current.Request["BusinessType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BusinessType"].ToString()); }
        }
        /// <summary>
        /// 技能组
        /// </summary>
        private string RequestSkillGroup
        {
            get { return HttpContext.Current.Request["SkillGroup"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SkillGroup"].ToString()); }
        }
        private string RequestPageSize
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["pageSize"]) ? "20" :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["pageSize"].ToString());
            }
        }
        /// <summary>
        /// 1是日，2是周，3是月
        /// </summary>
        public string DateType
        {
            get { return HttpContext.Current.Request["DateType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DateType"].ToString()); }
        }
        #endregion

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        TelNumManage Manage
        {
            get
            {
                return BitAuto.ISDC.CC2012.BLL.CallDisplay.Manage;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }
        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetRound(string number)
        {
            decimal n = decimal.Parse(number) * 100;
            decimal d = decimal.Round(n, 2);
            return d.ToString() + "%";
        }
        //绑定数据
        public void BindData()
        {
            DateTime b = System.DateTime.Now;
            DateTime e = System.DateTime.Now;
            int _DateType = 1;
            int.TryParse(DateType, out _DateType);
            if (!int.TryParse(RequestPageSize, out PageSize))
            {
                PageSize = 20;
            }
            Entities.QueryCallRecord_ORIG query = new Entities.QueryCallRecord_ORIG();

            if (!string.IsNullOrEmpty(RequestAgentID))
            {
                int _createuserid = 0;
                if (int.TryParse(RequestAgentID, out _createuserid))
                {
                    query.CreateUserID = _createuserid;
                }
            }
            if (!string.IsNullOrEmpty(RequestAgentNum))
            {
                query.AgentNum = RequestAgentNum;
            }
            if (!string.IsNullOrEmpty(RequestBeginTime) && !string.IsNullOrEmpty(RequestEndTime))
            {
                DateTime.TryParse(RequestBeginTime, out b);
                DateTime.TryParse(RequestEndTime, out e);
                query.BeginTime = RequestBeginTime;
                query.EndTime = RequestEndTime;
            }
            if (!string.IsNullOrEmpty(RequestSkillGroup) && RequestSkillGroup != "-1")
            {
                query.SkillGroup = RequestSkillGroup;
            }
            if (!string.IsNullOrEmpty(RequestBusinessType))
            {
                query.SwitchINNum = RequestBusinessType;
            }
            query.LoginID = userID;
            int RecordCount = 0;
            //placeID=1是西安，DateType=1是按日汇总
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.BeginTime));
            DataTable dt = BLL.CallRecord_ORIG.Instance.GetSatisfactionList(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount, 1, b, e, _DateType, tableEndName);

            //取汇总行
            //if (dt != null && dt.Rows.Count > 0)
            //{
            int RecordCountSum = 0;
            DataTable dth = BLL.CallRecord_ORIG.Instance.GetSatisfactionList(query, "", 1, 1, out RecordCountSum, 1, b, e, tableEndName);
            if (dth != null && dth.Rows.Count > 0)
            {
                DataColumn colum = new DataColumn("HuiZong", typeof(string));
                dt.Columns.Add(colum);
                DataRow r = dt.NewRow();
                r["HuiZong"] = "合 计（共" + RecordCount + "项）";
                //总接通量,sum(y.jjcpN) as 问题解决参评总数,case when sum(case when y.CalliD is null then 1 else 0 end)>0 then sum(y.jjcpN)/sum(case when y.CalliD is null then 1 else 0 end) else 0 end as 参评率,sum(y.jjN) as 解决总数 ,case when sum(y.jjcpN)>0  then sum(y.jjN)/sum(y.jjcpN) else 0 end as 解决率,sum(y.WjjN) as 未解决总数,sum(y.MydcpN) as 满意度参评总数,case when sum(case when y.CalliD is null then 1 else 0 end)>0 then sum(y.MydcpN)/sum(case when y.CalliD is null then 1 else 0 end) else 0 end as 满意度参评率,sum(MyN) as 满意个数,case when sum(y.MydcpN)>0  then sum(y.MyN)/sum(y.MydcpN) else 0 end as 满意度, sum(y.BmyN) as 不满意个数,sum(y.CljgbmY) as 对处理结果不满意个数,sum(FwbmY) as 对服务不满意个数
                r["TrueName"] = "--";
                r["总接通量"] = dth.Rows[0]["总接通量"] == DBNull.Value ? 0 : dth.Rows[0]["总接通量"];
                r["问题解决参评总数"] = dth.Rows[0]["问题解决参评总数"] == DBNull.Value ? 0 : dth.Rows[0]["问题解决参评总数"];
                r["参评率"] = dth.Rows[0]["参评率"];
                r["解决总数"] = dth.Rows[0]["解决总数"] == DBNull.Value ? 0 : dth.Rows[0]["解决总数"];
                r["解决率"] = dth.Rows[0]["解决率"];
                r["未解决总数"] = dth.Rows[0]["未解决总数"] == DBNull.Value ? 0 : dth.Rows[0]["未解决总数"];
                r["满意度参评总数"] = dth.Rows[0]["满意度参评总数"] == DBNull.Value ? 0 : dth.Rows[0]["满意度参评总数"];
                r["满意度参评率"] = dth.Rows[0]["满意度参评率"];
                r["满意个数"] = dth.Rows[0]["满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["满意个数"];
                r["满意度"] = dth.Rows[0]["满意度"];
                r["不满意个数"] = dth.Rows[0]["不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["不满意个数"];
                r["对处理结果不满意个数"] = dth.Rows[0]["对处理结果不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["对处理结果不满意个数"];
                r["对服务不满意个数"] = dth.Rows[0]["对服务不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["对服务不满意个数"];
                dt.Rows.Add(r);

            }
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        /// 校验号码是否是下列热线中的一个
        /// <summary>
        /// 校验号码是否是下列热线中的一个
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public bool CheckTelNum(string tel)
        {
            return Manage.CheckTelIsInXANos(tel);
        }
    }
}