using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage
{
    public partial class ViewProject : PageBase
    {
        public string ProjectID
        {
            get
            {
                if (HttpContext.Current.Request["projectid"] == null)
                {
                    return "";
                }
                int intVal = 0;

                if (!int.TryParse(HttpContext.Current.Request["projectid"], out intVal))
                {
                    return "";
                }
                else
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
                }
            }
        }

        public bool ShowSurvery { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long projectid = CommonFunction.ObjectToLong(ProjectID);
                DataTable dt = GetProject(projectid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //基本信息
                    BaseInfo(dt.Rows[0]);
                    //问卷信息
                    SurveryInfo();
                    //外呼信息
                    OutCallInfo(projectid);
                }
            }
        }

        /// 获取项目信息
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="projectid"></param>
        /// <returns></returns>
        private DataTable GetProject(long projectid)
        {
            QueryProjectInfo query = new QueryProjectInfo();
            query.ProjectID = projectid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = BLL.ProjectInfo.Instance.GetProjectInfo(query, string.Empty, 1, 1, out count);
            return dt;
        }

        /// 基本信息
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="model"></param>
        private void BaseInfo(DataRow dr)
        {
            Literal_NM.Text = CommonFunction.ObjectToString(dr["Name"]);
            Literal_Type.Text = CommonFunction.ObjectToString(dr["CategoryName"]);
            string status = CommonFunction.ObjectToString(dr["Status"]);
            if (status == "0")
            {
                Literal_Status.Text = "未开始";
            }
            else if (status == "1")
            {
                Literal_Status.Text = "进行中";
            }
            else if (status == "2")
            {
                Literal_Status.Text = "已结束";
            }

            Literal_Total.Text = CommonFunction.ObjectToInteger(dr["SumCount"]).ToString();//已生成的任务
            Literal_End.Text = CommonFunction.ObjectToInteger(dr["comCount"]).ToString();//已处理

            Literal_Desc.Text = CommonFunction.ObjectToString(dr["Notes"]);
        }
        /// 问卷信息
        /// <summary>
        /// 问卷信息
        /// </summary>
        private void SurveryInfo()
        {
            ShowSurvery = false;
            Entities.QueryProjectSurveyMapping mapQuery = new Entities.QueryProjectSurveyMapping();
            mapQuery.ProjectID = int.Parse(ProjectID);
            int totalCount = 0;
            DataTable dt = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMapping(mapQuery, "", 1, 999, out totalCount);
            if (dt != null)
            {
                ShowSurvery = dt.Rows.Count > 0;
                Repeater_Survery.DataSource = dt;
                Repeater_Survery.DataBind();
            }
        }
        /// 外呼信息
        /// <summary>
        /// 外呼信息
        /// </summary>
        private void OutCallInfo(long projectid)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DataTable dt = BLL.ProjectInfo.Instance.GetProjectAutoCallInfo(projectid);
            bool hasauto = dt.Rows.Count != 0;

            Dictionary<string, int> stat = StatInfo(projectid);

            int zd_total = 0;
            int ivr_jt = 0;
            int ivr_not_jt = 0;
            int total = CommonFunction.ObjectToInteger(Literal_Total.Text);

            if (hasauto)
            {
                dic.Add("自动外呼状态", BLL.Util.GetEnumOptText(typeof(ProjectACStatus), CommonFunction.ObjectToInteger(dt.Rows[0]["ACStatus"])));
                dic.Add("对应技能组", CommonFunction.ObjectToString(dt.Rows[0]["SkillName"]));
                dic.Add("外显400号码", CommonFunction.ObjectToString(dt.Rows[0]["CallNum"]));

                //ivr接通总量 (容错：ivr接通总量>=自动接收外呼总量)
                ivr_jt = Math.Max(stat["ZD_Total"], CommonFunction.ObjectToInteger(dt.Rows[0]["IVRConnectNum"]));

                ivr_not_jt = CommonFunction.ObjectToInteger(dt.Rows[0]["DisconnectNum"]);
                //自动外呼总量（容错：自动外呼总量>=ivr接通总量+ivr未接通）
                zd_total = Math.Max(ivr_jt + ivr_not_jt, CommonFunction.ObjectToInteger(dt.Rows[0]["ACTotalNum"]));
            }
            dic.Add("总数据量", total.ToString());
            dic.Add("剩余量", "");

            if (hasauto)
            {
                dic.Add("自动呼出量", zd_total.ToString());
            }
            dic.Add("人工呼出量", stat["RG_Total"].ToString());//人工外呼总量

            if (hasauto)
            {
                dic.Add("IVR接通量", ivr_jt.ToString());
            }
            dic.Add("客服接通量", (stat["ZD_JT"] + stat["RG_JT"]).ToString());//自动客服接通+人工客服接通

            if (hasauto)
            {
                dic.Add("排队放弃量", (ivr_jt - stat["ZD_Total"]).ToString());//ivr接通-自动坐席接收量
            }
            dic.Add("未接通量", (ivr_not_jt + stat["RG_Not_JT"]).ToString());//ivr未接通+人工客服未接通量

            dic.Add("无效量", stat["WX"].ToString());//无效量

            dic["剩余量"] = (total - zd_total - stat["RG_Total"] - stat["WX"]).ToString();//总量-自动总量-人工总量-无效量

            string html = CreateHtml(dic);
            Literal_OutCall.Text = html;
        }
        /// 统计信息
        /// <summary>
        /// 统计信息
        /// </summary>
        private Dictionary<string, int> StatInfo(long projectid)
        {
            return BLL.ProjectInfo.Instance.GetProjectStatInfo(projectid);
        }

        private string CreateHtml(Dictionary<string, string> dic)
        {
            int i = 0;
            string key1 = "";
            string value1 = "";
            string key2 = "";
            string value2 = "";
            string html = "";
            foreach (string key in dic.Keys)
            {
                if (i == 0)
                {
                    key1 = key;
                    value1 = dic[key];
                    i++;
                }
                else if (i == 1)
                {
                    key2 = key;
                    value2 = dic[key];

                    html += GetTR(key1, value1, key2, value2) + "\r\n";
                    i = 0;
                }
            }
            if (i == 1)
            {
                html += GetTR(key1, value1, "", "") + "\r\n";
            }
            return html;
        }
        private string GetTR(string key1, string value1, string key2, string value2)
        {
            string str = "<tr><th width=\"15%\">" + key1 + "：</th><td width=\"30%\">" + value1 + "</td><th width=\"15%\">" + key2 + "：</th><td width=\"30%\">" + value2 + "</td></tr>";
            return str;
        }
    }
}