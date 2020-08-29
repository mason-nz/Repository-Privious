using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard
{
    public partial class IMList : PageBase
    {
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        bool right_view;            //查看
        bool right_firstInstance; //初审
        bool right_reviewInstance;//复审
        bool right_score;            //评分

        private string PersonalIMURL = "";

        /// <summary>
        /// JSON字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD6004"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                right_view = BLL.Util.CheckRight(userID, "SYS024BUT600402");
                right_firstInstance = BLL.Util.CheckRight(userID, "SYS024BUT600403");
                right_reviewInstance = BLL.Util.CheckRight(userID, "SYS024BUT600404");
                right_score = BLL.Util.CheckRight(userID, "SYS024BUT600405");

                PersonalIMURL = ConfigurationUtil.GetAppSettingValue("PersonalIMURL");

                BindData();
            }
        }

        private void BindData()
        {
            //解析参数
            string errMsg = string.Empty;
            QueryQS_IM_Result query = new QueryQS_IM_Result();
            BLL.ConverToEntitie<QueryQS_IM_Result> conver = new BLL.ConverToEntitie<QueryQS_IM_Result>(query);

            errMsg = conver.Conver(JsonStr);
            if (errMsg != "")
            {
                return;
            }
            //参数校验处理（必须）
            query.LoginUerID = BLL.Util.GetLoginUserID();
            query.InitCheck();
            switch (query.BeginCount)
            {
                case "-1":
                    query.BeginCount = "1";
                    query.EndCount = "";
                    break;
                case "1":
                    query.BeginCount = "1";
                    query.EndCount = "30";
                    break;
                case "2":
                    query.BeginCount = "31";
                    query.EndCount = "59";
                    break;
                case "3":
                    query.BeginCount = "60";
                    query.EndCount = "99";
                    break;
                case "4":
                    query.BeginCount = "100";
                    query.EndCount = "";
                    break;
                default: break;
            }
            //查询数据
            DataTable dt = BLL.QS_IM_Result.Instance.GetQS_IM_Result(query, "", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }

        /// 解析状态字段
        /// <summary>
        /// 解析状态字段
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetStatus(string status)
        {
            return BLL.Util.GetEnumOptText(typeof(QSResultStatus), CommonFunction.ObjectToInteger(status));
        }
        /// 解析会话id
        /// <summary>
        /// 解析会话id
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public string GetLink(string csid)
        {
            int userID = BLL.Util.GetLoginUserID();
            string url = Server.UrlEncode(PersonalIMURL + "/ConversationHistoryForCC.aspx");
            string link = "<a href='javascript:void(0)' onclick=\"GotoConversation(this,'" + url + "','" + userID + "','" + csid + "')\">" + csid + "</a>";
            return link;
        }
        /// 工单跳转
        /// <summary>
        /// 工单跳转
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public string GetOrderUrl(string orderid)
        {
            string url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(orderid, "", "");
            string viewpage = "javascript:void(0)";
            string target = "";
            if (url != "")
            {
                url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(orderid, url);
                if (url != "")
                {
                    target = "target=\"_blank\"";
                    viewpage = url;
                }
            }
            return "href='" + viewpage + "' " + target;
        }

        //TODO:CodeReview 2016-07-13 IM质检列表-方法名规范
        /// 绑定操作
        /// <summary>
        /// 绑定操作
        /// </summary>
        /// <param name="status">评分状态</param>
        /// <param name="rtid">评分表id</param>
        /// <param name="qs_rid">结果id</param>
        /// <param name="csid">会话id</param>
        /// <param name="createrid">坐席id</param>
        /// <returns></returns>
        public string oper(string status, string rtid, string qs_rid, string csid, string createrid, string scoretype)
        {
            string operStr = string.Empty;
            int _status;
            int _rtid;
            int _userid;
            if (!int.TryParse(status, out _status) || !int.TryParse(rtid, out _rtid) || !int.TryParse(createrid, out _userid))
            {
                return operStr;
            }

            if (_rtid > 0)
            {
                string strScoreType = BLL.QS_RulesTable.Instance.GetScoreTypeByRTID(_rtid);
                if (!string.IsNullOrEmpty(strScoreType))
                {
                    scoretype = strScoreType;
                }
            }
            switch (_status)
            {
                case 0:
                    //如果没有评分表怎不显示后面的评分链接
                    if (string.IsNullOrEmpty(rtid) || _rtid == -1)
                    {
                        return operStr;
                    }
                    else
                    {
                        operStr += ScoreLink(_rtid, qs_rid, csid, scoretype);
                    }
                    break;
                //待评分
                case (int)Entities.QSResultStatus.WaitScore:
                    operStr += ScoreLink(_rtid, qs_rid, csid, scoretype);
                    break;
                //已提交
                case (int)Entities.QSResultStatus.Submitted:
                    operStr += view(qs_rid, _userid, scoretype);
                    break;
                //待初审
                case (int)Entities.QSResultStatus.TobeFirstInstance:
                    operStr += firstInstance(qs_rid, scoretype);
                    break;
                //待复审
                case (int)Entities.QSResultStatus.TobeReviewInstance:
                    operStr += reviewInstance(qs_rid, scoretype);
                    break;
                //已评分
                case (int)Entities.QSResultStatus.RatedScore:
                    break;
                //已申诉
                case (int)Entities.QSResultStatus.Claimed:
                    break;
            }
            return operStr;
        }
        //评分
        private string ScoreLink(int RTID, string RID, string csid, string scoretype)
        {
            if (right_score)
            {
                return "<a onclick=\"javascript:clickScore('" + RTID + "','" + RID + "','" + csid + "','" + scoretype + "')\" href=\"javascript:void(0);\" >评分</a>&nbsp;";
            }
            else
            {
                return "";
            }
        }
        //查看
        private string view(string qs_rid, int _userid, string scoretype)
        {
            if (right_view && userID == _userid)
            {
                if (scoretype == "3")
                {
                    return "<a href=\"/QualityStandard/IMDisposeFiveLevel.aspx?QS_RID=" + qs_rid + "\" target='_blank'>查看</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/IMDispose.aspx?QS_RID=" + qs_rid + "\" target='_blank'>查看</a>&nbsp;";
                }
            }
            else
            {
                return "";
            }
        }
        //初审
        private string firstInstance(string qs_rid, string scoretype)
        {
            if (right_firstInstance)
            {
                if (scoretype == "3")
                {
                    return "<a href=\"/QualityStandard/IMDisposeFiveLevel.aspx?QS_RID=" + qs_rid + "\" target='_blank'>初审</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/IMDispose.aspx?QS_RID=" + qs_rid + "\" target='_blank'>初审</a>&nbsp;";
                }
            }
            else
            {
                return "";
            }
        }
        //复审
        private string reviewInstance(string qs_rid, string scoretype)
        {
            if (right_reviewInstance)
            {
                if (scoretype == "3")
                {
                    return "<a href=\"/QualityStandard/IMDisposeFiveLevel.aspx?QS_RID=" + qs_rid + "\" target='_blank'>复审</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/IMDispose.aspx?QS_RID=" + qs_rid + "\" target='_blank'>复审</a>&nbsp;";
                }
            }
            else
            {
                return "";
            }
        }
        //成绩查看
        public string ViewScore(string qs_rid, string scoretype, string score, string isqualified, string resultScore)
        {
            string value = "";
            if (GetStatus(resultScore) == "待评分")
            {
                return "&nbsp;";
            }
            //评分型
            if (scoretype == "1")
            {
                decimal s = CommonFunction.ObjectToDecimal(score);
                if (s >= 0)
                {
                    value = s.ToString().Replace(".0", "");
                }
            }
            else if (scoretype == "3")
            {
                decimal s = CommonFunction.ObjectToDecimal(score);
                if (s >= 0)
                {
                    value = s.ToString().Replace(".0", "");
                }
            }
            //合格型
            else if (scoretype == "2")
            {
                if (isqualified == "1")
                {
                    value = "合格";
                }
                else
                {
                    value = "不合格";
                }
            }
            //添加查看链接
            if (value != "")
            {
                if (scoretype == "3")
                {
                    return "<a href='/QualityStandard/IMQualityResultManage/QualityResultFiveLevelView.aspx?pagefrom=im&QS_RID=" + qs_rid + "' target=\"_blank\">" + value + "</a>";
                }
                else
                {
                    return "<a href='/QualityStandard/IMQualityResultManage/QualityResultView.aspx?pagefrom=im&QS_RID=" + qs_rid + "' target=\"_blank\">" + value + "</a>";
                }
            }
            else
                return "&nbsp;";
        }
    }
}