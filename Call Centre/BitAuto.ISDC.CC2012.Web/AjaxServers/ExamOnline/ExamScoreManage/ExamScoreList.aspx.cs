using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage
{
    public partial class ExamScoreList : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
            get { return Request["BGID"] == null ? "" : HttpUtility.UrlDecode(Request["BGID"].ToString()).Trim(); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        //是否有阅卷权限
        public bool Marking = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3606"))//“考试成绩管理”功能验证逻辑 
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    //是否有阅卷权限
                    Marking = BLL.Util.CheckButtonRight("SYS024BUT3204");
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

            DataTable dt = BLL.ExamOnline.Instance.GetExamScoreManage(query, " BeginTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
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


                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            AjaxPager_Custs.PageSize = PageSize;
            AjaxPager_Custs.InitPager(RecordCount);
        }

        public string Deal(string eiid, string Type, string ExamPersonID, object endtime, string isMarking, string Examepid)
        {
            string str = "";
            eiid = BLL.Util.EncryptString(eiid);
            Type = BLL.Util.EncryptString(Type);
            ExamPersonID = BLL.Util.EncryptString(ExamPersonID);
            Examepid = BLL.Util.EncryptString(Examepid);
            string come = "";






            if (isMarking == "1")
            {

                come = BLL.Util.EncryptString("2");
                str = "<a href='/ExamOnline/ExamScoreManagement/MarkExamPaper.aspx?eiid=" + eiid + "&type=" + Type + "&come=" + come + "&ExamPersonID=" + ExamPersonID + "&epid=" + Examepid + "' target='_blank'>查看</a>&nbsp;&nbsp;&nbsp;&nbsp;";



            }
            else
            {
                //判断是否有阅卷权限
                if (Marking)
                {
                    come = BLL.Util.EncryptString("3");
                    str = "<a href='/ExamOnline/ExamScoreManagement/MarkExamPaper.aspx?eiid=" + eiid + "&type=" + Type + "&come=" + come + "&ExamPersonID=" + ExamPersonID + "&epid=" + Examepid + "' target='_blank'>阅卷</a>&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }

            return str;

        }
    }
}