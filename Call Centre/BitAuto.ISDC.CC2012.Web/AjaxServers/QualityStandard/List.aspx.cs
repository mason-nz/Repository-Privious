using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard
{
    public partial class List : PageBase
    {
        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID
        public string tableEndName = "";

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

        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        bool right_view;            //查看权限 
        bool right_firstInstance; //初审
        bool right_reviewInstance;//复审
        bool right_score;            //评分

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                right_view = BLL.Util.CheckRight(userID, "SYS024BUT600101");
                right_firstInstance = BLL.Util.CheckRight(userID, "SYS024BUT600102");
                right_reviewInstance = BLL.Util.CheckRight(userID, "SYS024BUT600103");
                right_score = BLL.Util.CheckRight(userID, "SYS024BUT600104");
                BindData();
            }
        }

        //链接
        public string GetTaskViewUrl(string TaskID, string BGID, string SCID)
        {
            string url = "";
            url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(TaskID, BGID, SCID);
            url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(TaskID, url);
            return BLL.Util.GenBusinessURLByBGIDAndSCID(BGID, SCID, url, TaskID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
        }

        //绑定数据
        public void BindData()
        {
            Entities.QueryCallRecordInfo query = new Entities.QueryCallRecordInfo();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.QueryCallRecordInfo> conver = new BLL.ConverToEntitie<Entities.QueryCallRecordInfo>(query);
            errMsg = conver.Conver(JsonStr);

            #region 从JSONSTR中解析出通话时间
            string begintime = "";
            string endtime = "";
            string[] array1 = JsonStr.Split('&');
            if (array1.Length >= 3)
            {
                begintime = array1[1].Split('=')[1];
                if (!string.IsNullOrEmpty(begintime))
                {
                    begintime = begintime.Insert(10, " ");
                    query.BeginTime = DateTime.Parse(begintime);
                }
                endtime = array1[2].Split('=')[1];
                if (!string.IsNullOrEmpty(endtime))
                {
                    endtime = endtime.Insert(10, " ");
                    query.EndTime = DateTime.Parse(endtime);
                }
            }
            #endregion

            if (errMsg != "")
            {
                return;
            }
            query.LoginID = userID;
            query.IsFilterNull = 1;
            int RecordCount = 0;

            tableEndName = BLL.Util.CalcTableNameByMonth(3, query.BeginTime.Value);
            DataTable dt = BLL.QS_Result.Instance.GetQS_ResultList(query, " cob.CreateTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, tableEndName, out RecordCount);

            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }

        //绑定操作
        public string oper(string status, string rtid, string qs_rid, string callID, string createrID, string scoreType)
        {
            string operStr = string.Empty;
            int _status;
            int _rtid;
            int _userid;
            string scoretype = "";
            if (!int.TryParse(status, out _status) || !int.TryParse(rtid, out _rtid) || !int.TryParse(createrID, out _userid))
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
                case 0://如果没有评分表怎不显示后面的评分链接
                    if (string.IsNullOrEmpty(rtid) || _rtid == -1)
                    {
                        //operStr += score(0, qs_rid, "0");
                        return operStr;
                    }
                    else
                    {
                        operStr += ScoreUrl(_rtid, qs_rid, callID, scoretype);
                    }
                    break;
                case (int)Entities.QSResultStatus.WaitScore:
                    operStr += ScoreUrl(_rtid, qs_rid, callID, scoretype);
                    break;
                case (int)Entities.QSResultStatus.Submitted:
                    operStr += viewUrl(qs_rid, _userid, scoretype);
                    break;
                case (int)Entities.QSResultStatus.TobeFirstInstance:
                    operStr += firstInstance(qs_rid, scoretype);
                    break;
                case (int)Entities.QSResultStatus.TobeReviewInstance:
                    operStr += reviewInstance(qs_rid, scoretype);
                    break;
                case (int)Entities.QSResultStatus.RatedScore:
                    //operStr += viewforend(qs_rid);
                    break;
                case (int)Entities.QSResultStatus.Claimed:
                    //operStr += viewforend(qs_rid);
                    break;
            }
            return operStr;
        }
        //申诉的查看
        private string viewUrl(string qs_rid, int _userid, string scoretype)
        {
            if (right_view && userID == _userid)
            {
                if (scoretype == "3")
                {
                    return "<a href=\"/QualityStandard/DisposeFiveLevel.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>查看</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/Dispose.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>查看</a>&nbsp;";
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
                    return "<a href=\"/QualityStandard/DisposeFiveLevel.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>初审</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/Dispose.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>初审</a>&nbsp;";
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
                    return "<a href=\"/QualityStandard/DisposeFiveLevel.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>复审</a>&nbsp;";
                }
                else
                {
                    return "<a href=\"/QualityStandard/Dispose.aspx?QS_RID=" + qs_rid + "&tableEndName=" + tableEndName + "\" target='_blank'>复审</a>&nbsp;";
                }
            }
            else
            {
                return "";
            }
        }
        //评分
        private string ScoreUrl(int RTID, string RID, string CallID, string scoreType)
        {
            if (right_score)
            {
                return "<a onclick=\"javascript:clickScore('" + RTID + "','" + RID + "','" + CallID + "','" + tableEndName + "','" + scoreType + "')\" href=\"javascript:void(0);\" >评分</a>&nbsp;";
            }
            else
            {
                return "";
            }
        }

        public string GetViewUrl(string qs_rid, string scoreType)
        {
            if (scoreType == "3")
            {
                return "/QualityStandard/QualityResultManage/QualityResultFiveLevelView.aspx?pagefrom=cc&QS_RID=" + qs_rid + "&tableEndName=" + tableEndName;
            }
            else
            {
                return "/QualityStandard/QualityResultManage/QualityResultView.aspx?pagefrom=cc&QS_RID=" + qs_rid + "&tableEndName=" + tableEndName;
            }
        }

        /// 获取分数显示信息
        public string GetScoreToView(string scoreType, string score, string isQualified, string scoreStatusName)
        {
            if (scoreStatusName == "待评分")
            {
                return "&nbsp;";
            }
            else
            {
                if (scoreType == "1" || scoreType == "3")
                {
                    if (score.Replace(".0", "") == "-2" || score.Replace(".0", "") == "-1")
                    {
                        return "&nbsp;";
                    }
                    else
                    {
                        return score.Replace(".0", "") + "&nbsp;";
                    }
                }
                else
                {
                    if (isQualified == "1")
                    {
                        return "合格";
                    }
                    else if (isQualified == "-1")
                    {
                        return "不合格";
                    }
                    else
                    {
                        return "&nbsp;";
                    }

                }
            }
        }
    }
}

