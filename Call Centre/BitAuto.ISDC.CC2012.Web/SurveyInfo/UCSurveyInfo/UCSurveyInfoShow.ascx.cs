using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.SurveyInfo.UCSurveyInfo
{
    public partial class UCSurveyInfoShow : System.Web.UI.UserControl
    {
        //各类型控件试题id串，以,号隔开
        public string RadioSQIDStr = "";
        public string CheckBoxSQIDStr = "";
        public string TextSQIDStr = "";
        public string MatrixRadioSQIDStr = "";
        public string MatrixDropSQIDStr = "";


        //单选矩阵
        public string TabMatrixRadio = "";
        //多选矩阵
        public string TabMatrixDropDown = "";

        private string siid = string.Empty;
        /// <summary>
        /// 取调查问卷ID
        /// </summary>
        public string SIID
        {
            get
            {
                return siid;
            }
            set
            {
                siid = value;
            }
        }
        /// <summary>
        /// 判断该选项是否跳转
        /// </summary>
        /// <param name="soid"></param>
        /// <returns></returns>
        public string GetJump(string soid)
        {
            string jump = "0";
            Entities.QuerySurveyOptionSkipQuestion model = new QuerySurveyOptionSkipQuestion();
            model.SOID = Convert.ToInt32(soid);
            int counts = 0;
            DataTable dt = BLL.SurveyOptionSkipQuestion.Instance.GetSurveyOptionSkipQuestion(model, "", 1, 1000000, out counts);
            if (dt != null & dt.Rows.Count > 0)
            {
                jump = dt.Rows[0]["SQID"].ToString();
            }
            return jump;
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
            query.SIID = Convert.ToInt32(SIID);
            query.Status = 0;
            DataTable dt = null;
            int Allcount = 0;
            dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "OrderNum", 1, 100000, out Allcount);
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
        ///// <summary>
        ///// 取调查问卷ID
        ///// </summary>
        //public string RequestSIID
        //{
        //    get
        //    {
        //        return siid;
        //    }
        //    set
        //    {
        //        siid = value;
        //    }
        //}

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
        //显示列样式，加问题类型
        public string GetShowColumnStyle(string showcolumnnum, string askCategory)
        {
            string stylestr = "xzt3";
            if (showcolumnnum == "2")
            {
                stylestr = "xzt2";
            }
            else
            {
                //问题类型如果是矩阵
                if ((Convert.ToInt32(askCategory) == (Int32)AskCategory.MatrixDropDownT) || (Convert.ToInt32(askCategory) == (Int32)AskCategory.MatrixRadioT))
                {
                    stylestr = "clearfix";
                }
            }
            return stylestr;
        }
        /// <summary>
        /// 宽度样式
        /// </summary>
        /// <param name="askCategory"></param>
        /// <returns></returns>
        public string GetWidthStyle(string askCategory)
        {
            string stylestr = "";

            //问题类型如果是矩阵
            if ((Convert.ToInt32(askCategory) == (Int32)AskCategory.MatrixDropDownT) || (Convert.ToInt32(askCategory) == (Int32)AskCategory.MatrixRadioT))
            {
                stylestr = "width: 910px;";
            }
            return stylestr;
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            //问卷ＩＤ不为空
            if (!string.IsNullOrEmpty(SIID))
            {
                //根据问卷ID取该问卷下所有题
                QuerySurveyQuestion query = new QuerySurveyQuestion();
                query.SIID = Convert.ToInt32(SIID);
                query.Status = 0;
                DataTable dt = null;
                int Allcount = 0;
                dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "OrderNum", 1, 100000, out Allcount);
                if (dt != null)
                {
                    repeaterTableList.DataSource = dt;
                    repeaterTableList.DataBind();
                }
                //取各题型，试题id集合
                RadioSQIDStr = SetSQIDStr((Int32)AskCategory.RadioT);
                CheckBoxSQIDStr = SetSQIDStr((Int32)AskCategory.CheckBoxT);
                TextSQIDStr = SetSQIDStr((Int32)AskCategory.TextT);
                MatrixRadioSQIDStr = SetSQIDStr((Int32)AskCategory.MatrixRadioT);
                MatrixDropSQIDStr = SetSQIDStr((Int32)AskCategory.MatrixDropDownT);
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
                System.Web.UI.HtmlControls.HtmlControl liRadio = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liRadio");
                Repeater repeaterRadio = e.Item.FindControl("repeaterRadio") as Repeater;

                //取多选
                System.Web.UI.HtmlControls.HtmlControl liCheckBox = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liCheckBox");
                Repeater repeaterCheckBox = e.Item.FindControl("repeaterCheckBox") as Repeater;

                //取文本
                System.Web.UI.HtmlControls.HtmlControl liText = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liText");
                Repeater repeaterText = e.Item.FindControl("repeaterText") as Repeater;

                //单选矩阵
                System.Web.UI.HtmlControls.HtmlControl liMatrixRadio = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liMatrixRadio");

                //下拉矩阵
                System.Web.UI.HtmlControls.HtmlControl liMatrixDropDown = (System.Web.UI.HtmlControls.HtmlControl)e.Item.FindControl("liMatrixDropDown");

                //取试题ID
                Label lblSQID = e.Item.FindControl("lblSQID") as Label;
                string SQID = lblSQID.Text.Trim();

                //根据题取选项
                DataTable SuryOptionDt = null;
                QuerySurveyOption query = new QuerySurveyOption();
                query.SQID = Convert.ToInt32(SQID);
                query.Status = 0;
                int allcount = 0;
                SuryOptionDt = BLL.SurveyOption.Instance.GetSurveyOption(query, "OrderNum", 1, 100000, out allcount);
                //判断该行属于哪种题型
                if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.RadioT)
                {
                    liRadio.Visible = true;
                    repeaterRadio.DataSource = SuryOptionDt;
                    repeaterRadio.DataBind();
                }
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.CheckBoxT)
                {
                    liCheckBox.Visible = true;
                    repeaterCheckBox.DataSource = SuryOptionDt;
                    repeaterCheckBox.DataBind();
                }
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.TextT)
                {
                    liText.Visible = true;
                }
                //如果是单选矩阵
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT)
                {
                    liMatrixRadio.Visible = true;
                }
                //如果是下拉选矩阵
                else if (Convert.ToInt32(AskCategory) == (Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT)
                {
                    liMatrixDropDown.Visible = true;
                }
            }
        }

        /// <summary>
        /// 取矩阵行列
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        private DataTable GetMatrixDataTable(int SQID, int Type)
        {
            DataTable dt = null;
            int RowCount = 0;
            QuerySurveyMatrixTitle query = new QuerySurveyMatrixTitle();
            query.SQID = SQID;
            query.Type = Type;
            query.Status = 0;
            dt = BLL.SurveyMatrixTitle.Instance.GetSurveyMatrixTitle(query, "", 1, 100000, out RowCount);
            return dt;
        }
        /// <summary>
        /// 取不同类型题
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        private DataTable GetCategoryDataTable(int SIID, int Type)
        {
            DataTable dt = null;
            int RowCount = 0;
            QuerySurveyQuestion query = new QuerySurveyQuestion();
            query.SIID = SIID;
            query.AskCategory = Type;
            query.Status = 0;
            dt = BLL.SurveyQuestion.Instance.GetSurveyQuestion(query, "", 1, 100000, out RowCount);
            return dt;
        }

        /// <summary>
        ///取不同类型题
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        private string SetSQIDStr(int Type)
        {
            //{public string RadioSQIDStr = "";
            //public string CheckBoxSQIDStr = "";
            //public string TextSQIDStr = "";
            //public string MatrixRadioSQIDStr = "";
            //public string MatrixDropSQIDStr = "";
            string str = String.Empty;
            DataTable dt = GetCategoryDataTable(Convert.ToInt32(SIID), Type);
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str += dt.Rows[i]["SQID"].ToString() + ",";
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 取文本输入允许范围
        /// </summary>
        /// <param name="SQID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTextLong(string SQID, string type)
        {
            Entities.SurveyQuestion model = BLL.SurveyQuestion.Instance.GetSurveyQuestion(Convert.ToInt32(SQID));
            if (type == "1")
            {
                return model.MaxTextLen.ToString();
            }
            else
            {
                return model.MinTextLen.ToString();
            }
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
                QuerySurveyOption query = new QuerySurveyOption();
                query.SQID = Convert.ToInt32(SQID);
                query.Status = 0;
                int allcount = 0;
                SuryOptionDt = BLL.SurveyOption.Instance.GetSurveyOption(query, "OrderNum", 1, 100000, out allcount);
                DataTable DataRowTitle = GetMatrixDataTable(Convert.ToInt32(SQID), 1);
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
                    string TypeName = Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixRadioT);
                    for (int i = 0; i < DataRowTitle.Rows.Count; i++)
                    {
                        if ((i % 2) > 0)
                        {
                            sbRadio.Append("<tr style='background: #F2F2F2;'>");
                            sbRadio.Append("<th>");
                            sbRadio.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbRadio.Append("</th>");

                            for (int j = 0; j < SuryOptionDt.Rows.Count; j++)
                            {
                                sbRadio.Append("<td style='text-align:center;'>");

                                sbRadio.Append("<input type='radio' SQID='" + SQID + "' lie='" + SuryOptionDt.Rows[j]["SOID"].ToString() + "' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SQID + "' TypeName='" + TypeName + "' id='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SuryOptionDt.Rows[j]["SOID"].ToString() + "' />");
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

                                sbRadio.Append("<input type='radio' SQID='" + SQID + "' lie='" + SuryOptionDt.Rows[j]["SOID"].ToString() + "' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SQID + "'  TypeName='" + TypeName + "' id='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + SuryOptionDt.Rows[j]["SOID"].ToString() + "' />");
                                sbRadio.Append("</td>");
                            }
                            sbRadio.Append("</tr>");
                        }
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
                QuerySurveyOption query = new QuerySurveyOption();
                query.SQID = Convert.ToInt32(SQID);
                query.Status = 0;
                int allcount = 0;
                SuryOptionDt = BLL.SurveyOption.Instance.GetSurveyOption(query, "OrderNum", 1, 100000, out allcount);
                //行，列
                DataTable DataRowTitle = GetMatrixDataTable(Convert.ToInt32(SQID), 1);
                DataTable DataColumTitle = GetMatrixDataTable(Convert.ToInt32(SQID), 2);
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
                    string TypeName = Convert.ToString((Int32)BitAuto.ISDC.CC2012.Entities.AskCategory.MatrixDropDownT);
                    for (int i = 0; i < DataRowTitle.Rows.Count; i++)
                    {
                        if ((i % 2) > 0)
                        {
                            sbDropDown.Append("<tr style='background: #F2F2F2;'>");
                            sbDropDown.Append("<th>");
                            sbDropDown.Append(DataRowTitle.Rows[i]["TitleName"].ToString());
                            sbDropDown.Append("</th>");

                            for (int j = 0; j < DataColumTitle.Rows.Count; j++)
                            {
                                sbDropDown.Append("<td style='text-align:center;'>");
                                sbDropDown.Append("<select  SQID='" + SQID + "' hang='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "' lie='" + DataColumTitle.Rows[j]["SMTID"].ToString() + "' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + DataColumTitle.Rows[j]["SMTID"].ToString() + "' TypeName='" + TypeName + "'>");
                                sbDropDown.Append("<option value='-1'>请选择</option>");
                                for (int m = 0; m < SuryOptionDt.Rows.Count; m++)
                                {
                                    sbDropDown.Append("<option value='" + SuryOptionDt.Rows[m]["SOID"].ToString() + "'>" + SuryOptionDt.Rows[m]["OptionName"].ToString() + "</option>");
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
                                sbDropDown.Append("<select SQID='" + SQID + "' hang='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "' lie='" + DataColumTitle.Rows[j]["SMTID"].ToString() + "' name='" + DataRowTitle.Rows[i]["SMTID"].ToString() + "_" + DataColumTitle.Rows[j]["SMTID"].ToString() + "' TypeName='" + TypeName + "'>");
                                sbDropDown.Append("<option value='-1'>请选择</option>");
                                for (int m = 0; m < SuryOptionDt.Rows.Count; m++)
                                {
                                    sbDropDown.Append("<option value='" + SuryOptionDt.Rows[m]["SOID"].ToString() + "'>" + SuryOptionDt.Rows[m]["OptionName"].ToString() + "</option>");
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