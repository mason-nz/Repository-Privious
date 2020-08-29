using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo;
using System.Collections;
using BitAuto.Utils.Config;
using System.Data.SqlClient;
using System.Data;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    /// <summary>
    /// TakingAnSurveyHandler 的摘要说明
    /// </summary>
    public class TakingAnSurveyHandler : IHttpHandler, IRequiresSessionState
    {

        #region 属性

        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpContext.Current.Request["Action"].ToString(); }
        }
        private string RequestSPIID
        {
            get { return HttpContext.Current.Request["SPIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SPIID"].ToString()); }
        }
        private string RequestSIID
        {
            get { return HttpContext.Current.Request["SIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SIID"].ToString()); }
        }
        private string RequestCustID
        {
            get { return HttpContext.Current.Request["CustID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["CustID"].ToString()); }
        }
        private string RequestJsonSurveyAnswer
        {
            get { return HttpContext.Current.Request["JsonSurveyAnswer"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["JsonSurveyAnswer"].ToString()).Replace(@"\", @"\\"); }
        }

        private string RequestProjectID
        {
            get { return HttpContext.Current.Request["ProjectID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ProjectID"].ToString()); }
        }
        private int RequestIsSub
        {
            get { return BLL.Util.GetCurrentRequestFormInt("IsSub"); }
        }

        #endregion

        int userID = 0;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;

            userID = BLL.Util.GetLoginUserID();

            switch (RequestAction.ToLower())
            {
                case "judgeiscorrect": judgeIsCorrect(out msg);
                    break;
                case "surveyanswersubmit": surveyAnswerSubmit(out msg);
                    break;
            }

            context.Response.Write("{msg:'" + msg + "'}");
        }

        //判断能否进入该页面或能否插入回答表；
        private void judgeIsCorrect(out string msg)
        {
            msg = string.Empty;
            //int _spiid;
            //if (!int.TryParse(RequestSPIID, out _spiid))
            //{
            //    msg = "调查项目ID有误";
            //    return;
            //}

            ////1-判断该问卷在人员表是否有该登陆者 
            //Entities.QuerySurveyPerson query_Person = new Entities.QuerySurveyPerson();
            //int count;
            //query_Person.SPIID = _spiid;
            //query_Person.ExamPersonID = userID;
            //DataTable dt_Person = BLL.SurveyPerson.Instance.GetSurveyPerson(query_Person, "", 1, 10000, out count);
            //if (dt_Person.Rows.Count == 0)
            //{
            //    msg = "您不在该问卷调查参与人员表中，无法参与该问卷的调查";
            //    return;
            //}

            ////2-判断问卷时间是否没到或已过
            //Entities.SurveyProjectInfo model_ProjectInfo = BLL.SurveyProjectInfo.Instance.GetSurveyProjectInfo(_spiid);
            //if (model_ProjectInfo == null)
            //{
            //    msg = "没有找到该问卷调查项目，无法参与该问卷的调查";
            //    return;
            //}
            //if (model_ProjectInfo.SurveyStartTime > DateTime.Now)
            //{
            //    msg = "该问卷调查项目尚未开始，无法参与该问卷的调查";
            //    return;
            //}
            //if (model_ProjectInfo.SurveyEndTime < DateTime.Now)
            //{
            //    msg = "该问卷调查项目已经结束，无法参与该问卷的调查";
            //    return;
            //}

            ////3-判断问卷是否被该登陆者提交过
            //Entities.QuerySurveyAnswer query_Answer = new Entities.QuerySurveyAnswer();
            //query_Answer.SPIID = _spiid;
            //query_Answer.CreateUserID = userID;
            //DataTable dt_Answer = BLL.SurveyAnswer.Instance.GetSurveyAnswer(query_Answer, "", 1, 10000, out count);
            //if (dt_Answer.Rows.Count > 0)
            //{
            //    msg = "您已提交过该调查问卷，无法再次提交";
            //    return;
            //}

            //如果上述验证都没问题，msg返回success
            msg = "success";
        }

        //问卷调查提交
        private void surveyAnswerSubmit(out string msg)
        {
            msg = string.Empty;
            judgeIsCorrect(out msg);
            if (msg != "success")
            {
                return;
            }

            #region 提交准备数据

            SurveyAnswerRoot modelRoot = null;
            List<Entities.SurveyAnswer> list_ESurveyAnswer = new List<Entities.SurveyAnswer>();
            ArrayList array_UserActionLog = new ArrayList();//日志

            modelRoot = (SurveyAnswerRoot)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(RequestJsonSurveyAnswer, typeof(SurveyAnswerRoot));
            if (modelRoot == null)
            {
                msg = "提交的问卷数据不存在";
                return;
            }

            for (int i = 0; i < modelRoot.DataRoot.Length; i++)
            {
                Entities.SurveyAnswer model = new Entities.SurveyAnswer();

                SurveyAnswer answer = modelRoot.DataRoot[i];
                int _spiid;
                if (int.TryParse(RequestSPIID, out _spiid))
                {
                    model.SPIID = _spiid;
                }
                if (!string.IsNullOrEmpty(RequestCustID))
                {
                    model.ReturnVisitCRMCustID = RequestCustID;
                }
                int _siid;
                if (int.TryParse(RequestSIID, out _siid))
                {
                    model.SIID = _siid;
                }
                int _sqid;
                if (int.TryParse(answer.SQID, out _sqid))
                {
                    model.SQID = _sqid;
                }
                int _smrtid;
                if (int.TryParse(answer.SMRTID, out _smrtid))
                {
                    model.SMRTID = _smrtid;
                }
                int _smctid;
                if (int.TryParse(answer.SMCTID, out _smctid))
                {
                    model.SMCTID = _smctid;
                }
                int _soid;
                if (int.TryParse(answer.SOID, out _soid))
                {
                    model.SOID = _soid;
                }
                model.AnswerContent = answer.AnswerContent;
                model.CreateUserID = userID;
                model.CreateTime = DateTime.Now;

                list_ESurveyAnswer.Add(model);

                string logStr = string.Empty;
                logStr = "调查问卷回答信息表【插入】问卷调查项目ID【" + model.SPIID + "】调查问卷ID【" + model.SIID + "】调查问卷试题ID【" + model.SQID + "】提交人【" + model.CreateUserID + "】调查问卷矩阵行ID【" + model.SMRTID + "】矩阵列ID【" + model.SMCTID + "】调查问卷选项ID【" + model.SOID + "】回答内容【" + model.AnswerContent + "】创建时间【" + model.CreateTime + "】";

                array_UserActionLog.Add(logStr);
            }

            #endregion

            #region 事务提交

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                //删除之前的答案
                if (!string.IsNullOrEmpty(RequestCustID))
                {
                    int _siid1;
                    if (int.TryParse(RequestSIID, out _siid1))
                    {
                        BLL.SurveyAnswer.Instance.DeleteByCustID(tran, _siid1, RequestCustID);
                    }
                }




                for (int i = 0; i < list_ESurveyAnswer.Count; i++)
                {
                    BLL.SurveyAnswer.Instance.Insert(tran, list_ESurveyAnswer[i]);
                }
                for (int i = 0; i < array_UserActionLog.Count; i++)
                {
                    BLL.Util.InsertUserLog(tran, array_UserActionLog[i].ToString());
                }


                //判断该项目，该任务，该问卷，是否已有答问卷记录，没有插入
                DataTable dt = null;
                Entities.QueryProjectTask_SurveyAnswer querey = new Entities.QueryProjectTask_SurveyAnswer();
                querey.CustID = RequestCustID;

                int _projectID;
                if (int.TryParse(RequestProjectID, out _projectID))
                {

                }
                querey.ProjectID = _projectID;
                int _siid;
                if (int.TryParse(RequestSIID, out _siid))
                {

                }
                querey.SIID = _siid;
                //int rowcount = 0;
                //dt = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswer(tran, querey, "", 1, 1000000, out rowcount);
                Entities.ProjectTask_SurveyAnswer getModel = BLL.ProjectTask_SurveyAnswer.Instance.GetProjectTask_SurveyAnswerByQuery(tran, querey);
                if (getModel == null)
                {
                    Entities.ProjectTask_SurveyAnswer model = new Entities.ProjectTask_SurveyAnswer();
                    model.ProjectID = _projectID;
                    model.ReturnVisitCustID = RequestCustID;
                    model.SIID = _siid;
                    model.CreateTime = System.DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    model.Status = RequestIsSub;
                    BLL.ProjectTask_SurveyAnswer.Instance.Insert(tran, model);
                }
                else
                {
                    Entities.ProjectTask_SurveyAnswer model = new Entities.ProjectTask_SurveyAnswer();
                    model.ProjectID = _projectID;
                    model.ReturnVisitCustID = RequestCustID;
                    model.SIID = _siid;
                    model.CreateTime = System.DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    model.Status = RequestIsSub;
                    BLL.ProjectTask_SurveyAnswer.Instance.Update(tran, model);
                }
                msg = "success";

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }

            #endregion

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}