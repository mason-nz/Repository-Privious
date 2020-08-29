﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask
{
    public partial class UCSurveyInfoView : System.Web.UI.UserControl
    {
        //单选矩阵
        public string TabMatrixRadio = "";
        //多选矩阵
        public string TabMatrixDropDown = "";
        //任务试卷下所有题目
        private DataTable dtSurveyQuestion = null;
        //任务试卷下所有答案
        private DataTable dtanwers = null;
        //问卷下所有选项
        private DataTable dtOptions = null;
        //问卷下所有矩阵行与列
        private DataTable dtMatrixData = null;
        //问卷下所有跳题信息
        private DataTable dtOptionSkip = null;

        //问卷名称
        private string _sIIDName = string.Empty;
        public string SIIDName
        {
            get
            {
                return _sIIDName;
            }
            set
            {
                _sIIDName = value;
            }
        }
        private string _sIID = string.Empty;
        /// <summary>
        /// 取调查问卷ID
        /// </summary>
        public string RequestSIID
        {
            get
            {
                return _sIID;
            }
            set
            {
                _sIID = value;
            }
        }

        private string _pTID;
        /// <summary>
        /// 数据清洗核实任务ID
        /// </summary>
        public string RequestPTID
        {
            get
            {
                return _pTID;
            }
            set
            {
                _pTID = value;
            }
        }
        /// <summary>
        /// 顺序号
        /// </summary>
        private int _num;
        public int Num
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
            }

        }
        private string _sPIID = string.Empty;
        /// <summary>
        /// 取调查项目ID
        /// </summary>
        public string RequestSPIID
        {
            get
            {
                return _sPIID;
            }
            set
            {
                _sPIID = value;
            }
        }
        private string _userID = string.Empty;
        public string RequestUserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }
        //动态class样式
        public string GetClass;
        //动态style样式
        public string GetStyle;
        public UCSurveyInfoView()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SurveyInfo"></param>
        public UCSurveyInfoView(Entities.SurveyInfo SurveyInfo)
            : this()
        {
            this.RequestSIID = SurveyInfo.SIID.ToString();
            this.RequestPTID = SurveyInfo.TaskID;
            this.Num = SurveyInfo.Num;
            this.SIIDName = SurveyInfo.Name;
        }
        /// <summary>
        /// 取题的顺序号
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="CheckBoxSQIDStr"></param>
        /// <returns></returns>
        public string GetIndex(string SQID)
        {
            string IndexStr = string.Empty;
            QuerySurveyQuestion query = new QuerySurveyQuestion();
            query.SIID = Convert.ToInt32(RequestSIID);
            query.Status = 0;
            DataTable dt = null;
            dt = dtSurveyQuestion;
            //int Allcount = 0;
            //dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "OrderNum", 1, 100000, out Allcount);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["SQID"].ToString() == SQID)
                    {
                        IndexStr = "跳转到第" + (i + 1).ToString() + "题";
                        break;
                    }
                }
            }
            return IndexStr;
        }
        /// <summary>
        /// 判断该选项是否跳转
        /// </summary>
        /// <param name="soid"></param>
        /// <returns></returns>
        public string GetJump(string soid)
        {
            string jump = "0";
            //Entities.QuerySurveyOptionSkipQuestion model = new QuerySurveyOptionSkipQuestion();
            //model.SOID = Convert.ToInt32(soid);
            //int counts = 0;
            //DataTable dt = BLL.SurveyOptionSkipQuestion.Instance.GetSurveyOptionSkipQuestion(model, "", 1, 1000000, out counts);
            if (dtOptionSkip != null & dtOptionSkip.Rows.Count > 0)
            {
                DataView dv = dtOptionSkip.DefaultView;
                dv.RowFilter = "SOID=" + Convert.ToInt32(soid);
                if (dv != null && dv.Count > 0)
                {
                    jump = dv[0]["SQID"].ToString();
                }
            }
            return jump;
        }

        //取题号
        public string ConvertStrForNeed(string strold)
        {
            return strold + ".";
        }
        //显示列样式
        public string GetShowColumnStyle(string showcolumnnum)
        {
            string stylestr = "xzt3";
            if (showcolumnnum == "2")
            {
                stylestr = "xzt2";
            }
            return stylestr;
        }
        /// <summary>
        /// 取任务下，该试卷所有答案
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnswerTable()
        {
            DataTable dtanwers = null;
            if (!string.IsNullOrEmpty(RequestPTID) && !string.IsNullOrEmpty(RequestSIID))
            {
                QuerySurveyAnswer query = new QuerySurveyAnswer();
                query.PTID = RequestPTID;
                int _siid = 0;
                if (int.TryParse(RequestSIID, out _siid))
                {

                }
                query.SIID = _siid;
                int AllCount = 0;
                dtanwers = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query, "", 1, 100000, out AllCount);
            }
            return dtanwers;
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

                //问卷ＩＤ不为空
                if (RequestSIID != "")
                {
                    //取任务问卷相关答案
                    dtanwers = GetAnswerTable();
                    //根据问卷ID取该问卷下所有题
                    QuerySurveyQuestion query = new QuerySurveyQuestion();
                    query.SIID = Convert.ToInt32(RequestSIID);
                    query.Status = 0;
                    int Allcount = 0;
                    dtSurveyQuestion = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "OrderNum", 1, 100000, out Allcount);

                    //根据问卷ID取问卷下所有选项
                    dtOptions = BLL.SurveyOption.Instance.GetOptionTable(Convert.ToInt32(RequestSIID));
                    //根据问卷ID取问卷下所有矩阵行与列
                    dtMatrixData = BLL.SurveyMatrixTitle.Instance.GetAllMatrixDataTableBySIID(Convert.ToInt32(RequestSIID));
                    //取所有跳题信息
                    dtOptionSkip = BLL.SurveyOptionSkipQuestion.Instance.GetAllOptionSkip(dtSurveyQuestion);


                    if (this.Num == 0)
                    {
                        GetClass = "display:block";
                        GetStyle = "/images/zhankai.gif";
                    }
                    else
                    {
                        GetClass = "display:none";

                        GetStyle = "/images/shouqi.gif";
                    }


                    if (dtSurveyQuestion != null)
                    {
                        repeaterTableList.DataSource = dtSurveyQuestion;
                        repeaterTableList.DataBind();
                    }
                }
            }
        }
        /// <summary>
        /// 绑定每一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void repeaterTableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取试题类型
                Label lblAskCategory = e.Item.FindControl("lblAskCategory") as Label;
                string AskCategory = lblAskCategory.Text.Trim();

                //取单选
                System.Web.UI.HtmlControls.HtmlControl ulRadio = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("ulRadio");
                Repeater repeaterRadio = e.Item.FindControl("repeaterRadio") as Repeater;

                //取多选
                System.Web.UI.HtmlControls.HtmlControl ulCheckBox = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("ulCheckBox");
                Repeater repeaterCheckBox = e.Item.FindControl("repeaterCheckBox") as Repeater;

                //取文本
                System.Web.UI.HtmlControls.HtmlControl ulText = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("ulText");

                //单选矩阵
                System.Web.UI.HtmlControls.HtmlControl liMatrixRadio = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liMatrixRadio");

                //下拉矩阵
                System.Web.UI.HtmlControls.HtmlControl liMatrixDropDown = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liMatrixDropDown");

                //取试题ID
                Label lblSQID = e.Item.FindControl("lblSQID") as Label;
                string SQID = lblSQID.Text.Trim();

                //根据题取选项
                DataTable SuryOptionDt = null;
                SuryOptionDt = BLL.SurveyOption.Instance.GetOptionBySQID(dtOptions, Convert.ToInt32(SQID));
                //判断该行属于哪种题型
                if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)
                {
                    ulRadio.Visible = true;
                    repeaterRadio.DataSource = SuryOptionDt;
                    repeaterRadio.DataBind();
                }
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)
                {
                    ulCheckBox.Visible = true;
                    repeaterCheckBox.DataSource = SuryOptionDt;
                    repeaterCheckBox.DataBind();
                }
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)
                {
                    ulText.Visible = true;
                }
                //如果是单选矩阵
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)
                {

                    #region 绑定单选矩阵
                    liMatrixRadio.Visible = true;

                    #endregion
                }
                //如果是下拉选矩阵
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)
                {
                    #region 绑定下拉矩阵
                    liMatrixDropDown.Visible = true;

                    #endregion
                }
            }
        }
        /// <summary>
        /// 根据问卷核实任务id，问卷id，问卷试题id，矩阵行，列标示，取所填答案
        /// </summary>
        /// <param name="PTID"></param>
        /// <param name="SIID"></param>
        /// <param name="SQID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable Getanswer(string PTID, string SIID, string SQID, string SMRTID, string SMCTID, out int askCategory)
        {
            //问题类型
            //askCategory = GetAskCategory(Convert.ToInt32(SQID));
            //DataTable dtanwers = null;
            //QuerySurveyAnswer query = new QuerySurveyAnswer();
            //query.PTID = PTID;
            //query.SIID = Convert.ToInt32(SIID);
            //query.SQID = Convert.ToInt32(SQID);
            ////如果是矩阵类型
            //if (askCategory == (Int32)Entities.AskCategory.MatrixRadioT || askCategory == (Int32)Entities.AskCategory.MatrixDropDownT)
            //{
            //    if (SMRTID != "" && SMRTID != "-2")
            //    {
            //        query.SMRTID = Convert.ToInt32(SMRTID);
            //    }
            //    if (SMCTID != "" && SMCTID != "-2")
            //    {
            //        query.SMCTID = Convert.ToInt32(SMCTID);
            //    }
            //}

            //int AllCount = 0;
            //dtanwers = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query, "", 1, 100000, out AllCount);
            //return dtanwers;
            //问题类型
            askCategory = GetAskCategory(Convert.ToInt32(SQID));
            DataTable dt = null;
            if (dtanwers != null && dtanwers.Rows.Count > 0)
            {
                dt = new DataTable();
                dt.Columns.Add("RecID", typeof(int));
                dt.Columns.Add("SPIID", typeof(int));
                dt.Columns.Add("SIID", typeof(int));
                dt.Columns.Add("SQID", typeof(int));
                dt.Columns.Add("SMRTID", typeof(int));
                dt.Columns.Add("SMCTID", typeof(int));
                dt.Columns.Add("SOID", typeof(int));
                dt.Columns.Add("AnswerContent", typeof(string));
                dt.Columns.Add("CreateTime", typeof(DateTime));
                dt.Columns.Add("CreateUserID", typeof(int));
                dt.Columns.Add("PTID", typeof(string));
                for (int i = 0; i < dtanwers.Rows.Count; i++)
                {
                    DataRow row = dt.NewRow();
                    if (askCategory == (Int32)Entities.AskCategory.MatrixRadioT || askCategory == (Int32)Entities.AskCategory.MatrixDropDownT)
                    {
                        if (dtanwers.Rows[i]["SQID"].ToString() == SQID)
                        {
                            if (SMRTID != "" && SMRTID != "-2")
                            {
                                if (SMCTID != "" && SMCTID != "-2")
                                {
                                    if (dtanwers.Rows[i]["SMRTID"].ToString() == SMRTID && dtanwers.Rows[i]["SMCTID"].ToString() == SMCTID)
                                    {
                                        row["RecID"] = dtanwers.Rows[i]["RecID"];
                                        row["SPIID"] = dtanwers.Rows[i]["SPIID"];
                                        row["SIID"] = dtanwers.Rows[i]["SIID"];
                                        row["SQID"] = dtanwers.Rows[i]["SQID"];
                                        row["SMRTID"] = dtanwers.Rows[i]["SMRTID"];
                                        row["SMCTID"] = dtanwers.Rows[i]["SMCTID"];
                                        row["SOID"] = dtanwers.Rows[i]["SOID"];
                                        row["AnswerContent"] = dtanwers.Rows[i]["AnswerContent"];
                                        row["CreateTime"] = dtanwers.Rows[i]["CreateTime"];
                                        row["CreateUserID"] = dtanwers.Rows[i]["CreateUserID"];
                                        row["PTID"] = dtanwers.Rows[i]["PTID"];
                                        dt.Rows.Add(row);
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (dtanwers.Rows[i]["SQID"].ToString() == SQID)
                        {
                            row["RecID"] = dtanwers.Rows[i]["RecID"];
                            row["SPIID"] = dtanwers.Rows[i]["SPIID"];
                            row["SIID"] = dtanwers.Rows[i]["SIID"];
                            row["SQID"] = dtanwers.Rows[i]["SQID"];
                            row["SMRTID"] = dtanwers.Rows[i]["SMRTID"];
                            row["SMCTID"] = dtanwers.Rows[i]["SMCTID"];
                            row["SOID"] = dtanwers.Rows[i]["SOID"];
                            row["AnswerContent"] = dtanwers.Rows[i]["AnswerContent"];
                            row["CreateTime"] = dtanwers.Rows[i]["CreateTime"];
                            row["CreateUserID"] = dtanwers.Rows[i]["CreateUserID"];
                            row["PTID"] = dtanwers.Rows[i]["PTID"];
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            return dt;
        }


        //是否选中
        /// <summary>
        /// 根据试题，和选项ID,判断选项是否被选中
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="SOID"></param>
        /// <returns></returns>
        public string GetChecked(string SQID, string SOID, string SMRTID, string SMCTID)
        {
            string flag = "";

            if (!string.IsNullOrEmpty(RequestPTID) && !string.IsNullOrEmpty(RequestSIID) && !string.IsNullOrEmpty(SQID))
            {
                //题型
                int askCategory = 0;
                //取答案列表
                DataTable dtAnwers = Getanswer(RequestPTID, RequestSIID, SQID, SMRTID, SMCTID, out askCategory);


                if (dtAnwers != null && dtAnwers.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAnwers.Rows.Count; i++)
                    {
                        //取答案选项ID
                        string selectedID = "";
                        //答案内容
                        string selectstr = "";
                        if (dtAnwers.Rows[i]["SOID"] != DBNull.Value)
                        {
                            selectedID = dtAnwers.Rows[i]["SOID"].ToString();
                        }
                        if (dtAnwers.Rows[i]["AnswerContent"] != DBNull.Value)
                        {
                            selectstr = dtAnwers.Rows[i]["AnswerContent"].ToString();
                        }

                        if (askCategory == (Int32)Entities.AskCategory.TextT)
                        {
                            //如果是文本题，把文本返回
                            flag = selectstr;
                            break;
                        }
                        else if (askCategory == (Int32)Entities.AskCategory.RadioT || askCategory == (Int32)Entities.AskCategory.CheckBoxT)
                        {
                            //当前选项id等于答案选项id，让当前选项选中
                            if (SOID == selectedID)
                            {
                                flag = "checked";
                                break;
                            }
                        }
                        else if (askCategory == (Int32)Entities.AskCategory.MatrixRadioT)
                        {
                            //如果是单选矩阵，矩阵行列确定的单元格在答案表中，说明该单元被选中了
                            flag = "checked";
                            break;
                        }
                        else if (askCategory == (Int32)Entities.AskCategory.MatrixDropDownT)
                        {
                            //当前选项id等于答案选项id，让当前选项选中
                            if (SOID == selectedID)
                            {
                                flag = "checked";
                                break;
                            }
                        }
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// 根据试题ID,问卷选项ID，取回答内容
        /// </summary>
        /// <returns></returns>
        public string AnswerContent(string SQID, string SOID)
        {
            string answercontent = "";
            if (!string.IsNullOrEmpty(RequestPTID) && !string.IsNullOrEmpty(RequestSIID))
            {

                DataView dv = dtanwers.DefaultView;
                dv.RowFilter = "SQID=" + Convert.ToInt32(SQID) + " and SOID=" + Convert.ToInt32(SOID) + " and PTID='" + RequestPTID + "' and SIID=" + Convert.ToInt32(RequestSIID);
                if (dv != null && dv.Count > 0)
                {
                    answercontent = dv[0]["answercontent"].ToString();
                }
            }
            return answercontent;
        }




        /// <summary>
        /// 根据问卷试题ID取问题类型
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public int GetAskCategory(int SQID)
        {
            int askcategory = 0;
            if (dtSurveyQuestion != null && dtSurveyQuestion.Rows.Count > 0)
            {
                for (int i = 0; i < dtSurveyQuestion.Rows.Count; i++)
                {
                    if (dtSurveyQuestion.Rows[i]["sqid"].ToString() == SQID.ToString())
                    {
                        int.TryParse(dtSurveyQuestion.Rows[i]["askCategory"].ToString(), out askcategory);
                    }
                }
            }
            return askcategory;
        }


        /// <summary>
        /// 是否显示文本输入框
        /// </summary>
        /// <returns></returns>
        public string IsBank(string isblank)
        {
            if (isblank == "1")
            {
                return "";
            }
            else
            {
                return "none";
            }

        }

        /// <summary>
        /// 取单选矩阵html
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public string GetTableHtmlForRadio(string SQID)
        {

            StringBuilder sbRadio = new StringBuilder();
            //判断是否是单选矩阵
            if (BLL.SurveyQuestion.Instance.GetSurveyQuestion(Convert.ToInt32(SQID)).AskCategory == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)
            {
                DataTable SuryOptionDt = null;
                SuryOptionDt = BLL.SurveyOption.Instance.GetOptionBySQID(dtOptions, Convert.ToInt32(SQID));

                DataTable DataRowTitle = BLL.SurveyMatrixTitle.Instance.GetMatrixDataTable(dtMatrixData, Convert.ToInt32(SQID), 1);
                //是否有列信息
                if (SuryOptionDt != null && SuryOptionDt.Rows.Count > 0)
                {
                    Decimal width = 95 / (SuryOptionDt.Rows.Count + 1);
                    sbRadio.Append("<table width='95%' border='1' class='wjdcView'>");
                    sbRadio.Append("<thead>");
                    sbRadio.Append("<tr style='background: #F2F2F2;'>");
                    sbRadio.Append("<th style='border-bottom: 1px solid #efefef;width:" + width + "%'>");
                    sbRadio.Append("</th>");
                    for (int i = 0; i < SuryOptionDt.Rows.Count; i++)
                    {
                        sbRadio.Append("<td style='width:" + width + "%;text-align:center;'>");
                        sbRadio.Append(SuryOptionDt.Rows[i]["OptionName"].ToString());
                        sbRadio.Append("</td>");
                    }
                    sbRadio.Append("</tr>");
                    sbRadio.Append("</thead>");
                    sbRadio.Append("<tbody>");
                    for (int i = 0; i < DataRowTitle.Rows.Count; i++)
                    {
                        #region
                        if (i % 2 > 0)
                        {
                            sbRadio.Append("<tr style='background: #F2F2F2;'>");
                            sbRadio.Append("<th>");
                            sbRadio.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbRadio.Append("</th>");
                            for (int j = 0; j < SuryOptionDt.Rows.Count; j++)
                            {
                                sbRadio.Append("<td style='text-align:center;'>");

                                //是否选中
                                string selectstr = GetChecked(SQID, SuryOptionDt.Rows[j]["SOID"].ToString(), DataRowTitle.Rows[i]["SMTID"].ToString(), SuryOptionDt.Rows[j]["SOID"].ToString());


                                sbRadio.Append("<input disabled='disabled' type='radio' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SQID + "' id='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SuryOptionDt.Rows[j]["SOID"].ToString() + "'  " + selectstr + "/>");
                                sbRadio.Append("</td>");
                            }
                            sbRadio.Append("</tr>");
                        }
                        else
                        {
                            sbRadio.Append("<tr style='background: #ffffff;'>");
                            sbRadio.Append("<th>");
                            sbRadio.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbRadio.Append("</th>");
                            for (int j = 0; j < SuryOptionDt.Rows.Count; j++)
                            {
                                sbRadio.Append("<td style='text-align:center;'>");

                                //是否选中
                                string selectstr = GetChecked(SQID, SuryOptionDt.Rows[j]["SOID"].ToString(), DataRowTitle.Rows[i]["SMTID"].ToString(), SuryOptionDt.Rows[j]["SOID"].ToString());
                                sbRadio.Append("<input disabled='disabled' type='radio' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SQID + "' id='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SuryOptionDt.Rows[j]["SOID"].ToString() + "' " + selectstr + "/>");
                                sbRadio.Append("</td>");
                            }
                            sbRadio.Append("</tr>");
                        }
                        #endregion
                    }
                    sbRadio.Append("</tbody>");
                    sbRadio.Append("</table>");
                }
            }
            return sbRadio.ToString();
        }
        /// <summary>
        /// 取下拉矩阵html
        /// </summary>
        /// <param name="SQID"></param>
        /// <returns></returns>
        public string GetHtmlForDropDown(string SQID)
        {
            StringBuilder sbDropDown = new StringBuilder();
            //判断是否是单选矩阵
            if (BLL.SurveyQuestion.Instance.GetSurveyQuestion(Convert.ToInt32(SQID)).AskCategory == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)
            {
                DataTable SuryOptionDt = null;
                SuryOptionDt = BLL.SurveyOption.Instance.GetOptionBySQID(dtOptions, Convert.ToInt32(SQID));
                //行，列
                DataTable DataRowTitle = BLL.SurveyMatrixTitle.Instance.GetMatrixDataTable(dtMatrixData, Convert.ToInt32(SQID), 1);
                DataTable DataColumTitle = BLL.SurveyMatrixTitle.Instance.GetMatrixDataTable(dtMatrixData, Convert.ToInt32(SQID), 2);
                //是否有列信息
                if (DataColumTitle != null && DataColumTitle.Rows.Count > 0)
                {
                    Decimal width = 95 / (SuryOptionDt.Rows.Count + 1);
                    sbDropDown.Append("<table width='95%' border='1' class='wjdcView'>");
                    sbDropDown.Append("<thead>");
                    sbDropDown.Append("<tr style='background: #F2F2F2;'>");
                    sbDropDown.Append("<th style='border-bottom: 1px solid #efefef;width:" + width + "%'>");
                    sbDropDown.Append("</th>");

                    for (int i = 0; i < DataColumTitle.Rows.Count; i++)
                    {
                        sbDropDown.Append("<td style='width:" + width + "%;text-align:center;'>");
                        sbDropDown.Append(DataColumTitle.Rows[i]["TitleName"].ToString());
                        sbDropDown.Append("</td>");
                    }
                    sbDropDown.Append("</tr>");
                    sbDropDown.Append("</thead>");
                    sbDropDown.Append("<tbody>");
                    for (int i = 0; i < DataRowTitle.Rows.Count; i++)
                    {
                        if (i % 2 > 0)
                        {
                            sbDropDown.Append("<tr style='background: #F2F2F2;'>");
                            sbDropDown.Append("<th>");
                            sbDropDown.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbDropDown.Append("</th>");
                            for (int j = 0; j < DataColumTitle.Rows.Count; j++)
                            {
                                sbDropDown.Append("<td style='text-align:center;'>");
                                sbDropDown.Append("<select disabled='disabled'  name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + DataColumTitle.Rows[j]["SMTID"].ToString() + "'>");
                                sbDropDown.Append("<option value='-1'>请选择</option>");
                                for (int m = 0; m < SuryOptionDt.Rows.Count; m++)
                                {
                                    //判断选项是否选中
                                    string selectstr = GetChecked(SQID, SuryOptionDt.Rows[m]["SOID"].ToString(), DataRowTitle.Rows[i]["SMTID"].ToString(), DataColumTitle.Rows[j]["SMTID"].ToString());
                                    if (selectstr == "checked")
                                    {
                                        selectstr = "selected=true";
                                    }
                                    sbDropDown.Append("<option value='" + SuryOptionDt.Rows[m]["SOID"].ToString() + "' " + selectstr + ">" + SuryOptionDt.Rows[m]["OptionName"].ToString() + "(" + SuryOptionDt.Rows[m]["Score"].ToString() + "分)</option>");
                                }
                                sbDropDown.Append("</select>");
                                sbDropDown.Append("</td>");
                            }
                            sbDropDown.Append("</tr>");
                        }
                        else
                        {
                            sbDropDown.Append("<tr style='background: #ffffff;'>");
                            sbDropDown.Append("<th>");
                            sbDropDown.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbDropDown.Append("</th>");
                            for (int j = 0; j < DataColumTitle.Rows.Count; j++)
                            {
                                sbDropDown.Append("<td style='text-align:center;'>");
                                sbDropDown.Append("<select disabled='disabled'  name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + DataColumTitle.Rows[j]["SMTID"].ToString() + "'>");
                                sbDropDown.Append("<option value='-1'>请选择</option>");
                                for (int m = 0; m < SuryOptionDt.Rows.Count; m++)
                                {
                                    //判断选项是否选中
                                    string selectstr = GetChecked(SQID, SuryOptionDt.Rows[m]["SOID"].ToString(), DataRowTitle.Rows[i]["SMTID"].ToString(), DataColumTitle.Rows[j]["SMTID"].ToString());
                                    if (selectstr == "checked")
                                    {
                                        selectstr = "selected=true";
                                    }

                                    sbDropDown.Append("<option value='" + SuryOptionDt.Rows[m]["SOID"].ToString() + "' " + selectstr + ">" + SuryOptionDt.Rows[m]["OptionName"].ToString() + "(" + SuryOptionDt.Rows[m]["Score"].ToString() + "分)</option>");
                                }
                                sbDropDown.Append("</select>");
                                sbDropDown.Append("</td>");
                            }
                        }
                    }
                    sbDropDown.Append("</tbody>");
                    sbDropDown.Append("</table>");
                    TabMatrixDropDown = sbDropDown.ToString();
                }
            }
            return sbDropDown.ToString();
        }
    }
}