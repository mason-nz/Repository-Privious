using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Data;
using System.Collections;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline
{
    /// <summary>
    /// ExamSubmitHandler 的摘要说明
    /// </summary>
    public class ExamSubmitHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        private string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        private string RequestEIID  //项目ID
        {
            get { return HttpContext.Current.Request["EIID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EIID"].ToString()); }
        }
        private string RequestExamType  //项目类型：0-正常项目考试；1-补考项目考试
        {
            get { return HttpContext.Current.Request["ExamType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ExamType"].ToString()); }
        }
        private string RequestExamPaperID  //考试ID
        {
            get { return HttpContext.Current.Request["ExamPaperID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ExamPaperID"].ToString()); }
        }
        private string RequestNoSubjectQestion  //非主观题 大题ID:小题ID:答案,大题ID:小题ID:答案
        {
            get { return HttpContext.Current.Request["NoSubjectQestion"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["NoSubjectQestion"].ToString()); }
        }
        private string RequestSubjectQuestion  //主观题 大题ID^^小题ID^^答案$$大题ID^^小题ID^^答案
        {
            get { return HttpContext.Current.Request["SubjectQuestion"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SubjectQuestion"].ToString()); }
        }
        private string RequestExamStartTime  //该考生考试开始时间
        {
            get { return HttpContext.Current.Request["ExamStartTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["ExamStartTime"].ToString()); }
        }
        #endregion
        private int loginerID = 0;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            loginerID = BLL.Util.GetLoginUserID();

            switch (RequestAction.ToLower())
            {
                case "examonlinesubmit": examOnlineSubmit(out msg);
                    break;
                case "examonlinesave": examOnlineSave(out msg);
                    break;
            }

            context.Response.Write(msg);
        }

        //提交
        public void examOnlineSubmit(out string msg)
        {
            msg = string.Empty;
            examOnlineOperate("submit", out msg);
        }

        //保存
        public void examOnlineSave(out string msg)
        {
            msg = string.Empty;
            examOnlineOperate("save", out msg);
        }

        //提交或保存：type=submit 提交；type=save 保存
        public void examOnlineOperate(string type, out string msg)
        {
            msg = string.Empty;


            int loginerID = BLL.Util.GetLoginUserID();    //登陆者ID
            int _EOLID = 0;    //插入该考生在线考试记录后返回的主键
            int _EOLDID = 0;    //插入在线考试小题明细记录后返回的主键
            int _totalScore = 0; //记录总得分，最后一步更新到在线考试表中
            bool isSubject = false;//是否存在主观题，默认false 不存在（在线考试试卷则为已阅）；true存在（在线考试试卷则为未阅）
            int _paperID;   //试卷ID
            if (!int.TryParse(RequestExamPaperID, out _paperID))
            {
                msg = "{msg:'试卷ID出错，操作失败！'}";
                return;
            }
            Entities.ExamOnline model_examOnlineInsert = null;      //新增时增加的在线考试实体，如果之前没保存过，则需要新增
            Entities.ExamOnline model_examOnlineToUpdate = null;    //需要修改之前保存过的试卷信息
            ArrayList array_examOnlineDetailDelete = new ArrayList();  //如果有过保存的记录，则保存需要删除的小题明细ID 集合
            ArrayList array_examOnlineAnswerDelete = new ArrayList();  //如果有过保存的记录，则保存需要删除的答案ID 集合
            bool paperIsSave = false;   //该考生试卷是否被保存过，false 没有；true 是；

            ArrayList array_appendValueAnswer = new ArrayList();    //记录答案对应的小题明细数组
            int examOnlineDetailIndex = 0;                          //数组值

            int _EIID;
            if (!int.TryParse(RequestEIID, out _EIID))
            {
                msg = "{msg:'项目ID出错，操作失败！'}";
                return;
            }

            judgeSubmitExamLimit(_EIID, out msg);
            if (msg != string.Empty)
            {
                return;
            }

            //在线考试明细表 集合(非主观题)
            List<Entities.ExamOnlineDetail> list_examOnlineDetail = new List<Entities.ExamOnlineDetail>();
            //在线考试答案表 集合(非主观题)
            List<Entities.ExamOnlineAnswer> list_examOnlineAnswer = new List<Entities.ExamOnlineAnswer>();

            //在线考试明细表 集合(主观题)
            List<Entities.ExamOnlineDetail> list_SubjetExamOnlineDetail = new List<Entities.ExamOnlineDetail>();
            //在线考试答案表 集合(主观题)
            List<Entities.ExamOnlineAnswer> list_SubjetExamOnlineAnswer = new List<Entities.ExamOnlineAnswer>();

            //日志：修改 在线考试表 集合
            ArrayList array_updateExamOnlineLog = new ArrayList();
            //在线考试表 修改前的model
            Entities.ExamOnline model_updateExamOnlineLog_Old = null;
            //日志：插入 在线考试表 集合
            ArrayList arry_insertExamOnlineLog = new ArrayList();
            //日志：插入 在线考试明细表 记录集合
            ArrayList array_insertExamOnlineDetailLog = new ArrayList();
            //日志：插入 在线考试答案表 记录集合
            ArrayList array_insertExamOnlineAnswerLog = new ArrayList();



            #region 准备数据

            Entities.QueryExamOnline query_examOnlineSave = new Entities.QueryExamOnline();
            int saveCount;
            if (RequestExamType == "1")    //补考项目考试
            {
                query_examOnlineSave.MEIID = _EIID;
                query_examOnlineSave.IsMakeUp = 1;
            }
            else if (RequestExamType == "0")    //正常项目考试
            {
                query_examOnlineSave.IsMakeUp = 0;
                query_examOnlineSave.EIID = _EIID;
            }
            query_examOnlineSave.ExamPersonID = loginerID;
            DataTable dt_examOnlineSave = BLL.ExamOnline.Instance.GetExamOnline(query_examOnlineSave, "", 1, 10000, out saveCount);
            if (dt_examOnlineSave.Rows.Count > 0)
            {
                paperIsSave = true;
                //存在数据，准备上次保存的数据，对其进行修改
                model_examOnlineToUpdate = BLL.ExamOnline.Instance.GetExamOnline(long.Parse(dt_examOnlineSave.Rows[0]["EOLID"].ToString()));
                model_updateExamOnlineLog_Old = model_examOnlineToUpdate;
                if (model_examOnlineToUpdate != null)
                {
                    model_examOnlineToUpdate.ExamEndTime = DateTime.Now;
                    model_examOnlineToUpdate.IsLack = 0;
                    //准备需要删除的小题明细ID 集合
                    Entities.QueryExamOnlineDetail query_examOnlineDetailDelete = new Entities.QueryExamOnlineDetail();
                    query_examOnlineDetailDelete.EOLID = int.Parse(model_examOnlineToUpdate.EOLID.ToString());
                    DataTable dt_detailDelete = BLL.ExamOnlineDetail.Instance.GetExamOnlineDetail(query_examOnlineDetailDelete, "", 1, 10000, out saveCount);
                    for (int i = 0; i < dt_detailDelete.Rows.Count; i++)
                    {
                        //插入 集合
                        array_examOnlineDetailDelete.Add(dt_detailDelete.Rows[i]["EOLDID"].ToString());
                        //准备需要删除的该小题答案ID 集合
                        Entities.QueryExamOnlineAnswer query_examOnlineAnswerDelete = new Entities.QueryExamOnlineAnswer();
                        query_examOnlineAnswerDelete.EOLDID = int.Parse(dt_detailDelete.Rows[i]["EOLDID"].ToString());
                        DataTable dt_answerDelete = BLL.ExamOnlineAnswer.Instance.GetExamOnlineAnswer(query_examOnlineAnswerDelete, "", 1, 10000, out saveCount);
                        for (int j = 0; j < dt_answerDelete.Rows.Count; j++)
                        {
                            //插入 集合
                            array_examOnlineAnswerDelete.Add(dt_answerDelete.Rows[j]["RecID"].ToString());
                        }
                    }
                }
            }

            //  a、准备 插入在线考试表记录 数据

            //  如果之前保存过数据，则不需要新增，修改即可
            if (paperIsSave == false)
            {
                model_examOnlineInsert = new Entities.ExamOnline();
                if (RequestExamType == "1")    //补考项目考试
                {
                    model_examOnlineInsert.IsMakeUp = 1;
                    model_examOnlineInsert.MEIID = _EIID;
                    //根据补考项目ID，找到对应的考试项目ID，并赋值
                    Entities.MakeUpExamInfo model_makeUpExamInfoGetEIID = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfo(_EIID);
                    if (model_makeUpExamInfoGetEIID == null)
                    {
                        msg = "{msg:'该补考项目对应考试项目ID未找到，操作失败！'}";
                        return;
                    }
                    model_examOnlineInsert.EIID = int.Parse(model_makeUpExamInfoGetEIID.EIID.ToString());
                }
                else if (RequestExamType == "0")    //正常项目考试
                {
                    model_examOnlineInsert.IsMakeUp = 0;
                    model_examOnlineInsert.EIID = _EIID;
                }
                model_examOnlineInsert.ExamPersonID = loginerID;
                model_examOnlineInsert.ExamStartTime = DateTime.Parse(RequestExamStartTime);
                model_examOnlineInsert.ExamEndTime = DateTime.Now;
                model_examOnlineInsert.CreateTime = DateTime.Now;
                model_examOnlineInsert.CreaetUserID = loginerID;
                model_examOnlineInsert.IsLack = 0;  //不算缺考

                //_EOLID = insertExamOnline(tran, model_examOnlineInsert);
            }
            if (model_examOnlineInsert != null)
            {
                model_examOnlineInsert.Status = type == "save" ? 0 : 1;
            }
            if (model_examOnlineToUpdate != null)
            {
                model_examOnlineToUpdate.Status = type == "save" ? 0 : 1;
            }

            #endregion

            #region b、准备 插入在线考试小题明细 数据

            #region 1)准备 非主观题 数据
            if (RequestNoSubjectQestion != "")
            {
                //分解成每一个小题数组，类似['大题ID:小题ID:单选题答案','大题ID:小题ID:复选题答案1;复选题答案2;复选题答案3']
                string[] noSubjectQuestion = RequestNoSubjectQestion.Split(',');
                for (int i = 0; i < noSubjectQuestion.Length; i++)
                {
                    examOnlineDetailIndex = i;
                    //分解成每一个小题详细信息的数组，类似['大题ID','小题ID','复选题答案1;复选题答案2;复选题答案3']
                    string[] eachNoSubjectQuestion = noSubjectQuestion[i].Split(':');
                    //如果该小题的详细信息都有值，则插入；否则，不插入
                    if (eachNoSubjectQuestion[0] != "" && eachNoSubjectQuestion[1] != "" && eachNoSubjectQuestion[2] != "")
                    {
                        Entities.ExamOnlineDetail model_examOnlineDetailInsert = new Entities.ExamOnlineDetail();
                        //model_examOnlineDetailInsert.EOLID = int.Parse(_EOLID.ToString());
                        model_examOnlineDetailInsert.EPID = _paperID;
                        model_examOnlineDetailInsert.BQID = long.Parse(eachNoSubjectQuestion[0]);
                        model_examOnlineDetailInsert.KLQID = long.Parse(eachNoSubjectQuestion[1]);
                        model_examOnlineDetailInsert.Score = 0;

                        //  (1)根据该生给出的答案去比较正确的答案
                        //  先根据该小题ID去找出该题正确答案串
                        string rightAnswer = string.Empty;
                        Entities.QueryKLQAnswer query_KLQAnswer = new Entities.QueryKLQAnswer();
                        int totalCount;
                        query_KLQAnswer.KLQID = model_examOnlineDetailInsert.KLQID;
                        DataTable dt_KLQAnswer = BLL.KLQAnswer.Instance.GetKLQAnswer(query_KLQAnswer, "", 1, 10000, out totalCount);
                        if (dt_KLQAnswer.Rows.Count == 0)
                        {
                            msg = "{msg:'对应题目的答案未找到，操作失败！'}";
                            return;
                        }
                        for (int j = 0; j < dt_KLQAnswer.Rows.Count; j++)
                        {
                            rightAnswer += dt_KLQAnswer.Rows[j]["KLAOID"].ToString() + ";";
                        }
                        rightAnswer = rightAnswer.TrimEnd(';');

                        //  比较该串与考生给出的答案串是否相同，相同则得分；不相同给零分
                        //  比较方法（两个长度相同，分解考生答案，保证每一个答案都在正确答案内）
                        if (eachNoSubjectQuestion[2].Length == rightAnswer.Length)
                        {
                            string[] eachNoSubjectAnswer = eachNoSubjectQuestion[2].Split(';');
                            bool IsRightOrWrong = true;    //  答案是否正确，正确true;错误false 
                            for (int ea = 0; ea < eachNoSubjectAnswer.Length; ea++)
                            {
                                if (!rightAnswer.Contains(eachNoSubjectAnswer[ea]))
                                {
                                    IsRightOrWrong = false;
                                }
                            }
                            if (IsRightOrWrong)
                            {
                                //  (2)找到该小题如果正确则应得的分数
                                Entities.ExamBigQuestion model_examBigQuestion = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(model_examOnlineDetailInsert.BQID);
                                if (model_examBigQuestion == null)
                                {
                                    msg = "{msg:'对应该小题的对应的分数未找到，操作失败！'}";
                                    return;
                                }
                                int eachQuestionScore = model_examBigQuestion.EachQuestionScore;
                                model_examOnlineDetailInsert.Score = eachQuestionScore;
                                _totalScore += eachQuestionScore;   //计入总得分
                            }
                        }
                        model_examOnlineDetailInsert.CreaetUserID = loginerID;
                        model_examOnlineDetailInsert.CreateTime = DateTime.Now;
                        //插入在线考试小题明细表 集合
                        list_examOnlineDetail.Add(model_examOnlineDetailInsert);
                        //插入在线考试小题明细表 日志集合
                        string detailsLog = "在线考试小题明细表【插入】";
                        int epidLog;
                        int bqidLog;
                        int klqidLog;
                        if (int.TryParse(model_examOnlineDetailInsert.EPID.ToString(), out epidLog))
                        {
                            Entities.ExamPaper model_examPaperLog = BLL.ExamPaper.Instance.GetExamPaper(epidLog);
                            if (model_examPaperLog != null)
                            {
                                detailsLog += "试卷名称【" + model_examPaperLog.Name + "】";
                            }
                        }
                        if (int.TryParse(model_examOnlineDetailInsert.BQID.ToString(), out bqidLog))
                        {
                            Entities.ExamBigQuestion model_examBigQuestionLog = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(bqidLog);
                            if (model_examBigQuestionLog != null)
                            {
                                detailsLog += "大题名称【" + model_examBigQuestionLog.Name + "】";
                            }
                        }
                        if (int.TryParse(model_examOnlineDetailInsert.KLQID.ToString(), out klqidLog))
                        {
                            Entities.KLQuestion model_klqQestionLog = BLL.KLQuestion.Instance.GetKLQuestion(klqidLog);
                            if (model_klqQestionLog != null)
                            {
                                detailsLog += "小题内容【" + model_klqQestionLog.Ask + "】";
                            }
                        }
                        detailsLog += "本小题得分【" + model_examOnlineDetailInsert.Score + "】插入时间【" + model_examOnlineDetailInsert.CreateTime + "】的记录";
                        array_insertExamOnlineDetailLog.Add(detailsLog);

                        //  (3)插入在线考试答案表
                        //  分解答案串，分条插入该表
                        string[] eachAnswer = eachNoSubjectQuestion[2].Split(';');
                        for (int k = 0; k < eachAnswer.Length; k++)
                        {
                            Entities.ExamOnlineAnswer model_examOnlineAnswer = new Entities.ExamOnlineAnswer();
                            //model_examOnlineAnswer.EOLDID = int.Parse(_EOLDID.ToString());
                            model_examOnlineAnswer.ExamAnswer = eachAnswer[k];
                            model_examOnlineAnswer.CreaetUserID = loginerID;
                            model_examOnlineAnswer.CreateTime = DateTime.Now;
                            //插入在线考试答案表 集合
                            list_examOnlineAnswer.Add(model_examOnlineAnswer);

                            array_appendValueAnswer.Add(examOnlineDetailIndex);

                            //插入在线考试答案表 日志集合
                            array_insertExamOnlineAnswerLog.Add("在线考试答案表【插入】提交答案：【" + model_examOnlineAnswer.ExamAnswer + "】插入时间【" + model_examOnlineAnswer.CreateTime + "】的记录");

                        }
                    }
                }
            }

            #endregion

            #region 2)准备 插入主观题 数据（主观题答案为空，也会插入）

            if (RequestSubjectQuestion != "")
            {
                //分解成每一个小题数组，类似['大题ID^^小题ID^^答案','大题ID^^小题ID^^答案','大题ID^^小题ID^^答案']
                string[] subjectQuestion = RequestSubjectQuestion.Split(new string[] { "$$" }, StringSplitOptions.None);
                for (int i = 0; i < subjectQuestion.Length; i++)
                {
                    //分解成每一题的每个详细信息，类似['大题ID','小题ID','答案']
                    string[] eachSubjectQuestion = subjectQuestion[i].Split(new string[] { "^^" }, StringSplitOptions.None);
                    Entities.ExamOnlineDetail model_subjectExamOnlineDetail = new Entities.ExamOnlineDetail();
                    //model_subjectExamOnlineDetail.EOLID = _EOLID.ToString;
                    model_subjectExamOnlineDetail.EPID = _paperID;
                    model_subjectExamOnlineDetail.BQID = long.Parse(eachSubjectQuestion[0]);
                    model_subjectExamOnlineDetail.KLQID = long.Parse(eachSubjectQuestion[1]);
                    model_subjectExamOnlineDetail.Score = 0;
                    model_subjectExamOnlineDetail.CreaetUserID = loginerID;
                    model_subjectExamOnlineDetail.CreateTime = DateTime.Now;
                    //插入在线考试小题明细表 集合
                    list_SubjetExamOnlineDetail.Add(model_subjectExamOnlineDetail);
                    //插入在线考试小题明细表 日志集合
                    string detailsLog = "在线考试小题明细表【主观题】【插入】";
                    int epidLog;
                    int bqidLog;
                    int klqidLog;
                    if (int.TryParse(model_subjectExamOnlineDetail.EPID.ToString(), out epidLog))
                    {
                        Entities.ExamPaper model_examPaperLog = BLL.ExamPaper.Instance.GetExamPaper(epidLog);
                        if (model_examPaperLog != null)
                        {
                            detailsLog += "试卷名称【" + model_examPaperLog.Name + "】";
                        }
                    }
                    if (int.TryParse(model_subjectExamOnlineDetail.BQID.ToString(), out bqidLog))
                    {
                        Entities.ExamBigQuestion model_examBigQuestionLog = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(bqidLog);
                        if (model_examBigQuestionLog != null)
                        {
                            detailsLog += "大题名称【" + model_examBigQuestionLog.Name + "】";
                        }
                    }
                    if (int.TryParse(model_subjectExamOnlineDetail.KLQID.ToString(), out klqidLog))
                    {
                        Entities.KLQuestion model_klqQestionLog = BLL.KLQuestion.Instance.GetKLQuestion(klqidLog);
                        if (model_klqQestionLog != null)
                        {
                            detailsLog += "小题内容【" + model_klqQestionLog.Ask + "】";
                        }
                    }
                    detailsLog += "本小题得分【" + model_subjectExamOnlineDetail.Score + "】插入时间【" + model_subjectExamOnlineDetail.CreateTime + "】的记录";
                    array_insertExamOnlineDetailLog.Add(detailsLog);

                    //在线考试答案表 实体
                    Entities.ExamOnlineAnswer model_subjectExamOnlineAnswer = new Entities.ExamOnlineAnswer();
                    //model_subjectExamOnlineAnswer.EOLDID = _EOLDID;
                    model_subjectExamOnlineAnswer.ExamAnswer = eachSubjectQuestion[2];
                    model_subjectExamOnlineAnswer.CreaetUserID = loginerID;
                    model_subjectExamOnlineAnswer.CreateTime = DateTime.Now;
                    //插入在线考试答案表 集合
                    list_SubjetExamOnlineAnswer.Add(model_subjectExamOnlineAnswer);

                    //插入在线考试答案表 日志集合
                    array_insertExamOnlineAnswerLog.Add("在线考试答案表【插入】提交答案：【" + model_subjectExamOnlineAnswer.ExamAnswer + "】插入时间【" + model_subjectExamOnlineAnswer.CreateTime + "】的记录");

                    //将isSubject字段修改为 true;在线考试的IsMarking需修改为0未阅
                    isSubject = true;
                }
            }

            if (model_examOnlineInsert != null)
            {
                model_examOnlineInsert.SumScore = _totalScore;
                model_examOnlineInsert.IsMarking = isSubject == false ? 1 : 0;

                //准备插入数据的日志
                string logStr = string.Empty;
                if (type == "save")
                {
                    logStr += "【保存】";
                }
                else
                {
                    logStr += "【提交】";
                }
                logStr += "在线考试表【插入】";
                string meiidStr = model_examOnlineInsert.IsMakeUp == 1 ? "补考项目ID【" + model_examOnlineInsert.MEIID + "】" : "";
                string isMakeUp = model_examOnlineInsert.IsMakeUp == 1 ? "补考" : "未补考";
                string isMakeing = model_examOnlineInsert.IsMarking == 1 ? "已阅" : "未阅";
                string status = model_examOnlineInsert.Status == 1 ? "提交" : "保存";
                //考试项目名称
                string eiidName = string.Empty;
                Entities.ExamInfo model_examInfo = BLL.ExamInfo.Instance.GetExamInfo(model_examOnlineInsert.EIID);
                if (model_examInfo != null)
                {
                    eiidName = model_examInfo.Name;
                    logStr += "考试项目名称【" + eiidName + "】";
                }
                logStr += meiidStr + "考生名称【" + BLL.Util.GetNameInHRLimitEID(loginerID) + "】考试开始时间【"
                                        + model_examOnlineInsert.ExamStartTime + "】考试结束时间【"
                                        + model_examOnlineInsert.ExamEndTime + "】总得分【"
                                        + model_examOnlineInsert.SumScore + "】是否补考【"
                                        + isMakeUp + "】是否阅卷【" + isMakeing + "】状态【" + status + "】的记录";

                arry_insertExamOnlineLog.Add(logStr);
            }
            if (model_examOnlineToUpdate != null)
            {
                model_examOnlineToUpdate.SumScore = _totalScore;
                model_examOnlineToUpdate.IsMarking = isSubject == false ? 1 : 0;

                //准备修改数据的日志
                string logStr = string.Empty;
                if (type == "save")
                {
                    logStr += "【保存】";
                }
                else
                {
                    logStr += "【提交】";
                }
                logStr += "在线考试表【更新】";
                if (model_updateExamOnlineLog_Old.SumScore != model_examOnlineToUpdate.SumScore)
                {
                    logStr += "总得分从【" + model_updateExamOnlineLog_Old.SumScore + "】修改成【" + model_examOnlineToUpdate.SumScore + "】";
                }
                if (model_updateExamOnlineLog_Old.IsMarking != model_examOnlineToUpdate.IsMarking)
                {
                    string isMarking_Old = model_updateExamOnlineLog_Old.IsMarking == 1 ? "是" : "否";
                    string isMarking_Now = model_examOnlineToUpdate.IsMarking == 1 ? "是" : "否";
                    logStr += "是否阅卷从【" + isMarking_Old + "】修改成【" + isMarking_Now + "】";
                }
                if (model_updateExamOnlineLog_Old.ExamEndTime != model_examOnlineToUpdate.ExamEndTime)
                {
                    logStr += "考试结束时间从【" + model_updateExamOnlineLog_Old.ExamEndTime + "】修改成【" + model_examOnlineToUpdate.ExamEndTime + "】";
                }
                if (model_updateExamOnlineLog_Old.Status != model_examOnlineToUpdate.Status)
                {
                    string status_Old = model_updateExamOnlineLog_Old.Status == 1 ? "提交" : "保存";
                    string status_Now = model_examOnlineToUpdate.Status == 1 ? "提交" : "保存";
                    logStr += "考生试卷状态从【" + status_Old + "】修改成【" + status_Now + "】";
                }
                logStr += "记录";
                array_updateExamOnlineLog.Add(logStr);
            }

            #endregion

            #endregion

            #region 提交、保存 实务操作

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                //如果没有保存过的，新增一条；如果被保存过，修改在线考试这条记录并删除之前的小题明细和答案，然后重新插入，反之都要插入
                if (paperIsSave == false)
                {
                    //插入在线考试表一条记录 _EOLID得到返回的主键值
                    _EOLID = insertExamOnline(tran, model_examOnlineInsert);
                }
                else
                {
                    _EOLID = int.Parse(model_examOnlineToUpdate.EOLID.ToString());

                    //修改在线考试表的该条记录
                    updateExamOnline(tran, model_examOnlineToUpdate);
                    //删除明细表的该考生本张试卷ID的所有明细记录
                    for (int i = 0; i < array_examOnlineDetailDelete.Count; i++)
                    {
                        long eoldid = long.Parse(array_examOnlineDetailDelete[i].ToString());
                        BLL.ExamOnlineDetail.Instance.Delete(tran, eoldid);
                        BLL.Util.InsertUserLog(tran, "在线考试明细表【删除】主键ID：【" + eoldid + "】的记录");
                    }
                    //删除答案表的该考生本张试卷ID的所有答案记录
                    for (int j = 0; j < array_examOnlineAnswerDelete.Count; j++)
                    {
                        long recid = long.Parse(array_examOnlineAnswerDelete[j].ToString());
                        BLL.ExamOnlineAnswer.Instance.Delete(tran, recid);
                        BLL.Util.InsertUserLog(tran, "在线考试答案表【删除】主键ID：【" + recid + "】的记录");
                    }
                }

                //插入非主观题 
                for (int kk = 0; kk < list_examOnlineDetail.Count; kk++)
                {
                    //插入在线考试小题明细表 _EOLDID到返回的主键值
                    Entities.ExamOnlineDetail eachExamOnlineDetail = list_examOnlineDetail[kk];
                    eachExamOnlineDetail.EOLID = _EOLID;
                    _EOLDID = insertOnlineDetail(tran, eachExamOnlineDetail);

                    //插入在线考试答案表  这两个表的插入不一定是一对，因为有的是多选，需要多条插入
                    for (int n = kk; n < array_appendValueAnswer.Count; n++)
                    {
                        if (kk == int.Parse(array_appendValueAnswer[n].ToString()))
                        {
                            Entities.ExamOnlineAnswer eachExamOnlineAnswer = list_examOnlineAnswer[n];
                            eachExamOnlineAnswer.EOLDID = _EOLDID;
                            insertExamOnlineAnswer(tran, eachExamOnlineAnswer);
                        }
                    }
                }

                //插入主观题
                for (int tt = 0; tt < list_SubjetExamOnlineDetail.Count; tt++)
                {
                    //插入在线考试小题明细表 _EOLDID到返回的主键值
                    Entities.ExamOnlineDetail eachExamOnlineDetail = list_SubjetExamOnlineDetail[tt];
                    eachExamOnlineDetail.EOLID = _EOLID;
                    _EOLDID = int.Parse(insertOnlineDetail(tran, eachExamOnlineDetail).ToString());

                    //插入在线考试答案表 这两个表的插入肯定是一对，所以不用循环list
                    Entities.ExamOnlineAnswer eachExamOnlineAnswer = list_SubjetExamOnlineAnswer[tt];
                    eachExamOnlineAnswer.EOLDID = _EOLDID;
                    insertExamOnlineAnswer(tran, eachExamOnlineAnswer);
                }

                //插入日志
                for (int log1 = 0; log1 < arry_insertExamOnlineLog.Count; log1++)
                {
                    BLL.Util.InsertUserLog(tran, arry_insertExamOnlineLog[log1].ToString());
                }
                for (int log2 = 0; log2 < array_updateExamOnlineLog.Count; log2++)
                {
                    BLL.Util.InsertUserLog(tran, array_updateExamOnlineLog[log2].ToString());
                }
                for (int log3 = 0; log3 < array_insertExamOnlineDetailLog.Count; log3++)
                {
                    BLL.Util.InsertUserLog(tran, array_insertExamOnlineDetailLog[log3].ToString());
                }
                for (int log4 = 0; log4 < array_insertExamOnlineAnswerLog.Count; log4++)
                {
                    BLL.Util.InsertUserLog(tran, array_insertExamOnlineAnswerLog[log4].ToString());
                }

                //事务提交
                tran.Commit();

                msg = "{msg:'true'}";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{msg:'" + ex.Message.ToString() + "'}";
                BLL.Loger.Log4Net.Error("在线考试异常", ex);
            }
            finally
            {
                connection.Close();
            }

            #endregion
        }

        //插入在线考试表；返回主键，没有成功则返回0
        private int insertExamOnline(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            int eolid = 0;
            if (model != null)
            {
                eolid = BLL.ExamOnline.Instance.Insert(sqltran, model);
            }
            return eolid;
        }

        //更新在线考试表
        private void updateExamOnline(SqlTransaction sqltran, Entities.ExamOnline model)
        {
            if (model != null)
            {
                BLL.ExamOnline.Instance.Update(sqltran, model);
            }
        }

        //插入在线考试小题明细表
        private int insertOnlineDetail(SqlTransaction sqltran, Entities.ExamOnlineDetail model)
        {
            int eoldid = 0;
            if (model != null)
            {
                eoldid = BLL.ExamOnlineDetail.Instance.Insert(sqltran, model);
            }
            return eoldid;
        }

        //插入在线考试答案表
        private void insertExamOnlineAnswer(SqlTransaction sqltran, Entities.ExamOnlineAnswer model)
        {
            if (model != null)
            {
                int recID = BLL.ExamOnlineAnswer.Instance.Insert(sqltran, model);
            }
        }

        //判断交卷人是否有权限提交该试卷
        private void judgeSubmitExamLimit(int eiid, out string msg)
        {
            msg = string.Empty;
            int _examType;
            if (!int.TryParse(RequestExamType, out _examType))
            {
                return;
            }

            loginerIsInExamPaper(eiid, _examType, out msg);
            if (msg != string.Empty)
            {
                return;
            }

            switch (_examType)
            {
                case 0:
                case 1:
                    loginerIsSubmit(eiid, _examType, out msg);
                    break;
                default: msg = "{msg:'问卷异常！'}";
                    break;
            }
        }

        //判断提交人是否在试卷项目内；eiid：试卷ID；type-1补考；type-2正常考试
        private void loginerIsInExamPaper(int eiid, int type, out string msg)
        {
            msg = string.Empty;

            Entities.QueryExamPerson query_examPerson = new Entities.QueryExamPerson();
            switch (type)
            {
                case 0:
                    query_examPerson.EIID = eiid;
                    break;
                case 1:
                    query_examPerson.MEIID = eiid;
                    break;
            }
            query_examPerson.ExamType = type;
            query_examPerson.ExamPerSonID = loginerID;
            int count;
            DataTable dt = BLL.ExamPerson.Instance.GetExamPerson(query_examPerson, "", 1, 10000, out count);
            if (dt.Rows.Count == 0)
            {
                //没有在该套试卷中找到登陆人，不允许访问该页面
                msg = "{msg:'你不在该考试项目中！'}";
                return;
            }

        }

        //判断提交人是否已提交过该试卷；eiid：试卷ID；type：试卷类型 0-正常考试；1-补考
        private void loginerIsSubmit(int eiid, int type, out string msg)
        {
            msg = string.Empty;
            Entities.QueryExamOnline query = new Entities.QueryExamOnline();
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
            if (dt.Rows.Count == 1)
            {
                //存在状态为提交的记录
                if (dt.Rows[0]["Status"].ToString() == "1")
                {
                    msg = "{msg:'您已提交本次考试试卷，无法再次提交！'}";
                    return;
                }
            }
            if (dt.Rows.Count > 1)
            {
                msg = "{msg:'本次考试试卷您已存在多条记录，提交存在异常，无法访问该页面请联系开发人员！'}";
                return;
            }
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