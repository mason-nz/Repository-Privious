using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard
{
    public partial class UCConversationsView : System.Web.UI.UserControl
    {
        /// 对话ID
        /// <summary>
        /// 对话ID
        /// </summary>
        public string CSID { get; set; }

        private string PersonalIMURL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PersonalIMURL = ConfigurationUtil.GetAppSettingValue("PersonalIMURL");
                BindData();
            }
        }

        private void BindData()
        {
            DataRow dr = BLL.QS_IM_Result.Instance.GetQS_IM_ResultForCSID(CSID);
            if (dr != null)
            {
                //坐席
                spUserName.InnerText = dr["AgentUserName"].ToString();
                //对话时间
                spBeginTime.InnerText = CommonFunction.ObjectToDateTime(dr["BeginTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                if (dr["BeginTime"] != null && !string.IsNullOrEmpty(dr["BeginTime"].ToString()) && dr["EndTime"] != null && !string.IsNullOrEmpty(dr["EndTime"].ToString()))
                {
                    TimeSpan tsp = Convert.ToDateTime(dr["EndTime"].ToString()) - Convert.ToDateTime(dr["BeginTime"].ToString());
                    spCTime.InnerText = tsp.TotalSeconds.ToString() + "秒";
                }

                //对话ID
                int userID = BLL.Util.GetLoginUserID();
                string url = Server.UrlEncode(PersonalIMURL + "/ConversationHistoryForCC.aspx");
                spCsID.InnerHtml = "<a href='javascript:void(0)' onclick=\"GotoConversation(this,'" + url + "','" + userID + "','" + CSID + "')\">" + CSID + "</a>";

                //满意度
                spSatisfaction.InnerText = GetSatisfaction(dr);

                //成绩
                tdScore.InnerText = GetScore(dr);
                //是否显示
                if (CommonFunction.ObjectToInteger(dr["Result_Status"]) != (int)QSResultStatus.WaitScore)
                {
                    this.tdScore.Visible = true;
                }
            }
        }

        /// 获取满意度
        /// <summary>
        /// 获取满意度
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetSatisfaction(DataRow dr)
        {
            string info1 = GetSatisfaction(dr["PerSatisfaction"].ToString());
            string info2 = GetSatisfaction(dr["ProSatisfaction"].ToString());
            string info = "";
            if (info1 != "")
            {
                info += "服务" + info1 + "，";
            }
            if (info2 != "")
            {
                info += "产品" + info2 + "，";
            }
            if (info.Length > 1)
            {
                info = info.Substring(0, info.Length - 1);
            }
            return info;
        }
        /// 获取满意度
        /// <summary>
        /// 获取满意度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetSatisfaction(string value)
        {
            int a = CommonFunction.ObjectToInteger(value);
            switch (a)
            {
                case 5: return "非常满意";
                case 4: return "满意";
                case 3: return " 一般";
                case 2: return "不满意";
                case 1: return "非常不满意";
                default: return "";
            }
        }
        /// 获取成绩
        /// <summary>
        /// 获取成绩
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetScore(DataRow dr)
        {
            int ScoreType = CommonFunction.ObjectToInteger(dr["ScoreType"]);
            int QS_RID = CommonFunction.ObjectToInteger(dr["QS_RID"]);
            if (QS_RID > 0)
            {
                if (ScoreType == 1)
                {
                    return CommonFunction.ObjectToDecimal(dr["Score"]).ToString().Replace(".0", "") + "分";
                }
                else if (ScoreType == 3)
                {
                    return CommonFunction.ObjectToDecimal(dr["Score"]).ToString().Replace(".0", "") + "分";
                }
                else
                {
                    int q = CommonFunction.ObjectToInteger(dr["IsQualified"]);
                    if (q == 1)
                    {
                        return "合格";
                    }
                    else
                    {
                        return "不合格";
                    }
                }
            }
            else return "";
        }
    }
}