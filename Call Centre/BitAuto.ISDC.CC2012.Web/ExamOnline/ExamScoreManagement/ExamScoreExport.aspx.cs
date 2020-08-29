using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamScoreManagement
{
    public partial class ExamScoreExport : PageBase
    {

        #region 属性
        /// <summary>
        /// 考试项目名称
        /// </summary>
        private string ExamProjectName
        {
            get { return Request["ExamProjectName"] == null ? "" : HttpUtility.UrlDecode(Request["ExamProjectName"].ToString()); }
        }
        /// <summary>
        /// 试卷名称
        /// </summary>
        private string PaperName
        {
            get { return Request["PaperName"] == null ? "" : HttpUtility.UrlDecode(Request["PaperName"].ToString()); }
        }
        /// <summary>
        /// 分类
        /// </summary>
        private string Catage
        {
            get { return Request["Catage"] == null ? "" : HttpUtility.UrlDecode(Request["Catage"].ToString()); }
        }
        /// <summary>
        /// 考试开始时间
        /// </summary>
        private string BeginTime
        {
            get { return Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(Request["BeginTime"].ToString()); }
        }
        /// <summary>
        /// 考试结束时间
        /// </summary>
        private string EndTime
        {
            get { return Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(Request["EndTime"].ToString()); }
        }
        /// <summary>
        /// 考生姓名
        /// </summary>
        private string ExamPerson
        {
            get { return Request["ExamPerson"] == null ? "" : HttpUtility.UrlDecode(Request["ExamPerson"].ToString()); }
        }
        /// <summary>
        /// 选择的分组
        /// </summary>
        private string Bgid
        {
            get { return Request["BGID"] == null ? "" : HttpUtility.UrlDecode(Request["BGID"].ToString()); }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024BUT3203"))//	添加试题或为知识点管理
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        private void BindData()
        {
            Entities.ExamScoreManageQuery query = new Entities.ExamScoreManageQuery();
            #region 条件
            DateTime dateVal = new DateTime();
            if (PaperName != string.Empty)
            {
                query.PaperName = PaperName;
            }
            if (ExamProjectName != string.Empty)
            {
                query.ProjectName = ExamProjectName;
            }
            if (ExamPerson != string.Empty)
            {
                query.TrueName = ExamPerson;
            }
            if (Catage != string.Empty)
            {
                query.ExamCategory = Catage;
            }
            if (BeginTime != string.Empty && DateTime.TryParse(BeginTime, out dateVal))
            {
                query.BeginTime = dateVal;
            }
            if (EndTime != string.Empty && DateTime.TryParse(EndTime, out dateVal))
            {
                query.EndTime = dateVal;
            }
            if (CommonFunction.ObjectToInteger(Bgid) > 0)
            {
                query.BGIDS = Bgid;
            }
            #endregion
            int RecordCount = 0;

            DataTable dt = BLL.ExamOnline.Instance.GetExamScoreManage(query, " BeginTime Desc", 1, -1, out RecordCount);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["isMarking"].ToString() == "0")
                        {
                            dt.Rows[i]["zhuguan"] = "";
                        }
                        else
                        {
                            if (dt.Rows[i]["zhuguan"] == DBNull.Value)
                            {
                                dt.Rows[i]["zhuguan"] = "0";
                            }
                        }
                        if (BLL.ExamBigQuestion.Instance.HaveAskCategoryByEPID(dt.Rows[i]["epid"].ToString(), 1) == false)
                        {
                            dt.Rows[i]["Onlyselect"] = "-";
                        }
                        else
                        {
                            if (dt.Rows[i]["Onlyselect"] == DBNull.Value)
                            {
                                dt.Rows[i]["Onlyselect"] = "0";
                            }
                        }
                        if (BLL.ExamBigQuestion.Instance.HaveAskCategoryByEPID(dt.Rows[i]["epid"].ToString(), 2) == false)
                        {
                            dt.Rows[i]["moreselect"] = "-";
                        }
                        else
                        {
                            if (dt.Rows[i]["moreselect"] == DBNull.Value)
                            {
                                dt.Rows[i]["moreselect"] = "0";
                            }
                        }
                        if (BLL.ExamBigQuestion.Instance.HaveAskCategoryByEPID(dt.Rows[i]["epid"].ToString(), 4) == false)
                        {
                            dt.Rows[i]["panduan"] = "-";
                        }
                        else
                        {
                            if (dt.Rows[i]["panduan"] == DBNull.Value)
                            {
                                dt.Rows[i]["panduan"] = "0";
                            }
                        }
                        if (BLL.ExamBigQuestion.Instance.HaveAskCategoryByEPID(dt.Rows[i]["epid"].ToString(), 3) == false)
                        {
                            dt.Rows[i]["zhuguan"] = "-";
                        }
                    }
                }

                //要导出的字段
                Dictionary<string, string> ExportColums = new Dictionary<string, string>();
                ExportColums.Add("truename", "考生姓名");
                ExportColums.Add("bgname", "业务组");
                ExportColums.Add("projectname", "考试项目");
                ExportColums.Add("papername", "试卷名称");
                ExportColums.Add("onlyselect", "单选题");
                ExportColums.Add("moreselect", "复选题");
                ExportColums.Add("panduan", "判断题");
                ExportColums.Add("zhuguan", "主观题");
                ExportColums.Add("sumscore", "考试成绩");
                ExportColums.Add("begintime", "考试时间");
                ExportColums.Add("lack", "是否缺考");

                //字段排序
                dt.Columns["Truename"].SetOrdinal(0);
                dt.Columns["BGName"].SetOrdinal(1);
                dt.Columns["ProjectName"].SetOrdinal(2);
                dt.Columns["Papername"].SetOrdinal(3);
                dt.Columns["Onlyselect"].SetOrdinal(4);
                dt.Columns["moreselect"].SetOrdinal(5);
                dt.Columns["panduan"].SetOrdinal(6);
                dt.Columns["zhuguan"].SetOrdinal(7);
                dt.Columns["sumscore"].SetOrdinal(8);
                dt.Columns["begintime"].SetOrdinal(9);
                dt.Columns["lack"].SetOrdinal(10);

                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (ExportColums.ContainsKey(dt.Columns[i].ColumnName.ToLower()))
                    {
                        //字段时要导出的字段，改名
                        dt.Columns[i].ColumnName = ExportColums[dt.Columns[i].ColumnName.ToLower()];
                    }
                    else
                    {
                        //不是要导出的字段，删除
                        dt.Columns.RemoveAt(i);
                    }
                }
                BLL.Util.ExportToCSV("考试成绩", dt);
            }
        }
    }
}