using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.QualityScoring.Export
{
    public partial class ScoreDetailsExport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        private string RequestPhoneNum
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("PhoneNum")); }
        }
        private string RequestBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BeginTime")); }
        }
        private string RequestEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("EndTime")); }
        }
        private string RequestBGID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BGID")); }
        }
        private string RequestSCID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SCID")); }
        }
        private string RequestANI
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ANI")); }
        }
        private string RequestSpanTime1
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SpanTime1")); }
        }
        private string RequestSpanTime2
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SpanTime2")); }
        }
        private string RequestCreateUserID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CreateUserID")); }
        }
        private string RequestBusinessID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("BusinessID")); }
        }
        private string RequestScoreBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ScoreBeginTime")); }
        }
        private string RequestScoreEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ScoreEndTime")); }
        }
        private string RequestScoreTable
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ScoreTable")); }
        }
        private string RequestCallID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CallID")); }
        }
        private string RequestAppealBeginTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("AppealBeginTime")); }
        }
        private string RequestAppealEndTime
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("AppealEndTime")); }
        }
        private string RequestScoreCreater
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ScoreCreater")); }
        }
        private string RequestCallType
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CallType")); }
        }
        private string RequestQSResultStatus
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QSResultStatus")); }
        }
        private string RequestQSStateResult
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QSStateResult")); }
        }
        private string RequestCallAgentBGID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("CallAgentBGID")); }
        }
        private string RequestBrowser
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Browser"));
            }
        }

        //private string RequestQualified
        //{
        //    get
        //    {
        //        return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Qualified"));
        //    }
        //}
        private string RequestQSResultScore
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QSResultScore"));
            }
        }
        #endregion

        private Entities.QueryCallRecordInfo GetQuery()
        {
            Entities.QueryCallRecordInfo query = new Entities.QueryCallRecordInfo();
            if (RequestPhoneNum != "")
            {
                query.PhoneNum = RequestPhoneNum;
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = DateTime.Parse(RequestBeginTime);
            }
            if (RequestEndTime != "")
            {
                query.EndTime = DateTime.Parse(RequestEndTime);
            }
            int bgid = 0;
            if (RequestBGID != "" && int.TryParse(RequestBGID, out bgid))
            {
                query.BGID = bgid;
            }
            int scid = 0;
            if (RequestSCID != "" && int.TryParse(RequestSCID, out scid))
            {
                query.SCID = scid;
            }
            if (RequestANI != "")
            {
                query.ANI = RequestANI;
            }
            int spanTime1 = 0;
            if (RequestSpanTime1 != "" && int.TryParse(RequestSpanTime1, out spanTime1))
            {
                query.SpanTime1 = spanTime1;
            }
            int spanTime2 = 0;
            if (RequestSpanTime2 != "" && int.TryParse(RequestSpanTime2, out spanTime2))
            {
                query.SpanTime2 = spanTime2;
            }
            int createUserid = 0;
            if (RequestCreateUserID != "" && int.TryParse(RequestCreateUserID, out createUserid))
            {
                query.CreateUserID = createUserid;
            }
            if (RequestBusinessID != "")
            {
                query.BusinessID = RequestBusinessID;
            }
            if (RequestScoreBeginTime != "")
            {
                query.ScoreBeginTime = RequestScoreBeginTime;
            }
            if (RequestScoreEndTime != "")
            {
                query.ScoreEndTime = RequestScoreEndTime;
            }
            int scoreTable = 0;
            if (RequestScoreTable != "" && int.TryParse(RequestScoreTable, out scoreTable))
            {
                query.ScoreTable = scoreTable;
            }
            long callid = 0;
            if (RequestCallID != "" && long.TryParse(RequestCallID, out callid))
            {
                query.CallID = callid;
            }
            if (RequestAppealBeginTime != "")
            {
                query.AppealBeginTime = RequestAppealBeginTime;
            }
            if (RequestAppealEndTime != "")
            {
                query.AppealEndTime = RequestAppealEndTime;
            }
            int screaterid = 0;
            if (RequestScoreCreater != "" && int.TryParse(RequestScoreCreater, out screaterid))
            {
                query.ScoreCreater = screaterid;
            }
            if (RequestCallType != "")
            {
                query.CallType = RequestCallType;
            }
            if (RequestQSResultStatus != "")
            {
                query.QSResultStatus = RequestQSResultStatus;
            }
            if (RequestQSStateResult != "")
            {
                query.QSStateResult = RequestQSStateResult;
            }
            //if (RequestQualified != "")
            //{
            //    query.Qualified = RequestQualified;
            //}
            if (RequestQSResultScore != "")
            {
                query.QSResultScore = RequestQSResultScore;
            }
            query.CallAgentBGID = RequestCallAgentBGID;
            query.LoginID = BLL.Util.GetLoginUserID();
            query.IsFilterNull = 1;
            return query;
        }
        string scoretype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT600105"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }

                int _qs_rtid;
                if (!int.TryParse(RequestScoreTable, out _qs_rtid))
                {
                    return;
                }
                scoretype = getTableType(_qs_rtid);//1-评分型；2-合格型
                string resultStr = string.Empty;
                switch (scoretype)
                {
                    case "1":
                    case "3":
                        resultStr = ExportByRatingType();
                        break;
                    case "2":
                        resultStr = ExportByQualifiedType();
                        break;
                }

                bool isIE = false;
                if (RequestBrowser == "IE")
                {
                    isIE = true;
                }

                ExportAnswerDetail(resultStr, "录音质检结果", isIE);
            }
        }

        //获取该评分表的评分表类型；1-评分型；2-合格型
        private string getTableType(int rtid)
        {
            string type = string.Empty;

            Entities.QS_RulesTable model = BLL.QS_RulesTable.Instance.GetQS_RulesTable(rtid);
            if (model != null)
            {
                type = model.ScoreType.ToString();
            }

            return type;
        }

        //评分型导出
        private string ExportByRatingType()
        {
            int qs_rtid = int.Parse(RequestScoreTable);
            StringBuilder returnStr = new StringBuilder();
            returnStr.Append("<table borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
            DataTable dtCategory = BLL.QS_Category.Instance.GetQS_CategoryNameAndItemNum(qs_rtid);

            #region 表头
            returnStr.Append("<tr>");

            string rowspan = "";
            if (dtCategory != null && dtCategory.Rows.Count > 0) rowspan = " rowspan='2' ";

            returnStr.Append("<td " + rowspan + " >序号</td>");
            returnStr.Append("<td " + rowspan + " >通话时间</td>");
            returnStr.Append("<td " + rowspan + " >评分时间</td>");
            returnStr.Append("<td " + rowspan + " >员工姓名</td>");
            returnStr.Append("<td " + rowspan + " >录音ID</td>");
            returnStr.Append("<td " + rowspan + " >通话时长(秒)</td>");
            returnStr.Append("<td " + rowspan + " >分组</td>");
            returnStr.Append("<td " + rowspan + " >分类</td>");
            returnStr.Append("<td " + rowspan + " >任务ID</td>");
            returnStr.Append("<td " + rowspan + " >任务状态</td>");

            #region 评分分类名称
            if (dtCategory != null)
            {
                foreach (DataRow dr in dtCategory.Rows)
                {
                    returnStr.Append("<td colspan=" + dr["ItemNum"] + " style='text-align:center'>" + dr["Name"] + "</td>");
                }
            }
            #endregion
            returnStr.Append("<td " + rowspan + " >致命项数</td>");
            returnStr.Append("<td " + rowspan + " >总分</td>");
            returnStr.Append("<td " + rowspan + " >质检点评</td>");

            returnStr.Append("</tr>");
            #endregion

            #region 评分项
            Entities.QueryQS_Item itemQuery = new Entities.QueryQS_Item();
            itemQuery.QS_RTID = qs_rtid;
            int itemTotalCount = 0;
            DataTable dtItem = BLL.QS_Item.Instance.GetQS_Item(itemQuery, "QS_IID ASC", 1, -1, out itemTotalCount);
            if (itemTotalCount > 0)
            {
                returnStr.Append("<tr>");
                foreach (DataRow dr in dtItem.Rows)
                {
                    returnStr.Append("<td>" + dr["ItemName"] + "</td>");
                }
                returnStr.Append("</tr>");
            }
            #endregion

            #region 输出统计结果
            DataTable dtDetails = GetResultDetail(qs_rtid);
            DataTable dtResult = GetResult();

            for (int j = 0; j < dtResult.Rows.Count; j++)
            {
                returnStr.Append("<tr>");
                DataRow dr = dtResult.Rows[j];
                //序号
                returnStr.Append("<td>" + (j + 1) + "</td>");
                //通话时间
                DateTime dt;
                if (DateTime.TryParse(dr["CallTime"].ToString(), out dt))
                {
                    returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + DateTime.Parse(dr["CallTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                }
                else
                {
                    returnStr.Append("<td></td>");
                }
                //评分时间
                if (DateTime.TryParse(dr["CreateTime"].ToString(), out dt))
                {
                    returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + DateTime.Parse(dr["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                }
                else
                {
                    returnStr.Append("<td></td>");
                }
                //坐席
                returnStr.Append("<td>" + dr["TrueName"] + "</td>");
                returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + dr["CallID"] + "</td>");
                returnStr.Append("<td>" + dr["TallTime"].ToString() + "</td>");
                returnStr.Append("<td>" + dr["bgName"] + "</td>");
                returnStr.Append("<td>" + dr["scName"] + "</td>");
                returnStr.Append("<td>" + dr["BusinessID"] + "</td>");
                returnStr.Append("<td>" + dr["ScoreStatusName"] + "</td>");
                string qsrid = dtResult.Rows[j]["QS_RID"].ToString();
                decimal totalScore = 100;
                if (scoretype == "3")
                {
                    totalScore = 0;
                }
                int deadNum = 0;
                for (int k = 0; k < dtItem.Rows.Count; k++)
                {
                    string qsiid = dtItem.Rows[k]["QS_IID"].ToString();
                    if (qsrid != "" && qsiid != "")
                    {
                        DataRow[] resultItem = dtDetails.Select(" QS_RID=" + qsrid + " and QS_IID=" + qsiid);
                        if (resultItem.Length > 0)
                        {
                            decimal enscore = 0;
                            if (scoretype == "3")
                            {
                                for (int p = 0; p < resultItem.Length; p++)
                                {
                                    if (resultItem[p]["Score"] != null && resultItem[p]["Score"].ToString() != string.Empty)
                                    {
                                        //单项扣的分数和
                                        enscore += decimal.Parse(resultItem[p]["Score"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                for (int p = 0; p < resultItem.Length; p++)
                                {
                                    if (resultItem[p]["enscore"] != null && resultItem[p]["enscore"].ToString() != string.Empty)
                                    {
                                        //单项扣的分数和
                                        enscore += decimal.Parse(resultItem[p]["enscore"].ToString());
                                    }
                                }
                                //比较这个总和是否大于整个选项的分值，如果扣分大于选项最大值，则取最大选项值
                                decimal itemMaxScore = decimal.Parse(dtItem.Rows[k]["Score"].ToString());
                                if (System.Math.Abs(enscore) > itemMaxScore)
                                {
                                    enscore = itemMaxScore * -1;
                                }
                            }
                            returnStr.Append("<td>" + enscore + "</td>");
                            totalScore += enscore;
                        }
                        else
                        {
                            //没有值则单项为空
                            returnStr.Append("<td></td>");
                            //判断是不是有致命项
                            DataRow[] resultItem1 = dtDetails.Select(" QS_RID=" + qsrid + " and QS_IID=-2");
                            if (resultItem1.Length > 0)
                            {
                                if (resultItem1[0]["deadNum"] != null && resultItem1[0]["deadNum"].ToString() != string.Empty)
                                {
                                    deadNum = int.Parse(resultItem1[0]["deadNum"].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        returnStr.Append("<td></td>");
                    }
                }
                string statusName = dtResult.Rows[j]["ScoreStatusName"].ToString();
                //是否有致命项
                if (statusName != "待评分")
                {
                    returnStr.Append("<td>" + deadNum + "</td>");
                }
                else
                {
                    returnStr.Append("<td></td>");
                }

                if (deadNum == 1)
                {
                    //有致命项，分数直接为0
                    returnStr.Append("<td>0</td>");
                }
                else
                {
                    //否则分数为统计分数
                    if (statusName != "待评分")
                    {
                        returnStr.Append("<td>" + totalScore + "</td>");
                    }
                    else
                    {
                        returnStr.Append("<td></td>");
                    }
                }
                //质检评价
                returnStr.Append("<td>" + dr["QualityAppraisal"] + "</td>");
                returnStr.Append("</tr>");
            }

            #endregion

            returnStr.Append("</table>");
            return returnStr.ToString();
        }
        /// 获取结果
        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns></returns>
        private DataTable GetResult()
        {
            Entities.QueryCallRecordInfo query = GetQuery();
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dtResult = BLL.QS_Result.Instance.getResultByExport(query, tableEndName);
            return dtResult;
        }
        /// 获取明细数据
        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="qs_rtid"></param>
        /// <returns></returns>
        private DataTable GetResultDetail(int qs_rtid)
        {
            string whereDetail = string.Empty;
            if (RequestBeginTime != "")
            {
                whereDetail += " and cob.CreateTime>= '" + RequestBeginTime + "'";
            }
            if (RequestEndTime != "")
            {
                whereDetail += " and cob.CreateTime<= '" + RequestEndTime + "'";
            }
            if (RequestScoreCreater != "" && RequestScoreCreater != "-1")
            {
                whereDetail += " and a.CreateUserID = " + RequestScoreCreater;
            }
            whereDetail += " and a.QS_RTID=" + qs_rtid;
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dtDetails = BLL.QS_ResultDetail.Instance.getDetailsByExport(whereDetail, tableEndName);
            return dtDetails;
        }

        //合格型导出
        private string ExportByQualifiedType()
        {
            int _qs_rtid = int.Parse(RequestScoreTable);
            DataTable dt_BaseInfo = GetQuaResult();
            DataTable dt_ResultDetail = GetQuaResultDetail(_qs_rtid);

            DataTable newDt = new DataTable();
            int apprisalCount = 0;//质检评价数
            //需要导出的table
            StringBuilder sbTableStr = new StringBuilder();

            sbTableStr.Append("<table style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
            sbTableStr.Append("<tr>");
            string[] columnStr = "通话时间,评分时间,姓名,录音ID,通话时长(秒),所属分组,分类,任务ID,是否合格,致命错误数,非致命错误数".Split(',');
            for (int i = 0; i < columnStr.Length; i++)
            {
                //前面基础信息列
                sbTableStr.Append("<td style='background-color:Orange'>" + columnStr[i] + "</td>");
            }

            Entities.QueryQS_Result query_result = new Entities.QueryQS_Result();
            query_result.QS_RTID = _qs_rtid;
            DataTable dt_Standard = BLL.QS_Result.Instance.GetStandardByQualifiedType(query_result);
            DataRow[] dr_Dead = dt_Standard.Select("IsIsDead=1");//致命项 放在一起
            for (int i = 0; i < dr_Dead.Length; i++)
            {
                //致命项列
                sbTableStr.Append("<td style='background-color:Yellow'>" + dr_Dead[i]["ScoringStandardName"].ToString() + "</td>");
            }
            DataRow[] dr_NoDead = dt_Standard.Select("IsIsDead=0");//非致命项 放在一起
            for (int i = 0; i < dr_NoDead.Length; i++)
            {
                //非致命项列
                sbTableStr.Append("<td style='background-color:Green'>" + dr_NoDead[i]["ScoringStandardName"].ToString() + "</td>");
            }
            //质检评价
            if (dt_Standard.Rows.Count > 0)
            {
                if (dt_Standard.Rows[0]["HaveQAppraisal"].ToString() == "1")
                {
                    sbTableStr.Append("<td style='background-color:Gray'>质检评价</td>");
                    apprisalCount = 1;
                }
            }
            sbTableStr.Append("</tr>");


            //根据质检成绩表ID循环得到答案
            for (int i = 0; i < dt_BaseInfo.Rows.Count; i++)
            {
                sbTableStr.Append("<tr>");
                //基础信息
                string CallTime = string.Empty;
                DateTime dtime;
                if (DateTime.TryParse(dt_BaseInfo.Rows[i]["CallTime"].ToString(), out dtime))
                {
                    CallTime = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                string TrueName = dt_BaseInfo.Rows[i]["TrueName"].ToString();
                string GroupName = dt_BaseInfo.Rows[i]["GroupName"].ToString();
                string CategoryName = dt_BaseInfo.Rows[i]["CategoryName"].ToString();
                DateTime dtCTime;
                string CreateTime = string.Empty;
                if (DateTime.TryParse(dt_BaseInfo.Rows[i]["CreateTime"].ToString(), out dtCTime))
                {
                    CreateTime = dtCTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                string TallTime = dt_BaseInfo.Rows[i]["TallTime"].ToString();
                string CallRecordID = dt_BaseInfo.Rows[i]["CallID"].ToString();
                string TaskID = dt_BaseInfo.Rows[i]["BusinessID"].ToString();
                string IsQualifiedStr = dt_BaseInfo.Rows[i]["IsQualifiedStr"].ToString();
                string DeadNum = dt_BaseInfo.Rows[i]["DeadNum"].ToString();
                string NotDeadNum = dt_BaseInfo.Rows[i]["NoDeadNum"].ToString();
                sbTableStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + CallTime + "</td><td style='vnd.ms-excel.numberformat:@'>" + CreateTime + "</td>" + "<td>" + TrueName + "</td>" + "<td  style='vnd.ms-excel.numberformat:@'>" + CallRecordID + "</td>" + "<td>" + TallTime + "</td>" + "<td>" + GroupName + "</td>" + "<td>" + CategoryName + "</td>" + "<td>" + TaskID + "</td>" + "<td>" + IsQualifiedStr + "</td>" + "<td>" + DeadNum + "</td>" + "<td>" + NotDeadNum + "</td>");

                string answer = string.Empty;

                string base_QSRID = dt_BaseInfo.Rows[i]["QS_RID"].ToString().Trim();
                for (int k = 0; k < dr_Dead.Length; k++)
                {
                    DataRow[] dr_standard = null;
                    if (dt_ResultDetail.Rows.Count > 0 && base_QSRID != "")
                    {
                        //评分详细信息
                        dr_standard = dt_ResultDetail.Select("QS_RID=" + base_QSRID + " and QS_CID=" + dr_Dead[k]["QS_CID"].ToString() + " and QS_IID=" + dr_Dead[k]["QS_IID"].ToString() + " and QS_SID=" + dr_Dead[k]["QS_SID"].ToString());
                    }

                    if (dr_standard != null && dr_standard.Length > 0)
                    {
                        //如果为-2，则是更新过的，否则肯定是有值
                        sbTableStr.Append("<td>" + (dr_standard[0]["QS_MID_End"].ToString() != "-2" ?
                            "1" : string.Empty) + "</td>");
                    }
                    else
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                for (int k = 0; k < dr_NoDead.Length; k++)
                {
                    DataRow[] dr_standard = null;
                    if (dt_ResultDetail.Rows.Count > 0 && base_QSRID != "")
                    {
                        //评分详细信息
                        dr_standard = dt_ResultDetail.Select("QS_RID=" + base_QSRID + " and QS_CID=" + dr_NoDead[k]["QS_CID"].ToString() + " and QS_IID=" + dr_NoDead[k]["QS_IID"].ToString() + " and QS_SID=" + dr_NoDead[k]["QS_SID"].ToString());
                    }

                    if (dr_standard != null && dr_standard.Length > 0)
                    {
                        //如果为-2，则是更新过的，否则肯定是有值
                        sbTableStr.Append("<td>" + (dr_standard[0]["QS_MID_End"].ToString() != "-2" ?
                            "1" : string.Empty) + "</td>");
                    }
                    else
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                //如果有质检评价，显示出来
                if (apprisalCount == 1)
                {
                    sbTableStr.Append("<td>" + dt_BaseInfo.Rows[i]["QualityAppraisal"].ToString() + "</td>");
                }

                sbTableStr.Append("</tr>");
            }

            sbTableStr.Append("</table>");
            return sbTableStr.ToString();

        }

        private DataTable GetQuaResultDetail(int _qs_rtid)
        {
            Entities.QueryQS_ResultDetail query_detail = new Entities.QueryQS_ResultDetail();
            query_detail.QS_RTID = _qs_rtid;
            if (RequestScoreBeginTime != "")
            {
                query_detail.BeginTime = RequestScoreBeginTime;
            }
            if (RequestScoreEndTime != "")
            {
                query_detail.EndTime = RequestScoreEndTime;
            }
            if (RequestScoreCreater != "" && RequestScoreCreater != "-1")
            {
                query_detail.CreateUserID = int.Parse(RequestScoreCreater);
            }
            if (RequestBeginTime != "")
            {
                query_detail.CallBeginTime = RequestBeginTime;
            }
            if (RequestEndTime != "")
            {
                query_detail.CallEndTime = RequestEndTime;
            }
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt_ResultDetail = BLL.QS_Result.Instance.GetAnswerByQualifiedType(query_detail, tableEndName);
            return dt_ResultDetail;
        }

        private DataTable GetQuaResult()
        {
            Entities.QueryCallRecordInfo query = GetQuery();
            string tableEndName = "_QS";//查询质检话务冗余表
            DataTable dt_BaseInfo = BLL.QS_Result.Instance.GetBaseInfoByQualifiedType(query, tableEndName);
            return dt_BaseInfo;
        }

        //导出
        public void ExportAnswerDetail(string strContent, string TrueName, bool IsIE)
        {
            Encoding en = Encoding.GetEncoding("gb2312");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            if (IsIE)
            {
                Response.AddHeader("Content-Disposition", "inline;filename=\"" + System.Web.HttpUtility.UrlEncode(TrueName, System.Text.Encoding.UTF8) + ".xls\"");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "inline;filename=\"" + TrueName + ".xls\"");
            }

            Response.ContentEncoding = System.Text.Encoding.UTF8;//表格内容添加编码格式
            //Response.HeaderEncoding = System.Text.Encoding.UTF8;//表头添加编码格式
            Response.ContentType = "application/vnd.ms-excel.numberformat:@";
            Page.EnableViewState = false;
            Response.Write(strContent);
            Response.Flush();
            Response.End();
        }
    }
}