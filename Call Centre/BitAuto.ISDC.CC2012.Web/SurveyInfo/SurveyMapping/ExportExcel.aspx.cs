using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyMapping
{
    public partial class ExportExcelTest : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string SIID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("SIID"));
            }
        }
        public string ProjectID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("ProjectID"));
            }
        }
        private string RequestBrowser
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Browser"));
            }
        }

        //1、2-代表是（数据清洗）任务的导出；3-代表客户回访的问卷导出；4-代表是其他任务的问卷导出
        public string TypeID
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("typeID"));
            }
        }
        //任务提交开始时间
        public string tasksubstart
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("tasksubstart"));
            }
        }
        //任务提交结束时间
        public string tasksubend
        {
            get
            {
                return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("tasksubend"));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string resultStr = string.Empty;

                    //（数据清洗）任务问卷导出
                    if (TypeID == "1" || TypeID == "2")
                    {
                        resultStr = GetQuestionColumn(1);
                    }
                    //客户回访问卷结果导出
                    else if (TypeID == "3")
                    {
                        resultStr = GetQuestionColumn(3);
                    }
                    //其他任务结果导出
                    else if (TypeID == "4")
                    {
                        resultStr = GetQuestionColumn(2);
                    }

                    bool isIE = false;
                    if (RequestBrowser == "IE")
                    {
                        isIE = true;
                    }
                    ExportAnswerDetail(resultStr, "问卷结果导出" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), isIE);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("导出调查问卷", ex);
                }
            }
        }

        private string temptFieldName = string.Empty;//如果是其他任务，且有客户ID，则存在自定义表中客户ID的字段名称
        //
        private string[] array_CustColumns = new string[8] { "客户ID", "客户名称", "客户省份", "客户城市", "客户区县", "客户所属大区", "会员ID", "会员名称" };
        private string GetQuestionColumn(int typeID)
        {
            //BLL.Loger.Log4Net.Info("GetQuestionColumn");

            int _siid;
            int _projectID;
            if (!int.TryParse(SIID, out _siid) || !int.TryParse(ProjectID, out _projectID))
            {
                return "";
            }

            //行的层级
            int columnLevel = 1;

            //需要导出的table
            StringBuilder sbTableStr = new StringBuilder();

            sbTableStr.Append("<table style='BORDER-COLLAPSE: collapse' borderColor=#000000 height=40 cellPadding=1 align=center border=1>");

            DataTable dt_Question = BLL.SurveyQuestion.Instance.GetQuestionBySIID(_siid);
            int questionCount = dt_Question.Rows.Count;
            int dataCleanFixedCount = 0;
            int customerFixedCount = 0;
            int otherTaskFixedCount = 0;

            int TField_Count = 0;//自定义表的数量，如果是其他任务才会有意义；否则为0
            string ColumnName_S = string.Empty;//自定义表显示的列名数组串，以逗号隔开，如果是其他任务才会有意义；
            DataTable dt_ColumnName = new DataTable();//得到自定义的字段，如果是其他任务才会有意义；

            #region 循环找出第一行 题目信息

            sbTableStr.Append("<tr>");
            //表示数据清洗任务导出，导出的前面固定项为9项
            if (typeID == 1)
            {
                dataCleanFixedCount = 10;
                sbTableStr.Append("<td>任务ID</td><td>客户ID</td><td>客户省份</td><td>客户城市</td><td>客户区县</td><td>客户所属大区</td><td>会员ID</td><td>会员名称</td><td>客户名称</td>");
            }
            //客户回访的导出
            else if (typeID == 3)
            {
                customerFixedCount = 8;
                sbTableStr.Append("<td>客户省份</td><td>客户城市</td><td>客户区县</td><td>客户所属大区</td><td>会员ID</td><td>会员名称</td><td>客户名称</td>");
            }
            //表示其他任务导出，导出的前面为自定义字段
            else if (typeID == 2)
            {
                Entities.ProjectInfo model_ProjectInfo = BLL.ProjectInfo.Instance.GetProjectInfo(_projectID);
                ColumnName_S = string.Empty;//显示的列名数组串，以逗号隔开
                dt_ColumnName = getTemplateColumn(model_ProjectInfo.TTCode, out ColumnName_S);//得到自定义的字段
                TField_Count = dt_ColumnName.Columns.Count;//自定义表的列数 
                Entities.TTable model_TTable = BLL.TTable.Instance.GetTTableByTTCode(model_ProjectInfo.TTCode);

                sbTableStr.Append("<td>任务ID</td>");
                #region 2014-09-02增加客户名称、工号
                sbTableStr.Append("<td>客服名称</td>");
                sbTableStr.Append("<td>工号</td>");

                //除了自定义表字段外 有增加 任务ID，提交时间，客服名称，工号 四个字段
                //因此 要增加4个空字段在Excel中,即：<td></td>
                otherTaskFixedCount = TField_Count + 4;
                #endregion
                for (int k = 0; k < TField_Count; k++)
                {
                    sbTableStr.Append("<td>" + dt_ColumnName.Columns[k].ToString() + "</td>");
                }

                //otherTaskFixedCount = TField_Count + 2;
            }
            //问卷导出时间
            sbTableStr.Append("<td>问卷提交时间</td>");

            for (int i = 0; i < questionCount; i++)
            {
                //选项名和个数
                string[] str_optionName = dt_Question.Rows[i]["OptionName"].ToString().Split('|');
                int count_optionName = str_optionName.Length;

                //行矩阵名和个数
                string[] str_rowTitleName = dt_Question.Rows[i]["RowTitleName"].ToString().Split('|');

                //列矩阵名和个数
                string[] str_columnTitleName = dt_Question.Rows[i]["ColumnTitleName"].ToString().Split('|');

                //占多列的数组
                string[] str_optionCloumn;

                //问题
                string ask = dt_Question.Rows[i]["Ask"].ToString();
                //选项名称
                string optionName = dt_Question.Rows[i]["OptionName"].ToString();
                //类型
                string category = dt_Question.Rows[i]["AskCategory"].ToString();

                //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                switch (category)
                {
                    case "1":
                        sbTableStr.Append("<td style='text-align:center;'>" + ask + "</td>");
                        break;
                    case "2":
                        str_optionCloumn = optionName.Split('|');
                        sbTableStr.Append("<td style='text-align:center;' colspan=" + str_optionCloumn.Length + ">" + ask + "</td>");
                        if (str_optionCloumn.Length > 0)
                        {
                            if (columnLevel <= 2)
                            {
                                columnLevel = 2;//如果有复选项，则层级至少为2
                            }
                        }
                        break;
                    case "3":
                        sbTableStr.Append("<td style='text-align:center;'>" + ask + "</td>");
                        break;
                    case "4":
                        string[] str_radioRowTitle = dt_Question.Rows[i]["RowTitleName"].ToString().Split('|');
                        int radioRowLength = str_radioRowTitle.Length;
                        sbTableStr.Append("<td style='text-align:center;' colspan=" + radioRowLength + ">" + ask + "</td>");
                        if (radioRowLength > 0)
                        {
                            if (columnLevel <= 2)
                            {
                                columnLevel = 2;//如果有矩阵单选项，则层级至少为2
                            }
                        }
                        break;
                    case "5":
                        //行矩阵
                        string[] str_rowTitle = dt_Question.Rows[i]["RowTitleName"].ToString().Split('|');
                        int rowLength = str_rowTitle.Length;
                        //列矩阵
                        string[] str_columnTitle = dt_Question.Rows[i]["ColumnTitleName"].ToString().Split('|');
                        int columnLength = str_columnTitle.Length;
                        if (rowLength > 0)
                        {
                            columnLevel = 3;//如果有行矩阵标题，则层级为3  
                        }
                        sbTableStr.Append("<td style='text-align:center;' colspan=" + (rowLength * columnLength) + ">" + ask + "</td>");
                        break;
                }
            }
            sbTableStr.Append("</tr>");


            #endregion

            #region 根据层级找到选项名并输出

            int rowIndex = 0;//行矩阵循环到第几个

            if (columnLevel == 3)
            {
                #region 层级3

                #region 第二行数据

                sbTableStr.Append("<tr>");
                if (typeID == 1)
                {
                    for (int df = 0; df < dataCleanFixedCount; df++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 3)
                {
                    for (int cf = 0; cf < customerFixedCount; cf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 2)
                {
                    for (int otf = 0; otf < otherTaskFixedCount; otf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }

                for (int i = 0; i < questionCount; i++)
                {
                    //类型
                    string category = dt_Question.Rows[i]["AskCategory"].ToString();
                    //选项
                    string[] str_optionCloumn = dt_Question.Rows[i]["OptionName"].ToString().Split('|');

                    //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                    switch (category)
                    {
                        case "1": sbTableStr.Append("<td></td>");
                            break;
                        case "2": sbTableStr.Append("<td colspan=" + str_optionCloumn.Length + "></td>");
                            break;
                        case "3": sbTableStr.Append("<td></td>");
                            break;
                        case "4": string[] str_radioRowTitle = dt_Question.Rows[i]["RowTitleName"].ToString().Split('|');
                            sbTableStr.Append("<td colspan=" + str_radioRowTitle.Length + "></td>");
                            break;
                        case "5"://行矩阵
                            string[] str_rowTitle = dt_Question.Rows[i]["RowTitleName"].ToString().Split('|');
                            int rowLength = str_rowTitle.Length;
                            //列矩阵
                            string[] str_columnTitle = dt_Question.Rows[i]["ColumnTitleName"].ToString().Split('|');
                            int columnLength = str_columnTitle.Length;
                            for (int p = rowIndex; p < rowLength; p++)
                            {
                                sbTableStr.Append("<td colspan=" + columnLength + ">" + str_rowTitle[p] + "</td>");
                                rowIndex++;
                            }
                            break;
                    }

                }

                sbTableStr.Append("</tr>");
                #endregion

                #region 第三行数据

                sbTableStr.Append("<tr>");
                if (typeID == 1)
                {
                    for (int df = 0; df < dataCleanFixedCount; df++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 3)
                {
                    for (int cf = 0; cf < customerFixedCount; cf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 2)
                {
                    for (int otf = 0; otf < otherTaskFixedCount; otf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }

                for (int t = 0; t < questionCount; t++)
                {
                    //类型
                    string category2 = dt_Question.Rows[t]["AskCategory"].ToString();

                    //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                    switch (category2)
                    {
                        case "1": sbTableStr.Append("<td></td>");
                            break;
                        case "2": //选项名和个数
                            string[] str_optionName = dt_Question.Rows[t]["OptionName"].ToString().Split('|');
                            int count_optionName = str_optionName.Length;
                            for (int m = 0; m < count_optionName; m++)
                            {
                                sbTableStr.Append("<td>" + str_optionName[m] + "</td>");
                            }
                            break;
                        case "3": sbTableStr.Append("<td></td>");
                            break;
                        case "4": string[] str_radioRowTitle = dt_Question.Rows[t]["RowTitleName"].ToString().Split('|');
                            for (int r = 0; r < str_radioRowTitle.Length; r++)
                            {
                                sbTableStr.Append("<td>" + str_radioRowTitle[r] + "</td>");
                            }
                            break;
                        case "5"://行矩阵
                            string[] str_rowTitle = dt_Question.Rows[t]["RowTitleName"].ToString().Split('|');
                            int rowLength = str_rowTitle.Length;
                            //列矩阵
                            string[] str_columnTitle = dt_Question.Rows[t]["ColumnTitleName"].ToString().Split('|');
                            int columnLength = str_columnTitle.Length;
                            for (int p = 0; p < rowLength; p++)
                            {
                                for (int m = 0; m < columnLength; m++)
                                {
                                    sbTableStr.Append("<td>" + str_columnTitle[m] + "</td>");
                                }
                            }
                            break;
                    }
                }
                sbTableStr.Append("</tr>");

                #endregion

                #endregion
            }
            else if (columnLevel == 2)
            {
                #region 层级2

                sbTableStr.Append("<tr>");
                if (typeID == 1)
                {
                    for (int df = 0; df < dataCleanFixedCount; df++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 3)
                {
                    for (int cf = 0; cf < customerFixedCount; cf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }
                else if (typeID == 2)
                {
                    for (int otf = 0; otf < otherTaskFixedCount; otf++)
                    {
                        sbTableStr.Append("<td></td>");
                    }
                }

                for (int t = 0; t < questionCount; t++)
                {
                    //类型
                    string category2 = dt_Question.Rows[t]["AskCategory"].ToString();

                    //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                    switch (category2)
                    {
                        case "1": sbTableStr.Append("<td></td>");
                            break;
                        case "2": //选项名和个数
                            string[] str_optionName = dt_Question.Rows[t]["OptionName"].ToString().Split('|');
                            int count_optionName = str_optionName.Length;
                            for (int m = 0; m < count_optionName; m++)
                            {
                                sbTableStr.Append("<td>" + str_optionName[m] + "</td>");
                            }
                            break;
                        case "3": sbTableStr.Append("<td></td>");
                            break;
                        case "4": string[] str_radioRowTitle = dt_Question.Rows[t]["RowTitleName"].ToString().Split('|');
                            for (int r = 0; r < str_radioRowTitle.Length; r++)
                            {
                                sbTableStr.Append("<td>" + str_radioRowTitle[r] + "</td>");
                            }
                            break;
                        case "5"://行矩阵
                            string[] str_rowTitle = dt_Question.Rows[t]["RowTitleName"].ToString().Split('|');
                            int rowLength = str_rowTitle.Length;
                            //列矩阵
                            string[] str_columnTitle = dt_Question.Rows[t]["ColumnTitleName"].ToString().Split('|');
                            int columnLength = str_columnTitle.Length;
                            for (int p = 0; p < rowLength; p++)
                            {
                                for (int m = 0; m < columnLength; m++)
                                {
                                    sbTableStr.Append("<td>" + str_columnTitle[m] + "</td>");
                                }
                            }
                            break;
                    }
                }
                sbTableStr.Append("</tr>");

                #endregion
            }

            #endregion

            #region 查找答案，并写入table

            string sbTrDataStr = string.Empty;
            if (typeID == 1 || typeID == 2)
            {
                sbTrDataStr = getAnswerInfo(dt_Question, _projectID, _siid, typeID, dt_ColumnName, ColumnName_S);
            }
            else if (typeID == 3)
            {
                sbTrDataStr = getReturnCustAnswerInfo(dt_Question, _projectID, _siid, typeID);
            }
            sbTableStr.Append(sbTrDataStr);

            #endregion

            sbTableStr.Append("</table>");

            //BLL.Loger.Log4Net.Info(sbTableStr.ToString());

            return sbTableStr.ToString();
        }

        //得到其他任务和数据清洗的答案tr
        private string getAnswerInfo(DataTable dt_Question, int projectID, int siid, int typeID, DataTable dt_ColumnName, string ColumnName_S)
        {
            Dictionary<int, List<string>> dicTable = new Dictionary<int, List<string>>();

            Dictionary<int, List<string>> dicTypeText = new Dictionary<int, List<string>>();

            StringBuilder sbTrStr = new StringBuilder();
            //得到答案列表
            DataTable dt_answerInfo = BLL.SurveyQuestion.Instance.GetAnswerInfoByProjectID(projectID, siid, typeID);
            //得到回答的ptid列表
            //DataTable dt_PTID = BLL.SurveyAnswer.Instance.getPTIDByProject(typeID, projectID, siid);
            //modify by qizq 加上任务开始时间和结束时间区间过滤任务 2014-11-25
            DataTable dt_PTID = BLL.SurveyAnswer.Instance.getPTIDByProject(typeID, projectID, siid, tasksubstart, tasksubend);
            //固定项table信息
            DataTable dt_BaseInfo = new DataTable();
            //其他任务客户信息固定项table
            DataTable dt_CustInfo = null;

            if (typeID == 1)
            {
                /////存在省 市 区县
                dt_BaseInfo = BLL.ProjectTask_Cust.Instance.GetCustInfoByProjectID(projectID, siid);
            }
            else if (typeID == 2)
            {
                //得到自定义基本信息 
                dt_BaseInfo = BLL.OtherTaskInfo.Instance.GetTemptInfoByProjectID(projectID.ToString());
                //如果存在客户ID，则查到客户信息，然后与之做关联
                if (temptFieldName != string.Empty)
                {
                    dt_CustInfo = BLL.ProjectInfo.Instance.GetExportCustInfoByTempt(temptFieldName, projectID.ToString());
                }
            }

            if (dt_answerInfo.Rows.Count > 0)
            {
                int effectiveRow = 1;
                //通过每个PTID循环得到每一个客户回答的信息
                for (int i = 0; i < dt_PTID.Rows.Count; i++)
                {
                    List<string> array_Content = new List<string>();
                    List<string> array_tdTextType = new List<string>();

                    string PTID = dt_PTID.Rows[i]["PTID"].ToString();
                    array_Content.Add(PTID);

                    #region 2014-09-02 其它任务增加客服名称、工号
                    array_Content.Add(dt_PTID.Rows[i]["TrueName"].ToString());
                    array_Content.Add(dt_PTID.Rows[i]["AgentNum"].ToString());
                    #endregion

                    string createTime = string.Empty;

                    #region 每一行数据

                    //sbTrStr.Append("<tr>");

                    //表示数据清洗任务导出，导出的前面固定项为8项
                    if (typeID == 1)
                    {
                        if (dt_BaseInfo.Rows.Count > 0)
                        {
                            //sbTrStr.Append("<td>" + PTID + "</td>");
                            DataRow[] dt_baseInfoByPTID = dt_BaseInfo.Select("  ptid='" + PTID + "'");

                            string provinceName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByPTID[0]["ProvinceID"].ToString());
                            string cityName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByPTID[0]["CityID"].ToString());
                            string countryName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByPTID[0]["CountyID"].ToString());

                            //查询大区 强斐 2014-12-17
                            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(
                                          BLL.Util.GetDataRowValue(dt_baseInfoByPTID[0], "ProvinceID"),
                                          BLL.Util.GetDataRowValue(dt_baseInfoByPTID[0], "CityID"),
                                          BLL.Util.GetDataRowValue(dt_baseInfoByPTID[0], "CountyID"));
                            string areaName = info == null ? "" : info.DistinctName;


                            string custName = dt_baseInfoByPTID[0]["CustName"].ToString();
                            string DMSMemberID = dt_baseInfoByPTID[0]["MemberID"].ToString();
                            string DMSMemberName = dt_baseInfoByPTID[0]["DMSName"].ToString();

                            array_Content.Add(dt_baseInfoByPTID[0]["OriginalCustID"].ToString());
                            array_Content.Add(provinceName);
                            array_Content.Add(cityName);
                            array_Content.Add(countryName);
                            array_Content.Add(areaName);
                            array_Content.Add(DMSMemberID);
                            array_tdTextType.Add(array_Content.Count.ToString());
                            array_Content.Add(DMSMemberName);
                            array_Content.Add(custName);
                            //sbTrStr.Append("<td>" + dt_baseInfoByPTID[0]["OriginalCustID"] + "</td>");
                            //sbTrStr.Append("<td>" + provinceName + "</td>");
                            //sbTrStr.Append("<td>" + cityName + "</td>");
                            //sbTrStr.Append("<td>" + countryName + "</td>");
                            //sbTrStr.Append("<td>" + areaName + "</td>");
                            //sbTrStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + DMSMemberID + "</td>");
                            //sbTrStr.Append("<td>" + DMSMemberName + "</td>");
                            //sbTrStr.Append("<td>" + custName + "</td>");
                        }
                    }
                    #region 其他任务固定项信息

                    else if (typeID == 2)
                    {
                        //sbTrStr.Append("<td>" + PTID + "</td>");

                        DataRow[] dt_baseInfoByPTID = dt_BaseInfo.Select("  PTID='" + PTID + "'");

                        //如果有客户信息，则导出客户信息固定项
                        if (temptFieldName != string.Empty && dt_CustInfo != null)
                        {
                            DataRow[] dt_custInfoByPTID = dt_CustInfo.Select("  ptid='" + PTID + "'");

                            string[] array_CustDatas = new string[8] { "", "", "", "", "", "", "", "" };
                            string excelType = string.Empty;

                            if (dt_custInfoByPTID.Length == 1)
                            {
                                string custID = dt_custInfoByPTID[0]["CustID"].ToString();
                                string provinceName = dt_custInfoByPTID[0]["ProvinceName"].ToString();
                                string cityName = dt_custInfoByPTID[0]["CityName"].ToString();
                                string countryName = dt_custInfoByPTID[0]["CountryName"].ToString();

                                //查询大区 强斐 2014-12-17
                                BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(
                                           BLL.Util.GetDataRowValue(dt_custInfoByPTID[0], "ProvinceID"),
                                           BLL.Util.GetDataRowValue(dt_custInfoByPTID[0], "CityID"),
                                           BLL.Util.GetDataRowValue(dt_custInfoByPTID[0], "CountyID"));

                                string areaName = info == null ? "" : info.DistinctName;

                                string custName = dt_custInfoByPTID[0]["CustName"].ToString();

                                //易湃会员
                                string[] MmeberIDs = dt_custInfoByPTID[0]["MemberID"].ToString().Split(',');
                                string DMSMemberID = "";
                                string DMSMemberName = "";

                                //if (MmeberIDs.Length >= 2)
                                //{
                                //    excelType = " style='vnd.ms-excel.numberformat:@' ";
                                //}
                                for (int m = 0; m < MmeberIDs.Length; m++)
                                {
                                    string[] mid = MmeberIDs[m].Split('|');
                                    if (mid.Length == 2)
                                    {
                                        DMSMemberID += mid[0] + ",";
                                        DMSMemberName += mid[1] + ",";
                                    }
                                }
                                DMSMemberID = DMSMemberID.TrimEnd(',');
                                DMSMemberName = DMSMemberName.TrimEnd(',');

                                //车商通会员
                                string[] cstMmeberIDs = dt_custInfoByPTID[0]["CstMemberID"].ToString().Split(',');
                                string cstMemberID = "";
                                string cstMemberName = "";
                                for (int m = 0; m < cstMmeberIDs.Length; m++)
                                {
                                    string[] mid = cstMmeberIDs[m].Split('|');
                                    if (mid.Length == 2)
                                    {
                                        cstMemberID += mid[0] + ",";
                                        cstMemberName += mid[1] + ",";
                                    }
                                }
                                cstMemberID = cstMemberID.TrimEnd(',');
                                cstMemberName = cstMemberName.TrimEnd(',');

                                DMSMemberID += ("," + cstMemberID).TrimEnd(',');
                                DMSMemberName += ("," + cstMemberName).TrimEnd(',');

                                array_CustDatas = new string[8] { custID, custName, provinceName, cityName, countryName, areaName, DMSMemberID, DMSMemberName };
                            }
                            for (int k = 0; k < array_CustColumns.Length; k++)
                            {
                                //sbTrStr.Append("<td " + excelType + ">" + array_CustDatas[k] + "</td>");
                                if (k == 7)
                                {
                                    array_tdTextType.Add(array_Content.Count.ToString());
                                }
                                array_Content.Add(array_CustDatas[k]);
                            }
                        }

                        //前面固定项，单独查出来(自定义) 
                        if (dt_baseInfoByPTID.Length > 0 && dt_ColumnName.Columns.Count > 0)
                        {

                            int TemptData_Count = dt_BaseInfo.Columns.Count;
                            for (int k = 0; k < TemptData_Count; k++)
                            {
                                //如果该列存在在显示的列中，则加上
                                string[] str_columns = ColumnName_S.Split(',');
                                for (int p = 0; p < str_columns.Length; p++)
                                {
                                    if (str_columns[p] == dt_BaseInfo.Columns[k].ToString())
                                    {
                                        string tdContent = string.Empty;
                                        string typeText = string.Empty;
                                        if (i <= dt_BaseInfo.Rows.Count - 1)
                                        {
                                            string contentStr = dt_baseInfoByPTID[0][k].ToString();
                                            tdContent = CommonFunction.ObjectToDateTime(contentStr).Date == new DateTime(1900, 1, 1) ? string.Empty : contentStr;
                                        }
                                        switch (str_columns[p])
                                        {
                                            case "IsEstablish":
                                            case "IsSuccess":
                                                tdContent = BLL.Util.GetIsNotStatus(dt_baseInfoByPTID[0][k].ToString());
                                                break;
                                            case "NotSuccessReason":
                                                string notSuccessReasonName = dt_baseInfoByPTID[0][k].ToString();
                                                tdContent = string.Empty;
                                                if (!(String.IsNullOrEmpty(notSuccessReasonName) ||
                                                    notSuccessReasonName.Trim() == "-1" ||
                                                    notSuccessReasonName.Trim() == "-2"))
                                                {
                                                    tdContent = BLL.Util.GetEnumOptText(typeof(Entities.NotSuccessReason), Int32.Parse(notSuccessReasonName));
                                                }
                                                break;
                                            case "NotEstablishReason":
                                                string notExportName = dt_baseInfoByPTID[0][k].ToString();
                                                tdContent = string.Empty;
                                                if (!(String.IsNullOrEmpty(notExportName) ||
                                                    notExportName.Trim() == "-1" ||
                                                    notExportName.Trim() == "-2" ||
                                                    (dt_baseInfoByPTID[0].Table.Columns.Contains("IsEstablish") && dt_baseInfoByPTID[0]["IsEstablish"].ToString() == "1")))
                                                {
                                                    //notExportName=空，-1，-2 或者 接通了 是没有值的
                                                    tdContent = BLL.Util.GetEnumOptText(typeof(Entities.NotEstablishReason), Int32.Parse(notExportName));
                                                }
                                                break;
                                            default:
                                                break;
                                        }

                                        array_Content.Add(tdContent);
                                        if (tdContent.StartsWith("0"))
                                        {
                                            array_tdTextType.Add(array_Content.Count.ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    //问卷提交时间
                    if (dt_answerInfo.Rows.Count > 0)
                    {
                        DataRow[] dt_answerTime = dt_answerInfo.Select("  ptid='" + PTID + "'");
                        if (dt_answerTime.Length > 0)
                        {
                            createTime = dt_answerTime[0]["CreateTime"].ToString();
                        }
                    }
                    //sbTrStr.Append("<td>" + createTime + "</td>");
                    array_Content.Add(createTime);

                    for (int n = 0; n < dt_Question.Rows.Count; n++)
                    {
                        int sqid = int.Parse(dt_Question.Rows[n]["SQID"].ToString());
                        //通过ptid和sqid筛选答案
                        DataRow[] dt_answer = dt_answerInfo.Select(" SQID=" + sqid + " and ptid='" + PTID + "'");
                        //如果有值，则赋值到td
                        if (dt_answer.Length > 0)
                        {
                            #region switch

                            //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                            switch (dt_Question.Rows[n]["AskCategory"].ToString())
                            {
                                case "1":
                                    //单选情况下，如果选项有填空项，则将标题和填空值同时写出来
                                    string[] array_str = dt_answer[0]["AnswerInfo"].ToString().Split('|');
                                    string type1 = string.Empty;
                                    switch (array_str.Length)
                                    {
                                        case 1: //sbTrStr.Append("<td>" + array_str[0] + "</td>");
                                            type1 = array_str[0];
                                            break;
                                        case 2: //sbTrStr.Append("<td>" + array_str[0] + "（" + array_str[1] + "）</td>");
                                            type1 = array_str[0] + "（" + array_str[1] + "）";
                                            break;
                                        default: //sbTrStr.Append("<td></td>");
                                            break;
                                    }
                                    array_Content.Add(type1);
                                    break;
                                case "2":
                                    DataRow[] drSelects_question = dt_Question.Select(" SQID=" + sqid);
                                    string[] str_options = drSelects_question[0]["OptionName"].ToString().Split('|');

                                    for (int p = 0; p < str_options.Length; p++)
                                    {
                                        int value = 0;
                                        for (int k = 0; k < dt_answer.Length; k++)
                                        {
                                            string[] str_blank = dt_answer[k]["AnswerInfo"].ToString().Split('|');

                                            if (str_blank.Length == 2)//如果有|隔开，说明是填空项
                                            {
                                                if (str_options[p] == str_blank[0].Trim())
                                                {
                                                    //sbTrStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + str_blank[1].Trim() + "</td>");
                                                    array_Content.Add(str_blank[1].Trim());
                                                    array_tdTextType.Add(array_Content.Count.ToString());
                                                    value = 1;
                                                    break;
                                                }
                                            }
                                            else if (str_options[p] == dt_answer[k]["AnswerInfo"].ToString())
                                            {
                                                //sbTrStr.Append("<td>" + dt_answer[k]["AnswerInfo"].ToString() + "</td>");
                                                array_Content.Add(dt_answer[k]["AnswerInfo"].ToString());
                                                value = 1;
                                                break;
                                            }
                                        }
                                        if (value == 0)
                                        {
                                            //sbTrStr.Append("<td></td>");
                                            array_Content.Add(string.Empty);
                                        }
                                    }
                                    break;
                                case "3":
                                    string typeText = string.Empty;
                                    string contentStr = dt_answer[0]["AnswerInfo"].ToString();
                                    //if (contentStr.StartsWith("0"))
                                    //{
                                    //    typeText = " style='vnd.ms-excel.numberformat:@' ";
                                    //}
                                    //sbTrStr.Append("<td" + typeText + ">" + contentStr + "</td>");
                                    array_Content.Add(contentStr);
                                    if (contentStr.StartsWith("0"))
                                    {
                                        array_tdTextType.Add(array_Content.Count.ToString());
                                    }
                                    break;
                                case "4": string[] str_radioRowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    for (int r = 0; r < str_radioRowTitle.Length; r++)
                                    {
                                        //sbTrStr.Append("<td>" + dt_answer[r]["AnswerInfo"] + "</td>");
                                        array_Content.Add(dt_answer[r]["AnswerInfo"].ToString());
                                    }
                                    break;
                                case "5": //行矩阵
                                    string[] str_rowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    int rowLength = str_rowTitle.Length;
                                    //列矩阵
                                    string[] str_columnTitle = dt_Question.Rows[n]["ColumnTitleName"].ToString().Split('|');
                                    int columnLength = str_columnTitle.Length;
                                    for (int p = 0; p < rowLength * columnLength; p++)
                                    {
                                        //sbTrStr.Append("<td>" + dt_answer[p]["AnswerInfo"] + "</td>");
                                        array_Content.Add(dt_answer[p]["AnswerInfo"].ToString());
                                    }
                                    break;
                            }
                            #endregion
                        }
                        //如果没有值，则赋值为<td></td>空格
                        else if (dt_answer.Length == 0)
                        {
                            #region switch

                            //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                            switch (dt_Question.Rows[n]["AskCategory"].ToString())
                            {
                                case "1": //sbTrStr.Append("<td></td>");
                                    array_Content.Add(string.Empty);
                                    break;
                                case "2": //选项名和个数
                                    string[] str_optionName = dt_Question.Rows[n]["OptionName"].ToString().Split('|');
                                    int count_optionName = str_optionName.Length;
                                    for (int m = 0; m < count_optionName; m++)
                                    {
                                        //sbTrStr.Append("<td></td>");
                                        array_Content.Add(string.Empty);
                                    }
                                    break;
                                case "3": sbTrStr.Append("<td></td>");
                                    array_Content.Add(string.Empty); break;
                                case "4": string[] str_radioRowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    for (int r = 0; r < str_radioRowTitle.Length; r++)
                                    {
                                        //sbTrStr.Append("<td></td>");
                                        array_Content.Add(string.Empty);
                                    } break;
                                case "5"://行矩阵
                                    string[] str_rowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    int rowLength = str_rowTitle.Length;
                                    //列矩阵
                                    string[] str_columnTitle = dt_Question.Rows[n]["ColumnTitleName"].ToString().Split('|');
                                    int columnLength = str_columnTitle.Length;
                                    for (int p = 0; p < rowLength * columnLength; p++)
                                    {
                                        //sbTrStr.Append("<td></td>");
                                        array_Content.Add(string.Empty);
                                    } break;
                            }

                            #endregion
                        }
                    }
                    //sbTrStr.Append("</tr>");
                    dicTable.Add(effectiveRow, array_Content);
                    dicTypeText.Add(effectiveRow, array_tdTextType);
                    ++effectiveRow;
                    #endregion
                }
            }
            //if (sbTrStr.Length > 0)
            //{
            //    return sbTrStr.ToString();
            //}
            if (dicTable.Count > 0)
            {
                StringBuilder sbTr = new StringBuilder();
                for (int c = 1; c <= dicTable.Count; c++)
                {
                    sbTr.Append("<tr>");
                    int k = 0;
                    for (int s = 0; s < dicTable[c].Count; s++)
                    {
                        string text = string.Empty;
                        if (k < dicTypeText[c].Count && dicTypeText[c][k] == (s + 1).ToString())
                        {
                            text = " style='vnd.ms-excel.numberformat:@' ";
                            ++k;
                        }
                        sbTr.Append("<td " + text + ">" + dicTable[c][s].ToString() + "</td>");
                    }
                    sbTr.Append("</tr>");
                }
                return sbTr.ToString();
            }
            else
            { return string.Empty; }
        }

        //得到回访客户答案tr
        private string getReturnCustAnswerInfo(DataTable dt_Question, int projectID, int siid, int typeID)
        {
            StringBuilder sbTrStr = new StringBuilder();
            //得到答案列表
            DataTable dt_answerInfo = BLL.SurveyQuestion.Instance.GetAnswerInfoByProjectID(projectID, siid, typeID);
            //得到回答的ReturnCust列表
            //DataTable dt_CustID = BLL.SurveyAnswer.Instance.getCustIDByProject(projectID, siid);
            //modify by qizq 加上文件提交的开始时间和结束时间区间过滤客户 2014-11-25
            DataTable dt_CustID = BLL.SurveyAnswer.Instance.getCustIDByProject(projectID, siid, tasksubstart, tasksubend);
            //固定项table信息
            DataTable dt_BaseInfo = new DataTable();
            //前面固定项，单独查出来（通过项目ID关联CRM2009库中的客户表，拿到该项目下所有的客户信息） 
            dt_BaseInfo = BLL.ProjectTask_Cust.Instance.GetCustInfoByReturnProjectID(projectID.ToString());

            if (dt_answerInfo.Rows.Count > 0)
            {
                //通过每个CustID循环得到每一个客户回答的信息
                for (int i = 0; i < dt_CustID.Rows.Count; i++)
                {
                    string CustID = dt_CustID.Rows[i]["ReturnVisitCRMCustID"].ToString();
                    string createTime = string.Empty;

                    #region 每一行数据

                    sbTrStr.Append("<tr>");

                    if (dt_BaseInfo.Rows.Count > 0)
                    {
                        DataRow[] dt_baseInfoByCustID = dt_BaseInfo.Select("  CustID='" + CustID + "'");

                        string provinceName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByCustID[0]["ProvinceID"].ToString());
                        string cityName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByCustID[0]["CityID"].ToString());
                        string countryName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(dt_baseInfoByCustID[0]["CountyID"].ToString());

                        //查询大区 强斐 2014-12-17
                        BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(
                                      BLL.Util.GetDataRowValue(dt_baseInfoByCustID[0], "ProvinceID"),
                                      BLL.Util.GetDataRowValue(dt_baseInfoByCustID[0], "CityID"),
                                      BLL.Util.GetDataRowValue(dt_baseInfoByCustID[0], "CountyID"));
                        string areaName = info == null ? "" : info.DistinctName;

                        string custName = dt_baseInfoByCustID[0]["CustName"].ToString();
                        string DMSMemberID = dt_baseInfoByCustID[0]["MemberID"].ToString();
                        string DMSMemberName = dt_baseInfoByCustID[0]["DMSName"].ToString();

                        sbTrStr.Append("<td>" + provinceName + "</td>");
                        sbTrStr.Append("<td>" + cityName + "</td>");
                        sbTrStr.Append("<td>" + countryName + "</td>");
                        sbTrStr.Append("<td>" + areaName + "</td>");
                        sbTrStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + DMSMemberID + "</td>");
                        sbTrStr.Append("<td>" + DMSMemberName + "</td>");
                        sbTrStr.Append("<td>" + custName + "</td>");
                    }
                    //问卷提交时间 
                    if (dt_answerInfo.Rows.Count > 0)
                    {
                        DataRow[] dt_answerTime = dt_answerInfo.Select("  ReturnVisitCRMCustID='" + CustID + "'");
                        if (dt_answerTime.Length > 0)
                        {
                            createTime = dt_answerTime[0]["CreateTime"].ToString();
                        }
                    }
                    sbTrStr.Append("<td>" + createTime + "</td>");

                    for (int n = 0; n < dt_Question.Rows.Count; n++)
                    {
                        int sqid = int.Parse(dt_Question.Rows[n]["SQID"].ToString());
                        //通过ReturnVisitCRMCustID和sqid筛选答案
                        DataRow[] dt_answer = dt_answerInfo.Select(" SQID=" + sqid + " and ReturnVisitCRMCustID='" + CustID + "'");
                        //如果有值，则赋值到td
                        if (dt_answer.Length > 0)
                        {
                            #region switch

                            //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                            switch (dt_Question.Rows[n]["AskCategory"].ToString())
                            {
                                case "1":
                                    string[] array_str = dt_answer[0]["AnswerInfo"].ToString().Split('|');
                                    switch (array_str.Length)
                                    {
                                        case 1: sbTrStr.Append("<td>" + array_str[0] + "</td>");
                                            break;
                                        case 2: sbTrStr.Append("<td>" + array_str[0] + "（" + array_str[1] + "）</td>");
                                            break;
                                        default: sbTrStr.Append("<td></td>");
                                            break;
                                    }

                                    break;
                                case "2":
                                    DataRow[] drSelects_question = dt_Question.Select(" SQID=" + sqid);
                                    DataRow[] drSelects_answer = dt_answerInfo.Select(" SQID=" + sqid + " and ReturnVisitCRMCustID='" + CustID + "'");
                                    string[] str_options = drSelects_question[0]["OptionName"].ToString().Split('|');

                                    for (int p = 0; p < str_options.Length; p++)
                                    {
                                        int value = 0;
                                        for (int k = 0; k < drSelects_answer.Length; k++)
                                        {
                                            string[] str_blank = drSelects_answer[k]["AnswerInfo"].ToString().Split('|');

                                            if (str_blank.Length == 2)//如果有|隔开，说明是填空项
                                            {
                                                if (str_options[p] == str_blank[0].Trim())
                                                {
                                                    sbTrStr.Append("<td style='vnd.ms-excel.numberformat:@'>" + str_blank[1].Trim() + "</td>");
                                                    value = 1;
                                                    break;
                                                }
                                            }
                                            else if (str_options[p] == drSelects_answer[k]["AnswerInfo"].ToString())
                                            {
                                                sbTrStr.Append("<td>" + drSelects_answer[k]["AnswerInfo"].ToString() + "</td>");
                                                value = 1;
                                                break;
                                            }
                                        }
                                        if (value == 0)
                                        {
                                            sbTrStr.Append("<td></td>");
                                        }
                                    }
                                    break;
                                case "3": string typeText = string.Empty;
                                    string contentStr = dt_answer[0]["AnswerInfo"].ToString();
                                    if (contentStr.StartsWith("0"))
                                    {
                                        typeText = " style='vnd.ms-excel.numberformat:@' ";
                                    }
                                    sbTrStr.Append("<td" + typeText + ">" + contentStr + "</td>");
                                    break;
                                case "4": string[] str_radioRowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    for (int r = 0; r < str_radioRowTitle.Length; r++)
                                    {
                                        sbTrStr.Append("<td>" + dt_answer[r]["AnswerInfo"] + "</td>");
                                    }
                                    break;
                                case "5": //行矩阵
                                    string[] str_rowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    int rowLength = str_rowTitle.Length;
                                    //列矩阵
                                    string[] str_columnTitle = dt_Question.Rows[n]["ColumnTitleName"].ToString().Split('|');
                                    int columnLength = str_columnTitle.Length;
                                    for (int p = 0; p < rowLength * columnLength; p++)
                                    {
                                        sbTrStr.Append("<td>" + dt_answer[p]["AnswerInfo"] + "</td>");
                                    }
                                    break;
                            }
                            #endregion
                        }
                        //如果没有值，则赋值为<td></td>空格
                        else if (dt_answer.Length == 0)
                        {
                            #region switch

                            //问题类型：1-单选；2-复选；3-文本；4-矩阵单选；5-矩阵评分题
                            switch (dt_Question.Rows[n]["AskCategory"].ToString())
                            {
                                case "1": sbTrStr.Append("<td></td>");
                                    break;
                                case "2": //选项名和个数
                                    string[] str_optionName = dt_Question.Rows[n]["OptionName"].ToString().Split('|');
                                    int count_optionName = str_optionName.Length;
                                    for (int m = 0; m < count_optionName; m++)
                                    {
                                        sbTrStr.Append("<td></td>");
                                    }
                                    break;
                                case "3": sbTrStr.Append("<td></td>"); break;
                                case "4": string[] str_radioRowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    for (int r = 0; r < str_radioRowTitle.Length; r++)
                                    {
                                        sbTrStr.Append("<td></td>");
                                    } break;
                                case "5"://行矩阵
                                    string[] str_rowTitle = dt_Question.Rows[n]["RowTitleName"].ToString().Split('|');
                                    int rowLength = str_rowTitle.Length;
                                    //列矩阵
                                    string[] str_columnTitle = dt_Question.Rows[n]["ColumnTitleName"].ToString().Split('|');
                                    int columnLength = str_columnTitle.Length;
                                    for (int p = 0; p < rowLength * columnLength; p++)
                                    {
                                        sbTrStr.Append("<td></td>");
                                    } break;
                            }

                            #endregion
                        }
                    }
                    sbTrStr.Append("</tr>");

                    #endregion
                }
            }
            if (sbTrStr.Length > 0)
            {
                return sbTrStr.ToString();
            }
            else
            { return string.Empty; }
        }


        private string getOptionStr(string category, ArrayList array_colspan, ArrayList rowTitle, ArrayList columnTitle)
        {
            StringBuilder sbTrStr = new StringBuilder();
            sbTrStr.Append("<tr>");


            sbTrStr.Append("</tr>");

            return sbTrStr.ToString();
        }

        /// <summary>
        /// 根据ttCode获取导出的列名称
        /// </summary>
        /// <param name="ttCode"></param>
        /// <returns>返回一个带列名的空表</returns>
        public DataTable getTemplateColumn(string ttCode, out string ColumnName_S)
        {
            DataTable dt_ExcelColumn = new DataTable();//需要导出的真实列名表,返回值
            int excelColumn_Count = 0;                          //列名表的个数 
            ColumnName_S = string.Empty;                    //列名数组串，以逗号隔开

            DataTable dt_TemptColumn = BLL.TField.Instance.GetTemptColumnNameByTableName(ttCode);
            if (dt_TemptColumn == null || dt_TemptColumn.Rows.Count == 0)
            { return dt_ExcelColumn; }

            int tempt_Count = dt_TemptColumn.Columns.Count;

            #region 如果存在客户ID，则需要增加客户信息字段
            for (int p = 0; p < tempt_Count; p++)
            {
                if (dt_TemptColumn.Columns[p].ToString().IndexOf("_crmcustid_name") != -1)
                {
                    temptFieldName = dt_TemptColumn.Columns[p].ToString();

                    for (int k = 0; k < array_CustColumns.Length; k++)
                    {
                        dt_ExcelColumn.Columns.Add(array_CustColumns[k]);
                    }
                }
            }
            #endregion

            for (int k = 0; k < tempt_Count; k++)
            {
                //column_Name格式：Tempf316_Province或Tempf316_Province_name
                string column_Name = dt_TemptColumn.Columns[k].ToString();

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"Tempf.*");
                bool m = reg.IsMatch(column_Name);

                if (m || column_Name == "IsEstablish" ||
                    column_Name == "NotEstablishReason" ||
                    column_Name == "IsSuccess" ||
                    column_Name == "NotSuccessReason")
                {

                    string[] columns = column_Name.Split('_');

                    string tfDesName = string.Empty;//字段名

                    //需要特殊处理的列
                    if (columns.Length >= 2)
                    {
                        string addDes = string.Empty;
                        bool isHaveCustID = false;

                        //如果为Province且有columns有三个，则表示是省份名；
                        if (columns[1] == "Province" && columns.Length == 3)
                        {
                            addDes = "（省）";
                        }
                        //如果为City且有columns有三个，则表示是城市名；
                        else if (columns[1] == "City" && columns.Length == 3)
                        {
                            addDes = "（市）";
                        }
                        //如果为Country且有columns有三个，则表示是县名；
                        else if (columns[1] == "Country" && columns.Length == 3)
                        {
                            addDes = "（县）";
                        }
                        //如果为startdata或starttime，则表示是开始日期/时间段；
                        else if (columns[1] == "startdata" || columns[1] == "starttime")
                        {
                            addDes = "（起）";
                        }
                        //如果为enddata或endtime，则表示是结束日期/时间段；
                        else if (columns[1] == "enddata" || columns[1] == "endtime")
                        {
                            addDes = "（止）";
                        }
                        else if (columns[1] == "crmcustid" && columns[2] == "name")
                        {
                            isHaveCustID = true;
                        }
                        else if ((columns[1] == "XDBrand" || columns[1] == "YXBrand" || columns[1] == "CSBrand") && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if ((columns[1] == "XDSerial" || columns[1] == "YXSerial" || columns[1] == "CSSerial") && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }

                        //columns.Length == 3：是判断是前面几项 还包括radio、check、select
                        //addDes != string.Empty：如果是时间段/点columns.Length =2，所以加上这个条件
                        //!isHaveCustID 是为了过滤客户ID这个字段
                        if ((columns.Length == 3 || addDes != string.Empty) && !isHaveCustID)
                        {
                            ColumnName_S += column_Name + ",";//将真实列名加入到串

                            DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);
                            tfDesName = dt_TField.Rows[0]["TFDesName"].ToString() + addDes;

                            //如果列名有跟固定项的客户信息的列名重复，则在自定义的列名后面加 自定义 区别
                            if (temptFieldName != string.Empty && Array.IndexOf<string>(array_CustColumns, tfDesName) != -1)
                            {
                                tfDesName += "（自定义）";
                            }

                            dt_ExcelColumn.Columns.Add(tfDesName);

                            excelColumn_Count++;
                        }

                    }

                    else
                    {
                        ColumnName_S += column_Name + ",";//将真实列名加入到串

                        DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);

                        tfDesName = dt_TField.Rows[0]["TFDesName"].ToString();

                        if (temptFieldName != string.Empty && Array.IndexOf<string>(array_CustColumns, tfDesName) != -1)
                        {
                            tfDesName += "（自定义）";
                        }

                        dt_ExcelColumn.Columns.Add(tfDesName);

                        excelColumn_Count++;
                    }
                }
            }

            return dt_ExcelColumn;
        }


        //导出
        public void ExportAnswerDetail(string strContent, string TrueName, bool IsIE)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                //Response.Charset = System.Text.ASCIIEncoding.Default.ToString();

                Response.Charset = "UTF-8";//添加编码格式
                if (IsIE)
                {
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(TrueName) + ".xls\"");
                }
                else
                {
                    Response.AddHeader("Content-Disposition", "inline;filename=\"" + TrueName + ".xls\"");
                }

                Response.ContentEncoding = Encoding.GetEncoding("UTF-8");//表格内容添加编码格式
                Response.HeaderEncoding = Encoding.GetEncoding("UTF-8");//表头添加编码格式
                //Response.ContentEncoding = System.Text.ASCIIEncoding.Default;
                Response.ContentType = "application/ms-excel";
                Page.EnableViewState = false;
                Response.Write(strContent);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("导出调查问卷", ex);
            }
        }
    }
}