using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject
{
    public partial class ExportExcel : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string SPIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SPIID"));
            }
        }
        private string RequestBrowser
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Browser"));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024BUT5010") || BLL.Util.CheckRight(userID, "SYS024MOD3607"))
                {
                    string resultStr = GetAnswerDetailResultTableStr();
                    bool isIE = false;
                    if (RequestBrowser == "IE")
                    {
                        isIE = true;
                    }
                    ExportAnswerDetail(resultStr, "员工答题详细", isIE);
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private string GetAnswerDetailResultTableStr()
        {
            int spiid = 0;
            StringBuilder sbTableStr = new StringBuilder();

            if (int.TryParse(SPIID, out spiid))
            {

                Entities.SurveyProjectInfo info = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(spiid);
                if (info != null)
                {

                    Entities.QuerySurveyOption queryAllOption = new Entities.QuerySurveyOption();
                    queryAllOption.SIID = info.SIID;
                    queryAllOption.Status = 0;
                    int totalAllOption = 0;
                    //获取试题下所有选项
                    DataTable dtAllOption = BLL.SurveyOption.Instance.GetSurveyOption(queryAllOption, "", 1, -1, out totalAllOption);

                    sbTableStr.Append("<table style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");
                    DataTable dt = BLL.SurveyAnswer.Instance.GetAnswerDetailBySPIID(spiid);
                    if (dt.Rows.Count > 0)
                    {
                        //记录非矩阵题的选项个数
                        int totalCount = 0;
                        //记录单选题选项个数
                        int scoreQuestionCount = 0;
                        int scoreOptionCount = 0;
                        //记录单选题选项个数
                        int radioQuestionCount = 0;
                        int radioOptionCount = 0;
                        //记录多选题选项个数
                        int checkQuestionCount = 0;
                        int checkOptionCount = 0;
                        //记录文本题选项个数
                        int textQuestionCount = 0;
                        //记录矩阵单选选项个数
                        int matrixRadioQuestionCount = 0;
                        int matrixRadioOptionCount = 0;

                        //记录矩阵下拉横坐标个数
                        int matrixDQuestionCount = 0;
                        int matrixDOptionCount = 0;

                        #region 输出试题名称
                        sbTableStr.Append("<tr>");
                        sbTableStr.Append("<td>姓名</td><td>时间</td>");

                        Entities.QuerySurveyQuestion queryQuestion = new Entities.QuerySurveyQuestion();
                        queryQuestion.SIID = (int)info.SIID;
                        queryQuestion.AskCategory = 1;
                        queryQuestion.IsStatByScore = 1;
                        queryQuestion.Status = 0;
                        DataTable dtScoreQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out scoreQuestionCount);

                        //单选试题名称行(按分数统计)
                        foreach (DataRow dr in dtScoreQuestion.Rows)
                        {
                            //int optionCount= BLL.SurveyOption.Instance.GetSurveyOptionCountBySQID(int.Parse(dr["SQID"].ToString()));
                            sbTableStr.Append("<td align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                        }
                        scoreOptionCount = scoreQuestionCount;
                        totalCount += scoreOptionCount;

                        queryQuestion.AskCategory = 1;
                        queryQuestion.IsStatByScore = 0;
                        DataTable dtRadioQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out radioQuestionCount);

                        //单选试题名称行(不按分数统计)
                        foreach (DataRow dr in dtRadioQuestion.Rows)
                        {
                            //int optionCount = BLL.SurveyOption.Instance.GetSurveyOptionCountBySQID(int.Parse(dr["SQID"].ToString()));
                            sbTableStr.Append("<td align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                        }

                        radioOptionCount = radioQuestionCount;
                        totalCount += radioOptionCount;

                        queryQuestion.AskCategory = 2;
                        DataTable dtCheckQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out checkQuestionCount);

                        //拼接多选试题名称行
                        foreach (DataRow dr in dtCheckQuestion.Rows)
                        {
                            int optionCount = BLL.SurveyOption.Instance.GetSurveyOptionCountBySQID(int.Parse(dr["SQID"].ToString()));
                            checkOptionCount += optionCount;
                            sbTableStr.Append("<td colspan='" + optionCount + "' align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                            totalCount += checkOptionCount;
                        }

                        queryQuestion.AskCategory = 3;
                        DataTable dtTextQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out textQuestionCount);
                        totalCount += textQuestionCount;
                        //拼接文本试题名称行
                        foreach (DataRow dr in dtTextQuestion.Rows)
                        {
                            sbTableStr.Append("<td align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                        }

                        queryQuestion.AskCategory = 4;
                        DataTable dtMatrixRQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out matrixRadioQuestionCount);
                        int rTitleCount = 0;
                        //拼接矩阵单选试题名称行
                        foreach (DataRow dr in dtMatrixRQuestion.Rows)
                        {
                            rTitleCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 1);
                            sbTableStr.Append("<td colspan='" + rTitleCount + "' align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                            matrixRadioOptionCount += rTitleCount;
                        }

                        queryQuestion.AskCategory = 5;
                        DataTable dtMatrixDQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(queryQuestion, " SQID asc", 1, -1, out matrixDQuestionCount);
                        int dTitleCount = 0;
                        //拼接矩阵下拉试题名称行
                        foreach (DataRow dr in dtMatrixDQuestion.Rows)
                        {
                            int optionCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 2);
                            matrixDOptionCount += optionCount;
                            dTitleCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 1);
                            sbTableStr.Append("<td colspan='" + (optionCount * dTitleCount) + "' align='center'>" + (int.Parse(dr["OrderNum"].ToString()) + 1) + "、" + dr["Ask"].ToString() + "</td>");
                        }
                        matrixDOptionCount = matrixDOptionCount * dTitleCount;
                        sbTableStr.Append("</tr>");
                        #endregion

                        #region 输出矩阵纵坐标选项
                        if (matrixRadioQuestionCount > 0 || matrixDQuestionCount > 0)
                        {
                            int spanLength = scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + 2;
                            sbTableStr.Append("<tr>");
                            sbTableStr.Append("<td colspan='" + spanLength + "'></td>");
                            foreach (DataRow dr in dtMatrixRQuestion.Rows)
                            {
                                int optionCount = BLL.SurveyOption.Instance.GetSurveyOptionCountBySQID(int.Parse(dr["SQID"].ToString()));
                                int titleCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 1);
                                Entities.QuerySurveyMatrixTitle matrixTitleQuery = new Entities.QuerySurveyMatrixTitle();
                                matrixTitleQuery.SQID = int.Parse(dr["SQID"].ToString());
                                matrixTitleQuery.Type = 1;
                                matrixTitleQuery.Status = 0;
                                int matrixTitleCount = 0;
                                DataTable dtMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(matrixTitleQuery, "SMTID asc", 1, -1, out matrixTitleCount);
                                foreach (DataRow row in dtMatrixTitle.Rows)
                                {
                                    sbTableStr.Append("<td  align='center'>" + row["TitleName"].ToString() + "</td>");
                                }
                            }
                            foreach (DataRow dr in dtMatrixDQuestion.Rows)
                            {
                                int optionCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 2);
                                int titleCount = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitleCount(int.Parse(dr["SQID"].ToString()), 1);
                                Entities.QuerySurveyMatrixTitle matrixTitleQuery = new Entities.QuerySurveyMatrixTitle();
                                matrixTitleQuery.SQID = int.Parse(dr["SQID"].ToString());
                                matrixTitleQuery.Type = 1;
                                matrixTitleQuery.Status = 0;
                                int matrixTitleCount = 0;
                                DataTable dtMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(matrixTitleQuery, "SMTID asc", 1, -1, out matrixTitleCount);
                                foreach (DataRow row in dtMatrixTitle.Rows)
                                {
                                    sbTableStr.Append("<td colspan='" + optionCount + "' align='center'>" + row["TitleName"].ToString() + "</td>");
                                }
                            }
                            sbTableStr.Append("</tr>");
                        }
                        #endregion

                        #region 输出选项
                        sbTableStr.Append("<tr><td></td><td></td>");
                        //按分数统计的单选题
                        foreach (DataRow dr in dtScoreQuestion.Rows)
                        {
                            sbTableStr.Append("<td></td>");
                        }
                        //不按分数统计的单选题
                        foreach (DataRow dr in dtRadioQuestion.Rows)
                        {
                            sbTableStr.Append("<td></td>");
                        }
                        //多选题
                        foreach (DataRow dr in dtCheckQuestion.Rows)
                        {
                            List<Entities.SurveyOption> list = BLL.SurveyOption.Instance.GetSurveyOptionListBySQID(int.Parse(dr["SQID"].ToString()));
                            foreach (Entities.SurveyOption optionInfo in list)
                            {
                                sbTableStr.Append("<td>" + optionInfo.OptionName + "</td>");
                            }
                        }
                        //文本题
                        foreach (DataRow dr in dtTextQuestion.Rows)
                        {
                            sbTableStr.Append("<td></td>");
                        }

                        //矩阵单选题
                        foreach (DataRow dr in dtMatrixRQuestion.Rows)
                        {
                            Entities.QuerySurveyMatrixTitle queryMatrixTitle = new Entities.QuerySurveyMatrixTitle();
                            queryMatrixTitle.SQID = int.Parse(dr["SQID"].ToString());
                            queryMatrixTitle.Type = 1;
                            queryMatrixTitle.Status = 0;
                            int totalTitle = 0;
                            DataTable dtCMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(queryMatrixTitle, "SMTID asc", 1, -1, out totalTitle);
                            foreach (DataRow drTitle in dtCMatrixTitle.Rows)
                            {
                                sbTableStr.Append("<td></td>");
                            }
                        }
                        //矩阵下拉
                        foreach (DataRow dr in dtMatrixDQuestion.Rows)
                        {
                            Entities.QuerySurveyMatrixTitle queryRMatrixTitle = new Entities.QuerySurveyMatrixTitle();
                            queryRMatrixTitle.SQID = int.Parse(dr["SQID"].ToString());
                            queryRMatrixTitle.Type = 2;
                            queryRMatrixTitle.Status = 0;
                            int totalTitle = 0;
                            DataTable dtRMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(queryRMatrixTitle, "SMTID asc", 1, -1, out totalTitle);

                            Entities.QuerySurveyMatrixTitle queryCMatrixTitle = new Entities.QuerySurveyMatrixTitle();
                            queryCMatrixTitle.SQID = int.Parse(dr["SQID"].ToString());
                            queryCMatrixTitle.Type = 1;
                            queryCMatrixTitle.Status = 0;
                            DataTable dtCMatrixTitle = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(queryCMatrixTitle, "SMTID asc", 1, -1, out totalTitle);

                            foreach (DataRow cTitle in dtCMatrixTitle.Rows)
                            {
                                foreach (DataRow rTitle in dtRMatrixTitle.Rows)
                                {
                                    sbTableStr.Append("<td>" + rTitle["TitleName"] + "</td>");
                                }
                            }
                        }
                        sbTableStr.Append("</tr>");
                        #endregion

                        #region 输出答题情况
                        DataTable dtResult = BLL.SurveyAnswer.Instance.GetAnswerDetailBySPIID(info.SPIID);
                        if (dtResult.Rows.Count > 0)
                        {
                            int colCount = dtResult.Columns.Count;
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                sbTableStr.Append("<tr>");
                                for (int j = 0; j < colCount; j++)
                                {
                                    if (j == 0)
                                    {
                                        string userName = dtResult.Rows[i][j].ToString();
                                        if (!string.IsNullOrEmpty(dtResult.Rows[i][j].ToString()))
                                        {
                                            int userId = int.Parse(dtResult.Rows[i][j].ToString());
                                            userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(userId);
                                        }
                                        //int userId=int.Parse()
                                        sbTableStr.Append("<td>" + userName + "</td>");
                                    }
                                    else if (j == 1)
                                    {
                                        sbTableStr.Append("<td>" + dtResult.Rows[i][j].ToString() + "</td>");
                                    }
                                    //单选按分数统计
                                    else if (j > 2 && j < scoreOptionCount + 3)
                                    {
                                        sbTableStr.Append("<td>" + dtResult.Rows[i][j].ToString() + "</td>");
                                    }
                                    //单选不按分数统计
                                    else if (j > scoreOptionCount + 3 && j < scoreOptionCount + radioOptionCount + 4)
                                    {
                                        string resultStr = dtResult.Rows[i][j].ToString();
                                        if (!string.IsNullOrEmpty(resultStr))
                                        {
                                            DataRow[] drSelects = dtAllOption.Select(" SOID=" + int.Parse(resultStr));
                                            if (drSelects.Length > 0)
                                            {
                                                if (drSelects[0]["IsBlank"].ToString() == "1")//如果是填空的回答，找答案
                                                {
                                                    resultStr = BLL.SurveyAnswer.Instance.getAnswerBySQID(" spiid=" + spiid + " and siid=" + drSelects[0]["siid"].ToString() + " and CreateUserID=" + dtResult.Rows[i]["cuserID"].ToString() + " and soid=" + drSelects[0]["soid"].ToString());
                                                }
                                                else
                                                {
                                                    resultStr = drSelects[0]["OptionName"].ToString();
                                                }
                                            }
                                        }
                                        sbTableStr.Append("<td>" + resultStr + "</td>");
                                    }
                                    //多选题
                                    else if (j > scoreOptionCount + radioOptionCount + 4 && j < scoreOptionCount + radioOptionCount + checkOptionCount + 5)
                                    {
                                        string resultStr = dtResult.Rows[i][j].ToString();
                                        if (!string.IsNullOrEmpty(resultStr))
                                        {
                                            DataRow[] drSelects = dtAllOption.Select(" SOID=" + int.Parse(resultStr));
                                            if (drSelects.Length > 0)
                                            {
                                                if (drSelects[0]["IsBlank"].ToString() == "1")//如果是填空的回答，找答案
                                                {
                                                    resultStr = BLL.SurveyAnswer.Instance.getAnswerBySQID(" spiid=" + spiid + " and siid=" + drSelects[0]["siid"].ToString() + " and CreateUserID=" + dtResult.Rows[i]["cuserID"].ToString() + " and soid=" + drSelects[0]["soid"].ToString());
                                                }
                                                else
                                                {
                                                    resultStr = drSelects[0]["OptionName"].ToString();
                                                }
                                            }
                                        }
                                        sbTableStr.Append("<td>" + resultStr + "</td>");
                                    }
                                    //文本题
                                    else if (j > scoreOptionCount + radioOptionCount + checkOptionCount + 5 && j < scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + 6)
                                    {
                                        sbTableStr.Append("<td>" + dtResult.Rows[i][j].ToString() + "</td>");
                                    }
                                    //矩阵单选题
                                    else if (j > scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + 6 && j < scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + matrixRadioOptionCount + 7)
                                    {
                                        string resultStr = dtResult.Rows[i][j].ToString();
                                        if (!string.IsNullOrEmpty(resultStr))
                                        {
                                            DataRow[] drSelects = dtAllOption.Select(" SOID=" + int.Parse(resultStr));
                                            if (drSelects.Length > 0)
                                            {
                                                resultStr = drSelects[0]["OptionName"].ToString();
                                            }
                                        }
                                        sbTableStr.Append("<td>" + resultStr + "</td>");
                                    }
                                    //矩阵下拉
                                    else if (j > scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + matrixRadioOptionCount + 7 && j < scoreOptionCount + radioOptionCount + checkOptionCount + textQuestionCount + matrixRadioOptionCount + matrixDOptionCount + 8)
                                    {
                                        sbTableStr.Append("<td>" + dtResult.Rows[i][j].ToString() + "</td>");
                                    }
                                }
                                sbTableStr.Append("</tr>");
                            }
                        }
                        #endregion
                    }

                    sbTableStr.Append("</table>");
                }
            }

            return sbTableStr.ToString();
        }

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
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/vnd.ms-excel";
            Page.EnableViewState = false;
            Response.Write(strContent);
            Response.Flush();
            Response.End();
        }
    }
}

