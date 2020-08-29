using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline
{
    public partial class TakingAnExam : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestEIID
        {
            get { return HttpContext.Current.Request["eiid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["eiid"].ToString()); }
        }
        public string RequestType
        {
            get { return HttpContext.Current.Request["type"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["type"].ToString()); }
        }
        #endregion

        public int EPID = -2;
        Entities.ExamPaper model_paper = null;
        Entities.ExamInfo model_examInfo = null;
        Entities.MakeUpExamInfo model_makeUpExamInfo = null;
        QueryExamPerson query_examPerson = new QueryExamPerson();
        public int loginerID = -2;

        public string errorMsg = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int _eiid;
                int _type;
                if (!int.TryParse(RequestEIID, out _eiid) || !int.TryParse(RequestType, out _type))
                {
                    errorMsg = "试卷ID或试卷类型出错，无法访问";
                    return;
                }

                loginerID = BLL.Util.GetLoginUserID();
                bool isEnter = true;

                //判断权限：登陆人在该套试卷的考试人员表中；考试时间在开始时间和结束时间之内
                IsHaveLimits(out isEnter);

                if (isEnter)
                {
                    //绑定数据
                    BindData();

                    //记录考生进入该页面 开始的考试时间
                    hidStartTime.Value = DateTime.Now.ToString();
                }
            }
        }

        //绑定基础数据
        private void BindData()
        {
            switch (RequestType)
            {
                case "0": BindDataByNormalExam();
                    break;
                case "1": BindDataByMakeUpExam();
                    break;
                default:
                    return;
            }

            spanExamPaperName.InnerText = model_paper.Name;
            spanExamPaperDesc.InnerText = model_paper.ExamDesc;

            ExamPaperInfo ExamPaperInfo = null;
            if (EPID != -2)
            {
                ExamPaperInfo = BLL.ExamPaper.Instance.GetExamPaperInfo(EPID);
                this.ExamPaperView1.ExamPaperInfo = ExamPaperInfo;
            }
        }

        //绑定正常考试项目的答案数据和考试倒计时时间
        private void BindDataByNormalExam()
        {
            if (model_examInfo == null)
            {
                return;
            }

            EPID = model_examInfo.EPID;
            model_paper = BLL.ExamPaper.Instance.GetExamPaper(EPID);

            if (model_paper == null)
            {
                return;
            }

            spanCountDown.InnerText = getSpanTime(model_examInfo.ExamEndTime);
        }

        //绑定补考项目的答案数据和考试倒计时时间
        private void BindDataByMakeUpExam()
        {
            if (model_makeUpExamInfo == null)
            {
                return;
            }

            EPID = int.Parse(model_makeUpExamInfo.MakeUpEPID.ToString());
            model_paper = BLL.ExamPaper.Instance.GetExamPaper(model_makeUpExamInfo.MakeUpEPID);

            if (model_paper == null)
            {
                return;
            }

            spanCountDown.InnerText = getSpanTime(model_makeUpExamInfo.MakeupExamEndTime);
        }

        //根据考试项目开始时间和结束时间来得到倒计时的时间差
        private string getSpanTime(DateTime endTime)
        {
            DateTime startTime = DateTime.Now;
            string strSpanTime = "00:00:00";

            if (startTime <= endTime)
            {
                TimeSpan spanTime = endTime - startTime;
                int hours = spanTime.Days * 24;
                strSpanTime = (hours + spanTime.Hours).ToString() + ":" + spanTime.Minutes + ":" + spanTime.Seconds;
            }

            return strSpanTime;
        }

        //判断是否具有权限
        private void IsHaveLimits(out bool isEnter)
        {
            isEnter = true;
             
            switch (RequestType)
            {
                case "0": IsHaveLimitsByNormalExam(int.Parse(RequestEIID), out isEnter);
                    break;
                case "1": IsHaveLimitsByMakeUpExam(int.Parse(RequestEIID), out isEnter);
                    break;
                default: noLimits(out isEnter);
                    break;
            }
        }

        //类型为正常考试的试卷 权限判断
        private void IsHaveLimitsByNormalExam(int eiid, out bool isEnter)
        {
            isEnter = true;

            model_examInfo = BLL.ExamInfo.Instance.GetExamInfo(eiid);
            if (model_examInfo == null)
            {
                //试卷项目不存在
                noLimits(out isEnter);
                return;
            }

            //判断考试时间是否未到或已过
            IsTimeOver(model_examInfo.ExamStartTime, model_examInfo.ExamEndTime, out isEnter);

            query_examPerson.EIID = model_examInfo.EIID;
            query_examPerson.ExamType = 0;
            query_examPerson.ExamPerSonID = loginerID;
            int count;
            DataTable dt = BLL.ExamPerson.Instance.GetExamPerson(query_examPerson, "", 1, 10000, out count);
            if (dt.Rows.Count == 0)
            {
                //没有在该套试卷中找到登陆人，不允许访问该页面
                noLimits(out isEnter);
                return;
            }

            //判断是否提交过该问卷
            judgeIsSubmit(eiid, 0, out isEnter);
        }

        //类型为补考项目的试卷 权限判断
        private void IsHaveLimitsByMakeUpExam(int eiid, out bool isEnter)
        {
            isEnter = true;

            model_makeUpExamInfo = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfo(eiid);
            if (model_makeUpExamInfo == null)
            {
                //试卷项目不存在
                noLimits(out isEnter);
                return;
            }

            //判断考试时间是否未到或已过
            IsTimeOver(model_makeUpExamInfo.MakeUpExamStartTime, model_makeUpExamInfo.MakeupExamEndTime, out isEnter);

            query_examPerson.MEIID = model_makeUpExamInfo.MEIID;
            query_examPerson.ExamType = 1;
            query_examPerson.ExamPerSonID = loginerID;
            int count;
            DataTable dt = BLL.ExamPerson.Instance.GetExamPerson(query_examPerson, "", 1, 10000, out count);
            if (dt.Rows.Count == 0)
            {
                noLimits(out isEnter);
                return;
            }

            //判断是否提交过该问卷
            judgeIsSubmit(eiid, 1, out isEnter);
        }

        //判断该生是不是已交卷；若交卷 则无法进入该页面；若未交卷，只是保存状态，则将答案加载进来
        private void judgeIsSubmit(int eiid, int type, out bool isEnter)
        {
            isEnter = true;

            QueryExamOnline query = new QueryExamOnline();
            switch (type)
            {
                case 0: query.EIID = eiid;
                    query.IsMakeUp = 0;
                    break;
                case 1: query.MEIID = eiid;
                    query.IsMakeUp = 1;
                    break;
            }
            query.ExamPersonID = loginerID;
            int count;
            DataTable dt = BLL.ExamOnline.Instance.GetExamOnline(query, "", 1, 10000, out count);
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            //  Status = 1 代表已交卷；Status = 0 代表保存，如果是保存，应将保存的答案加载到页面
            if (dt.Rows[0]["Status"].ToString() == "1")
            {
                isEnter = false;
                errorMsg += "您已提交本次考试试卷，无法访问该页面<br/>";
                return;
                //Response.Write("<script language='javascript'>alert('您已提交本次考试试卷，无法访问该页面。');closePage();</script>");
            }
            else if (dt.Rows[0]["Status"].ToString() == "0")
            {
                //如果存在保存的数据，则传递 这条记录 在线考试ID 到用户控件，用以绑定之前保存的答案
                this.ExamPaperView1.EOLID = long.Parse(dt.Rows[0]["EOLID"].ToString());
            }
        }

        //无权限访问页面方法
        private void noLimits(out bool isEnter)
        {
            isEnter = false;
            errorMsg += "您没有该套试卷的访问权限，无法访问该页面<br/>";
            return;
            //Response.Write("<script language='javascript'>alert('您没有该套试卷的访问权限，无法访问该页面。');closePage();</script>");
        }

        //考试时间是否未到或已过，不允许访问该页面
        private void IsTimeOver(DateTime startTime, DateTime endTime, out bool isEnter)
        {
            isEnter = true;
            if (DateTime.Now < startTime || DateTime.Now > endTime)
            {
                //考试时间不在开始时间和结束时间之内，不允许访问该页面
                isEnter = false;
                errorMsg += "考试时间已过，无法访问该页面<br/>";
                return;
                //Response.Write("<script language='javascript'>alert('考试时间已过，无法访问该页面。');closePage();</script>");
            }
        }
    }
}