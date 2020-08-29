using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline
{
    public partial class ExamList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestKeywords
        {
            get { return HttpContext.Current.Request["Keywords"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Keywords"].ToString()); }
        }
        private string RequestExamCategory
        {
            get { return HttpContext.Current.Request["ExamCategory"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ExamCategory"].ToString()); }
        }
        #endregion
        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 int userID = BLL.Util.GetLoginUserID();

                 if (!BLL.Util.CheckRight(userID, "SYS024MOD3004"))
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
        public void BindData()
        {
            QueryExamPaper query = new QueryExamPaper();
            if (RequestKeywords != "")
            {
                query.Name = RequestKeywords;
            }
            if (RequestExamCategory != "")
            {
                query.ExamCategory = RequestExamCategory;
            }
            query.ExamPersonID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.ExamPaper.Instance.GetExamPaperByExamList(query, " eiEndTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);

            int testOverCount;  //  考试结束数目（已考）
            int noTestCount;    //  考试正在进行中数目（未考）

            QueryExamPaper query_testOver = query;
            query_testOver.TestOverEndTime = "1";
            DataTable dt_testOver = BLL.ExamPaper.Instance.GetExamPaperByExamList(query_testOver, "", BLL.PageCommon.Instance.PageIndex, PageSize, out testOverCount);

            //QueryExamPaper query_noTest = query;
            //query_noTest.TestOverEndTime = null;
            //query_noTest.NoTestEndTime = "1";
            //DataTable dt_noTest = BLL.ExamPaper.Instance.GetExamPaperByExamList(query_noTest, "", BLL.PageCommon.Instance.PageIndex, PageSize, out noTestCount);
            noTestCount = RecordCount - testOverCount;

            hidTestOver.Value = testOverCount.ToString();
            hidNoTest.Value = noTestCount.ToString();
        }
        //判断考试结束时间是否已到，已到返回false
        public string IsEndTimeOver(string endTime, string examType, string objectID)
        {
            string strBool = "true";
            if (DateTime.Parse(endTime) < DateTime.Now)
            {
                strBool = "false";
            }
            else
            {
                int _objectID;
                //根据考试类型判断是否提交 1:提交；0：保存；如果该试卷已被提交，则不显示粗体
                QueryExamOnline query = new QueryExamOnline();
                if (int.TryParse(objectID, out _objectID))
                {
                    switch (examType)
                    {
                        case "0": query.EIID = _objectID;
                            query.IsMakeUp = 0;
                            break;
                        case "1": query.MEIID = _objectID;
                            query.IsMakeUp = 1;
                            break;
                    }
                    query.ExamPersonID = BLL.Util.GetLoginUserID();
                    int count;
                    DataTable dt = BLL.ExamOnline.Instance.GetExamOnline(query, "", 1, 10000, out count);
                    if (dt.Rows.Count == 1 && dt.Rows[0]["Status"].ToString() == "1")
                    {
                        strBool = "false";
                    }
                }
            }
            return strBool;
        }
        /// <summary>
        /// 判断列表后面显示的按钮
        /// </summary>
        /// <param name="endTime">考试结束时间</param>
        /// <param name="examType">考试类型 1:补考；0：正常考试</param> 
        /// <param name="objectID">项目ID：补考时为补考项目ID；正常考试时为考试项目ID</param>
        /// <param name="epid">试卷ID</param> 
        /// <returns></returns>
        public string showOperBtn(string endTime, string examType, string objectID, string epid)
        {
            string strOper = string.Empty;

            int _objectID;
            if (int.TryParse(objectID, out _objectID))
            {
                string objectIDStr = BLL.Util.EncryptString(objectID);
                string epidStr = BLL.Util.EncryptString(epid);
                string examTypeStr = BLL.Util.EncryptString(examType);
                string comeStr = BLL.Util.EncryptString("1");

                if (DateTime.Parse(endTime) <= DateTime.Now)
                {
                    //根据考试类型判断是否阅卷 1:补考；0：正常考试
                    QueryExamOnline query = new QueryExamOnline();
                    int count;
                    switch (examType)
                    {
                        case "0": query.EIID = _objectID;
                            query.IsMakeUp = 0;
                            query.ExamPersonID = BLL.Util.GetLoginUserID();
                            DataTable dt0 = BLL.ExamOnline.Instance.GetExamOnline(query, "", 1, 10000, out count);
                            if (dt0.Rows.Count > 0)
                            {
                                //  IsMakeing = 1 代表已阅卷；IsMakeing = 0 代表未阅卷，如果是已阅卷，显示 查看成绩；否则不显示
                                if (dt0.Rows[0]["IsMarking"].ToString() == "1")
                                {
                                    strOper = "<span class='right btnsearch2'><input name='' type='button'  value='查看成绩' onclick='ViewExamPaper(\"" + epidStr + "\",\"" + objectIDStr + "\",\"" + comeStr + "\",\"" + examTypeStr + "\")'/></span>";
                                }
                                //strOper = getOperBtn(dt0.Rows[0]["IsMarking"].ToString(), 0, objectID, int.Parse(epid));
                            }
                            break;
                        case "1": query.MEIID = _objectID;
                            query.IsMakeUp = 1;
                            query.ExamPersonID = BLL.Util.GetLoginUserID();
                            DataTable dt1 = BLL.ExamOnline.Instance.GetExamOnline(query, "", 1, 10000, out count);
                            if (dt1.Rows.Count > 0)
                            {
                                //  IsMakeing = 1 代表已阅卷；IsMakeing = 0 代表未阅卷，如果是已阅卷，显示 查看成绩；否则不显示
                                if (dt1.Rows[0]["IsMarking"].ToString() == "1")
                                {
                                    strOper = "<span class='right btnsearch2'><input name='' type='button'  value='查看成绩' onclick='ViewExamPaper(\"" + epidStr + "\",\"" + objectIDStr + "\",\"" + comeStr + "\",\"" + examTypeStr + "\")'/></span>";
                                }

                                //strOper = getOperBtn(dt1.Rows[0]["IsMarking"].ToString(), 1, objectID, int.Parse(epid));
                            }
                            break;
                    }
                }
                else
                {
                    //判断在线考试表中，该生是否提交过，如果是则不显示后面的控件
                    QueryExamOnline query = new QueryExamOnline();
                    switch (examType)
                    {
                        case "0": query.EIID = _objectID;
                            query.IsMakeUp = 0;
                            break;
                        case "1": query.MEIID = _objectID;
                            query.IsMakeUp = 1;
                            break;
                    }
                    query.ExamPersonID = BLL.Util.GetLoginUserID();
                    int count;
                    DataTable dt = BLL.ExamOnline.Instance.GetExamOnline(query, "", 1, 10000, out count);
                    if (dt.Rows.Count > 0)
                    {
                        //如果已经阅卷，则显示 查看成绩 按钮
                        //  IsMakeing = 1 代表已阅卷；IsMakeing = 0 代表未阅卷，如果是已阅卷，显示 查看成绩；否则不显示
                        if (dt.Rows[0]["IsMarking"].ToString() == "1")
                        {
                            strOper = "<span class='right btnsearch2'><input name='' type='button'  value='查看成绩' onclick='ViewExamPaper(\"" + epidStr + "\",\"" + objectIDStr + "\",\"" + comeStr + "\",\"" + examTypeStr + "\")'/></span>";
                        }

                        //  Status = 1 代表已交卷；Status = 0 代表保存，如果是保存，显示 进入考试；否则不显示
                        if (dt.Rows[0]["Status"].ToString() == "0")
                        {
                            strOper = "<span class='right btnsearch'><input name='' type='button'  value='进入考试' onclick='StartExam(\"" + objectID + "\",\"" + examType + "\")'/></span>";
                        }
                    }
                    else
                    {
                        strOper = "<span class='right btnsearch'><input name='' type='button'  value='进入考试' onclick='StartExam(\"" + objectID + "\",\"" + examType + "\")'/></span>";
                    }
                }
            }

            return strOper;
        }

        /// <summary>
        /// 根据是否阅卷类型，返回不同按钮 1-已阅：显示 查看成绩 按钮；0-未阅：不显示
        /// </summary>
        /// <param name="markType">是否阅卷</param> 
        /// <param name="examType">是否补考 0-正常；1-补考</param>
        /// <param name="objectID">项目ID</param>
        /// <param name="epid">试卷ID</param>
        /// <returns></returns>
        private string getOperBtn(string markType, int examType, string objectID, int epid)
        {
            string strOper = string.Empty;

            string objectIDStr = BLL.Util.EncryptString(objectID);
            string epidStr = BLL.Util.EncryptString(epid.ToString());
            string examTypeStr = BLL.Util.EncryptString(examType.ToString());
            string comeStr = BLL.Util.EncryptString("1");
            switch (markType)
            {
                case "1":
                    strOper = "<span class='right btnsearch2'><input name='' type='button'  value='查看成绩' onclick='ViewExamPaper(\"" + epidStr + "\",\"" + objectIDStr + "\",\"" + comeStr + "\",\"" + examTypeStr + "\")'/></span>";
                    break;
                case "0": strOper = "";
                    break;
            }

            return strOper;
        }

    }
}