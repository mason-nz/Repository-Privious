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
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.Export
{
    public partial class IMScoreDetailsExport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string Params
        {
            get
            {
                try
                {
                    return HttpContext.Current.Request.QueryString.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        private string RequestBrowser
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Browser"));
            }
        }

        QueryQS_IM_Result query = new QueryQS_IM_Result();
        int[] nums = null;
        string scoretype = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT600401"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                //解析参数
                string errMsg = string.Empty;
                BLL.ConverToEntitie<QueryQS_IM_Result> conver = new BLL.ConverToEntitie<QueryQS_IM_Result>(query);
                errMsg = conver.Conver(Params);
                if (errMsg != "")
                {
                    return;
                }
                //参数校验处理（必须）
                query.LoginUerID = BLL.Util.GetLoginUserID();
                query.InitCheck();

                //浏览器
                bool isIE = false;
                if (RequestBrowser == "IE")
                {
                    isIE = true;
                }

                //校验
                if (!string.IsNullOrEmpty(query.ScoreTable) && query.ScoreTable != "-1")
                {
                    int _qs_rtid = CommonFunction.ObjectToInteger(query.ScoreTable);
                    //1-评分型；2-合格型;3.五级质检
                    scoretype = getTableType(_qs_rtid, out nums);
                    string resultStr = string.Empty;
                    switch (scoretype)
                    {
                        case "1":
                        case "3":
                            resultStr = ExportByRatingType(_qs_rtid);
                            break;
                        case "2":
                            resultStr = ExportByQualifiedType(_qs_rtid);
                            break;
                    }

                    ExportAnswerDetail(resultStr, "对话质检结果", isIE);
                }
            }
        }

        //获取该评分表的评分表类型；1-评分型；2-合格型;3.五级质检
        private string getTableType(int rtid, out int[] nums)
        {
            nums = new int[2];
            string type = string.Empty;
            Entities.QS_RulesTable model = BLL.QS_RulesTable.Instance.GetQS_RulesTable(rtid);
            if (model != null)
            {
                type = model.ScoreType.ToString();
                nums[0] = model.DeadItemNum.HasValue ? model.DeadItemNum.Value : 0;
                nums[1] = model.NoDeadItemNum.HasValue ? model.NoDeadItemNum.Value : 0;
            }
            return type;
        }

        //评分型导出
        private string ExportByRatingType(int qs_rtid)
        {
            StringBuilder returnStr = new StringBuilder();
            returnStr.Append("<table borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
            DataTable dtCategory = BLL.QS_Category.Instance.GetQS_CategoryNameAndItemNum(qs_rtid);

            #region 表头
            returnStr.Append("<tr>");
            string rowspan = "";
            if (dtCategory != null && dtCategory.Rows.Count > 0)
                rowspan = " rowspan='2' ";
            returnStr.Append("<td " + rowspan + " >序号</td>");
            returnStr.Append("<td " + rowspan + " >对话时间</td>");
            returnStr.Append("<td " + rowspan + " >评分时间</td>");
            returnStr.Append("<td " + rowspan + " >员工姓名</td>");
            returnStr.Append("<td " + rowspan + " >对话ID</td>");
            returnStr.Append("<td " + rowspan + " >消息次数</td>");
            returnStr.Append("<td " + rowspan + " >所属分组</td>");
            //returnStr.Append("<td " + rowspan + " >分类</td>");
            returnStr.Append("<td " + rowspan + " >工单ID</td>");
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
                DateTime dt;
                //对话时间
                if (DateTime.TryParse(dr["BeginTime"].ToString(), out dt))
                {
                    returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                }
                else
                {
                    returnStr.Append("<td></td>");
                }
                //评分时间
                if (DateTime.TryParse(dr["Result_CreateTime"].ToString(), out dt))
                {
                    returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
                }
                else
                {
                    returnStr.Append("<td></td>");
                }
                //坐席
                returnStr.Append("<td>" + dr["AgentUserName"] + "</td>");
                //会话id
                returnStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + dr["CSID"] + "</td>");
                //消息次数
                returnStr.Append("<td>" + dr["Count"].ToString() + "</td>");
                //所属分组
                returnStr.Append("<td>" + dr["BGName"] + "</td>");
                //工单ID
                returnStr.Append("<td>" + dr["OrderID"] + "</td>");
                //任务状态
                string statusname = BLL.Util.GetEnumOptText(typeof(QSResultStatus), CommonFunction.ObjectToInteger(dr["Result_Status"]));
                returnStr.Append("<td>" + statusname + "</td>");

                //成绩明细
                string qsrid = dr["QS_RID"].ToString();
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
                //是否有致命项
                if (statusname != "待评分")
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
                    if (statusname != "待评分")
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
            int RecordCount = 0;
            //查询数据
            DataTable dt = BLL.QS_IM_Result.Instance.GetQS_IM_Result(query, "", 1, -1, out RecordCount);
            return dt;
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
            whereDetail += " and a.QS_RTID=" + qs_rtid;

            if (!string.IsNullOrEmpty(query.ScoreBeginTime) || !string.IsNullOrEmpty(query.ScoreEndTime))
            {
                whereDetail += " AND a.QS_RID IN (SELECT QS_RID FROM dbo.QS_IM_Result q WHERE 1=1";

                if (!string.IsNullOrEmpty(query.ScoreBeginTime) && !string.IsNullOrEmpty(query.ScoreEndTime))
                {
                    whereDetail += " and ((q.CreateTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime) + "' "
                                        + " and q.CreateTime<='" + StringHelper.SqlFilter(query.ScoreEndTime) + "') "
                                        + " or (q.ModifyTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime) + "' "
                                             +" and q.ModifyTime<='" + StringHelper.SqlFilter(query.ScoreEndTime) + "')) ";
                                     
                }
                else if (!string.IsNullOrEmpty(query.ScoreBeginTime))
                {
                    whereDetail += " and (q.CreateTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime)
                        + "' or q.ModifyTime>='" + StringHelper.SqlFilter(query.ScoreBeginTime) + "') ";
                }
                else if (!string.IsNullOrEmpty(query.ScoreEndTime))
                {
                    whereDetail += " and (q.CreateTime<='" + StringHelper.SqlFilter(query.ScoreEndTime)
                        + "' or q.ModifyTime<='" + StringHelper.SqlFilter(query.ScoreEndTime) + "') ";
                }
                whereDetail += ")";
            }

            return BLL.QS_ResultDetail.Instance.getDetailsByExport_IM(whereDetail);
        }

        //合格型导出
        private string ExportByQualifiedType(int qs_rtid)
        {
            #region 表头
            //需要导出的table
            StringBuilder sbTableStr = new StringBuilder();
            int apprisalCount = 0;//质检评价数
            sbTableStr.Append("<table style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
            sbTableStr.Append("<tr>");
            string[] columnStr = "对话时间,评分时间,姓名,对话ID,消息次数,所属分组,工单ID,是否合格,致命错误数,非致命错误数".Split(',');
            for (int i = 0; i < columnStr.Length; i++)
            {
                //前面基础信息列
                sbTableStr.Append("<td style='background-color:Orange'>" + columnStr[i] + "</td>");
            }

            Entities.QueryQS_Result query_result = new Entities.QueryQS_Result();
            query_result.QS_RTID = qs_rtid;
            DataTable dt_Standard = BLL.QS_Result.Instance.GetStandardByQualifiedType(query_result);
            DataRow[] dr_Dead = dt_Standard.Select("IsIsDead=1");
            //致命项 放在一起
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
            #endregion

            #region 输出统计结果
            DataTable dt_BaseInfo = GetResult();
            DataTable dt_ResultDetail = GetQuaResultDetail(qs_rtid);

            //根据质检成绩表ID循环得到答案
            for (int i = 0; i < dt_BaseInfo.Rows.Count; i++)
            {
                sbTableStr.Append("<tr>");
                //对话时间
                string BeginTime = string.Empty;
                DateTime dtime;
                if (DateTime.TryParse(dt_BaseInfo.Rows[i]["BeginTime"].ToString(), out dtime))
                {
                    BeginTime = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                //评分时间
                string CreateTime = string.Empty;
                if (DateTime.TryParse(dt_BaseInfo.Rows[i]["Result_CreateTime"].ToString(), out dtime))
                {
                    CreateTime = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                //坐席
                string TrueName = dt_BaseInfo.Rows[i]["AgentUserName"].ToString();
                //会话ID
                string CSID = dt_BaseInfo.Rows[i]["CSID"].ToString();
                //消息次数
                string Count = dt_BaseInfo.Rows[i]["Count"].ToString();
                //所属分组
                string GroupName = dt_BaseInfo.Rows[i]["BGName"].ToString();
                //工单ID
                string OrderID = dt_BaseInfo.Rows[i]["OrderID"].ToString();

                //是否合格
                string IsQualifiedStr = "";
                if (dt_BaseInfo.Rows[i]["IsQualified"].ToString() == "1")
                {
                    IsQualifiedStr = "合格";
                }
                else if (dt_BaseInfo.Rows[i]["IsQualified"].ToString() == "-1")
                {
                    IsQualifiedStr = "不合格";
                }
                //致命项
                string DeadNum = nums[0].ToString();
                //非致命项
                string NotDeadNum = nums[1].ToString();

                sbTableStr.Append("<td style='vnd.ms-excel.numberformat:@'>" +
                    BeginTime + "</td><td style='vnd.ms-excel.numberformat:@'>" +
                    CreateTime + "</td>" + "<td>" +
                    TrueName + "</td>" + "<td  style='vnd.ms-excel.numberformat:@'>" +
                    CSID + "</td>" + "<td>" +
                    Count + "</td>" + "<td>" +
                    GroupName + "</td>" + "<td>" +
                    OrderID + "</td>" + "<td>" +
                    IsQualifiedStr + "</td>" + "<td>" +
                    DeadNum + "</td>" + "<td>" +
                    NotDeadNum + "</td>");

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
            #endregion

            sbTableStr.Append("</table>");
            return sbTableStr.ToString();
        }

        /// 查询详细信息
        /// <summary>
        /// 查询详细信息
        /// </summary>
        /// <param name="qs_rtid"></param>
        /// <returns></returns>
        private DataTable GetQuaResultDetail(int qs_rtid)
        {
            string whereDetail = string.Empty;
            whereDetail += " and a.QS_RTID=" + qs_rtid;

            if (!string.IsNullOrEmpty(query.BeginTime) && !string.IsNullOrEmpty(query.EndTime))
            {
                whereDetail += " AND a.QS_RID IN (SELECT QS_RID FROM dbo.QS_IM_Result q WHERE 1=1";
                if (!string.IsNullOrEmpty(query.BeginTime))
                {
                    whereDetail += " and q.CreateTime>='" + query.BeginTime + "' ";
                }
                if (!string.IsNullOrEmpty(query.EndTime))
                {
                    whereDetail += " and q.CreateTime<='" + query.EndTime + "' ";
                }
                whereDetail += "  ) ";
            }

            return BLL.QS_Result.Instance.GetAnswerByQualifiedType_IM(whereDetail);
        }

        //导出
        public void ExportAnswerDetail(string strContent, string TrueName, bool IsIE)
        {
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
            Response.HeaderEncoding = System.Text.Encoding.UTF8;//表头添加编码格式
            Response.ContentType = "application/vnd.ms-excel.numberformat:@";
            Page.EnableViewState = false;
            Response.Write(strContent);
            Response.Flush();
            Response.End();
        }
    }
}