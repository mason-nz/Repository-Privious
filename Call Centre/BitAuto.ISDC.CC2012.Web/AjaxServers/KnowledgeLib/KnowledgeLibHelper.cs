using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using System.Text;
using BitAuto.Utils.Config;
using System.IO;
using System.Data;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{

    public class KnowledgeLibHelper
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string CheckedInfoStr
        {
            get
            {
                if (Request["CheckedInfoStr"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CheckedInfoStr"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestCommonKID
        {
            get
            {
                if (Request["kid"] != null)
                {
                    return HttpUtility.UrlDecode(Request["kid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// HTMl
        /// </summary>
        public string HtmlStr
        {
            get
            {
                if (HttpContext.Current.Request["HtmlStr"] != null)
                {
                    string html = HttpContext.Current.Request["HtmlStr"].ToString();
                    html = html.Replace("+", "%2B");
                    return HttpContext.Current.Server.UrlDecode(html);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string RequestKLIDS
        {
            get
            {
                if (Request["KLID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["KLID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestKCID
        {
            get
            {
                if (HttpContext.Current.Request["KCID"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["KCID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="msg"></param>
        /// <param name="action">操作类型（save:保存  sub:提交）</param>
        /// <param name="RequestSingleInfo">单独添加特殊知识点（singleFAQ,singleQuestion）</param>
        internal void SubmitCheckInfo(int userID, out string msg, string action, out int klid, bool isManager, string RequestSingleInfo)
        {

            klid = 0;
            msg = string.Empty;
            CheckInfos ci = (CheckInfos)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(CheckedInfoStr, typeof(CheckInfos));

            if (ci.Knowledgeinfo != null)
            {
                ci.Knowledgeinfo.Content = HtmlStr;
            }

            Entities.KnowledgeLib KnowledgeLib = null;
            List<Entities.KLUploadFile> KLUploadFile = null;
            string deleteFilesID = string.Empty;

            List<Entities.KLFAQ> faqinfo = null;
            string deleteFaqIDs = string.Empty;
            List<Entities.KLQuestion> KLQuestionList = null;
            string deleteOptionIDs = string.Empty;
            string deleteQuestionIDs = string.Empty;
            Entities.KLOptionLog optionLog = null;//操作日志
            int idT = 0;
            string newFilePath = ""; //新增的文件路径(用于事务提交后的删除)
            if (RequestSingleInfo.ToLower() == "singlefaq")
            {
                if (!int.TryParse(RequestKCID, out idT) || !int.TryParse(RequestCommonKID, out idT))
                {
                    msg = "类别KCID或者KID格式错误";
                }
                else
                {
                    ci.ValidateFAQ(out faqinfo, out deleteFaqIDs, out msg);
                }

            }
            else if (RequestSingleInfo.ToLower() == "singlequestion")
            {
                if (!int.TryParse(RequestKCID, out idT) || !int.TryParse(RequestCommonKID, out idT))
                {
                    msg = "类别KCID或者KID格式错误";
                }
                else
                {
                    ci.ValidateQuestion(out KLQuestionList, out deleteQuestionIDs, out deleteOptionIDs, out msg);
                }

            }
            else
            {
                ci.Validate(out faqinfo, out deleteFaqIDs, out KLUploadFile, out deleteFilesID, out KnowledgeLib, out KLQuestionList, out deleteQuestionIDs, out deleteOptionIDs, out msg);
            }



            if (!string.IsNullOrEmpty(msg))
            {
                return;
            }
            //如果验证通过

            //记录用户操作日志
            List<StringBuilder> listLogStr = new List<StringBuilder>();

            #region 获得最终知识点实体类

            StringBuilder sblogstr = new StringBuilder();

            //如果是单个FAQ或者Question时不操作知识点
            if (string.Compare(RequestSingleInfo, "singleFAQ", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(RequestSingleInfo, "singleQuestion", StringComparison.OrdinalIgnoreCase) != 0)
            {
                if (KnowledgeLib.KLID > 0)
                {
                    //edit
                    Entities.KnowledgeLib oldKnowledge = null;
                    oldKnowledge = BLL.KnowledgeLib.Instance.GetKnowledgeLib(KnowledgeLib.KLID);
                    if (oldKnowledge == null)
                    {
                        msg = "没有找到原有的知识点信息";
                        return;
                    }

                    #region 记录日志

                    sblogstr = new StringBuilder();
                    sblogstr.Append("编辑了知识点【" + oldKnowledge.Title + "】:");
                    if (KnowledgeLib.Title != oldKnowledge.Title)
                    {
                        sblogstr.Append("把标题【" + oldKnowledge.Title + "】修改为【" + KnowledgeLib.Title + "】;");
                    }
                    if (KnowledgeLib.KCID != oldKnowledge.KCID)
                    {
                        sblogstr.Append("把分类ID【" + oldKnowledge.KCID + "】修改为【" + KnowledgeLib.KCID + "】;");
                    }

                    listLogStr.Add(sblogstr);

                    #endregion

                    oldKnowledge.KCID = KnowledgeLib.KCID;
                    oldKnowledge.Title = KnowledgeLib.Title;
                    oldKnowledge.Content = KnowledgeLib.Content;

                    oldKnowledge.Abstract = KnowledgeLib.Abstract;
                    oldKnowledge.UploadFileCount = KLUploadFile == null ? 0 : KLUploadFile.Count;
                    oldKnowledge.FAQCount = faqinfo == null ? 0 : faqinfo.Count;
                    oldKnowledge.QuestionCount = KLQuestionList == null ? 0 : KLQuestionList.Count;



                    KnowledgeLib = oldKnowledge;
                }
                else
                {
                    //add

                    #region 给实体赋值

                    KnowledgeLib.KLNum = "KL" + BLL.KnowledgeLib.Instance.GetCurrMaxID().ToString().PadLeft(7, '0');
                    KnowledgeLib.Status = 0;//0 未提交
                    KnowledgeLib.CreateTime = DateTime.Now;
                    KnowledgeLib.CreateUserID = userID;
                    KnowledgeLib.LastModifyTime = DateTime.Now;
                    KnowledgeLib.LastModifyUserID = userID;
                    KnowledgeLib.IsHistory = 0;
                    KnowledgeLib.RejectReason = "";
                    KnowledgeLib.UploadFileCount = 0;

                    KnowledgeLib.FAQCount = faqinfo == null ? 0 : faqinfo.Count;
                    KnowledgeLib.QuestionCount = KLQuestionList == null ? 0 : KLQuestionList.Count;//试题数量

                    #region 记录日志

                    sblogstr = new StringBuilder();
                    sblogstr.Append("添加了知识点:【知识点编号为：" + KnowledgeLib.KLNum + ";标题为：" + KnowledgeLib.Title + "】");
                    listLogStr.Add(sblogstr);

                    #endregion

                    #endregion
                }

                #region 验证提交

                if (!isManager)
                {
                    //不是管理员，才限制状态

                    if (KnowledgeLib != null)
                    {
                        if (action == "sub" && KnowledgeLib.Status != 0 && KnowledgeLib.Status != -1)
                        {
                            //不是“未提交”状态，也不是“驳回”状态，不能提交
                            msg += "当前状态不能提交";
                            return;
                        }
                    }
                }

                if (action == "sub")
                {
                    //如果是提交   1、待审核
                    KnowledgeLib.Status = 1;
                }
                else
                {
                    //如果是提交   0、未提交
                    KnowledgeLib.Status = 0;
                }

                #endregion
            }



            #endregion



            #region 获得编辑或者新增的文件实体类

            if (KLUploadFile != null)
            {
                KLUploadFile oldKlUploadFile;
                for (int n = 0; n < KLUploadFile.Count; n++)
                {
                    if (KLUploadFile[n].RecID > 0)
                    {
                        //编辑

                        oldKlUploadFile = BLL.KLUploadFile.Instance.GetKLUploadFile(KLUploadFile[n].RecID);
                        if (oldKlUploadFile != null)
                        {
                            #region 记录日志

                            sblogstr = new StringBuilder();
                            sblogstr.Append("修改了知识点附件信息【知识点编号：" + KnowledgeLib.KLNum + ";知识点标题为：" + KnowledgeLib.Title + "】:");
                            if (KLUploadFile[n].FilePath != oldKlUploadFile.FilePath)
                            {
                                sblogstr.Append("把文件路径【" + oldKlUploadFile.FilePath + "】修改为【" + KLUploadFile[n].FilePath + "】;");
                            }
                            if (KLUploadFile[n].Filename != oldKlUploadFile.Filename)
                            {
                                sblogstr.Append("把文件名称【" + oldKlUploadFile.Filename + "】修改为【" + KLUploadFile[n].Filename + "】;");
                            }
                            if (KLUploadFile[n].ExtendName != oldKlUploadFile.ExtendName)
                            {
                                sblogstr.Append("把文件扩展名【" + oldKlUploadFile.ExtendName + "】修改为【" + KLUploadFile[n].ExtendName + "】;");
                            }
                            if (KLUploadFile[n].FileSize != oldKlUploadFile.FileSize)
                            {
                                sblogstr.Append("把文件大小【" + oldKlUploadFile.FileSize + "】修改为【" + KLUploadFile[n].FileSize + "】;");
                            }

                            listLogStr.Add(sblogstr);

                            #endregion

                            oldKlUploadFile.FilePath = KLUploadFile[n].FilePath;
                            oldKlUploadFile.Filename = KLUploadFile[n].Filename;
                            oldKlUploadFile.ExtendName = KLUploadFile[n].ExtendName;
                            oldKlUploadFile.FileSize = KLUploadFile[n].FileSize;

                            KLUploadFile[n] = oldKlUploadFile;
                        }
                    }
                    else
                    {
                        //添加

                        KLUploadFile[n].KLID = KnowledgeLib.KLID;
                        KLUploadFile[n].ClickCount = 0;
                        KLUploadFile[n].CreateTime = DateTime.Now;
                        KLUploadFile[n].CreateUserID = userID;

                        #region 记录日志

                        sblogstr = new StringBuilder();
                        sblogstr.Append("添加了知识点的附件【知识点编号为：" + KnowledgeLib.KLNum + ";知识点标题为：" + KnowledgeLib.Title + "】：");
                        sblogstr.Append("【附件名称：" + KLUploadFile[n].Filename + ";附件扩展名：" + KLUploadFile[n].ExtendName + ";附件大小：" + KLUploadFile[n].FileSize + "】");
                        listLogStr.Add(sblogstr);

                        #endregion

                        newFilePath = newFilePath + "," + KLUploadFile[n].FilePath;

                    }
                }
                if (KLUploadFile.Count > 0)
                {
                    KnowledgeLib.FileUrl = KLUploadFile[0].FilePath;
                }
            }

            #endregion



            #region 获得编辑或者新增的FAQ实体类 并做FAQ操作日志
            StringBuilder faqlogstr = new StringBuilder();
            if (faqinfo != null)
            {
                for (int n = 0; n < faqinfo.Count; n++)
                {
                    faqinfo[n].ModifyTime = DateTime.Now;
                    faqinfo[n].ModifyUserID = userID;
                    faqinfo[n].KLID = 0;
                    if (faqinfo[n].KLFAQID == 0)
                    {
                        //新增
                        faqinfo[n].CreateTime = DateTime.Now;
                        faqinfo[n].CreateUserID = userID;
                        faqlogstr.Append("添加FAQ；Q：" + faqinfo[n].Question + "；A" + faqinfo[n].Ask + "。");
                    }
                    else
                    {
                        //修改
                        Entities.KLFAQ FAQOri = new Entities.KLFAQ();

                        FAQOri = BLL.KLFAQ.Instance.GetKLFAQ(faqinfo[n].KLFAQID);
                        if (FAQOri != null)
                        {
                            if (faqinfo[n].Ask != FAQOri.Ask || faqinfo[n].Question != FAQOri.Question)
                            {
                                faqlogstr.Append("修改FAQ（KLFAQID：" + faqinfo[n].KLFAQID.ToString() + "）；");
                                if (faqinfo[n].Question != FAQOri.Question)
                                {
                                    faqlogstr.Append("将Q由“" + FAQOri.Question + "”改为“" + faqinfo[n].Question + "”；");
                                }
                                else
                                {
                                    faqlogstr.Append("将A由“" + FAQOri.Ask + "”改为“" + faqinfo[n].Ask + "”；");
                                }
                            }
                            FAQOri.Ask = faqinfo[n].Ask;
                            FAQOri.Question = faqinfo[n].Question;
                            FAQOri.ModifyTime = faqinfo[n].ModifyTime;
                            FAQOri.ModifyUserID = faqinfo[n].ModifyUserID;
                            faqinfo[n] = FAQOri;
                        }
                    }
                }
            }

            #endregion

            #region 删除FAQ验证 并做FAQ操作日志

            if (deleteFaqIDs.Length > 0)
            {
                string[] fileIdsList = deleteFaqIDs.Split(',');
                foreach (string item in fileIdsList)
                {
                    Entities.KLFAQ FAQOri = new Entities.KLFAQ();
                    FAQOri = BLL.KLFAQ.Instance.GetKLFAQ(int.Parse(item));
                    faqlogstr.Append("删除FAQ；KLFAQID：" + item + "；Q：" + FAQOri.Question + "；A：" + FAQOri.Ask + "。");
                }
            }
            listLogStr.Add(faqlogstr);
            #endregion

            #region 获取试题实体赋值并做日志
            StringBuilder sbDeleteQuestionLogStr = new StringBuilder();
            StringBuilder sbDeleteOptionLogStr = new StringBuilder();

            if (KLQuestionList != null)
            {
                //删除删除的试题日志
                if (!string.IsNullOrEmpty(deleteQuestionIDs))
                {
                    foreach (string klqId in deleteQuestionIDs.Split(','))
                    {
                        Entities.KLQuestion questionInfo = BLL.KLQuestion.Instance.GetKLQuestion(int.Parse(klqId));
                        if (questionInfo != null)
                        {
                            sbDeleteQuestionLogStr.Append("删除试题【" + questionInfo.Ask + "】,试题ID【" + questionInfo.KLID + "】；");
                        }
                    }
                }
                //删除删除的选项日志
                if (!string.IsNullOrEmpty(deleteOptionIDs))
                {
                    foreach (string klaoId in deleteOptionIDs.Split(','))
                    {
                        Entities.KLAnswerOption optionInfo = BLL.KLAnswerOption.Instance.GetKLAnswerOption(int.Parse(klaoId));
                        if (optionInfo != null)
                        {
                            sbDeleteOptionLogStr.Append("删除试题ID【" + optionInfo.KLQID + "】下的选项【" + optionInfo.Answer + "】,选项ID【" + optionInfo.KLAOID + "】；");
                        }
                    }
                }
                //修改试题日志
                for (int i = 0; i < KLQuestionList.Count; i++)
                {
                    //如果试题ID大于0，进行修改操作，否则进行新增操作
                    StringBuilder sbQuestionDisployLogStr = new StringBuilder();
                    if (KLQuestionList[i].KLQID > 0)
                    {
                        #region 修改试题
                        Entities.KLQuestion questionInfo = BLL.KLQuestion.Instance.GetKLQuestion(KLQuestionList[i].KLQID);
                        if (questionInfo != null)
                        {
                            #region 比对数据
                            if (!questionInfo.Ask.Equals(KLQuestionList[i].Ask))
                            {
                                sbQuestionDisployLogStr.Append("修改试题ID【" + questionInfo.KLQID + "】,把试题【" + questionInfo.Ask + "】修改为【" + KLQuestionList[i].Ask + "】;");
                            }
                            #endregion
                            KLQuestionList[i].AskCategory = questionInfo.AskCategory;
                            KLQuestionList[i].CreateTime = questionInfo.CreateTime;
                            KLQuestionList[i].CreateUserID = questionInfo.CreateUserID;
                            KLQuestionList[i].Status = questionInfo.Status;
                            KLQuestionList[i].KLID = questionInfo.KLID;
                            KLQuestionList[i].ModifyTime = DateTime.Now;
                            KLQuestionList[i].ModifyUserID = userID;

                            if (KLQuestionList[i].OptionList != null)
                            {
                                List<long> answerOptionIDList = new List<long>();
                                for (int j = 0; j < KLQuestionList[i].OptionList.Count; j++)
                                {
                                    //如果选项存在ID，进行修改操作，否则进行新增操作
                                    if (KLQuestionList[i].OptionList[j].KLAOID > 0)
                                    {
                                        Entities.KLAnswerOption answerOptionInfo = BLL.KLAnswerOption.Instance.GetKLAnswerOption(KLQuestionList[i].OptionList[j].KLAOID);
                                        if (answerOptionInfo != null)
                                        {
                                            #region 选项数据比对
                                            if (!answerOptionInfo.Answer.Equals(KLQuestionList[i].OptionList[j].Answer))
                                            {
                                                sbQuestionDisployLogStr.Append("修改选项ID为【" + answerOptionInfo.KLAOID + "】,把选项名称【" + answerOptionInfo.Answer + "】修改为【" + KLQuestionList[i].OptionList[j].Answer + "】;");
                                            }
                                            #endregion

                                            KLQuestionList[i].OptionList[j].CreateTime = answerOptionInfo.CreateTime;
                                            KLQuestionList[i].OptionList[j].CreateUserID = answerOptionInfo.CreateUserID;
                                            KLQuestionList[i].OptionList[j].KLQID = answerOptionInfo.KLQID;
                                            KLQuestionList[i].OptionList[j].ModifyTime = DateTime.Now;
                                            KLQuestionList[i].OptionList[j].ModifyUserID = userID;

                                            answerOptionIDList.Add(KLQuestionList[i].OptionList[j].KLAOID);
                                        }
                                    }
                                    else
                                    {
                                        sbQuestionDisployLogStr.Append("添加选项【" + KLQuestionList[i].OptionList[j].Answer + "】;");
                                        KLQuestionList[i].OptionList[j].CreateTime = DateTime.Now;
                                        KLQuestionList[i].OptionList[j].CreateUserID = userID;
                                        KLQuestionList[i].OptionList[j].KLQID = KLQuestionList[i].KLQID;
                                    }
                                }
                                if (KLQuestionList[i].AnswerOptionIndexList != null)
                                {
                                    DataTable dt = BLL.KLQAnswer.Instance.GetKLQAnswerByKLQID(KLQuestionList[i].KLQID);
                                    string beforeDeleteStr = string.Empty;
                                    string lastDeleteStr = string.Empty;
                                    if (dt != null)
                                    {
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            beforeDeleteStr += dr["KLAOID"] + "，";
                                        }
                                    }
                                    sbQuestionDisployLogStr.Append("把答案【" + beforeDeleteStr + "】修改为当前答案");
                                }
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        #region 新增试题
                        KLQuestionList[i].CreateTime = DateTime.Now;
                        KLQuestionList[i].CreateUserID = userID;
                        KLQuestionList[i].Status = 0;
                        KLQuestionList[i].KLID = klid;//知识点ID

                        sbQuestionDisployLogStr.Append("新增试题【" + KLQuestionList[i].Ask + "】,类型为【" + KLQuestionList[i].AskCategory + "】；");
                        //int klqId = BLL.KLQuestion.Instance.Insert(tran, question);

                        if (KLQuestionList[i].OptionList != null)
                        {
                            List<int> optionIDList = new List<int>();
                            //新增选项
                            foreach (Entities.KLAnswerOption option in KLQuestionList[i].OptionList)
                            {
                                option.CreateUserID = userID;
                                option.CreateTime = DateTime.Now;
                                sbQuestionDisployLogStr.Append("新增选项【" + option.Answer + "】;");
                            }
                        }
                        #endregion
                    }
                    listLogStr.Add(sbQuestionDisployLogStr);
                }

            }
            #endregion

            #region 操作日志表

            optionLog = new KLOptionLog();
            if (KnowledgeLib != null)
            {
                if (KnowledgeLib.Status == 0)
                {
                    optionLog.OptStatus = (int)(EnumOptStatus.NoSubmit); //当前状态
                }
                else if (KnowledgeLib.Status == 1)
                {
                    optionLog.OptStatus = (int)(EnumOptStatus.WaitingAudit); //当前状态
                }
            }
            optionLog.Remark = "";
            optionLog.CreateTime = DateTime.Now;
            optionLog.CreateUserID = userID;

            #endregion

            #region 事务保存信息

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            //真实的数据操作
            try
            {
                klid = 0;

                #region 保存知识点

                if (KnowledgeLib != null)
                {
                    if (KnowledgeLib.KLID > 0)
                    {
                        //edit

                        klid = int.Parse(KnowledgeLib.KLID.ToString());
                        int retnum = BLL.KnowledgeLib.Instance.Update(tran, KnowledgeLib);
                    }
                    else
                    {
                        //add
                        klid = BLL.KnowledgeLib.Instance.Insert(tran, KnowledgeLib);
                    }
                }

                #endregion

                #region 保存文件

                if (KLUploadFile != null)
                {
                    foreach (KLUploadFile item in KLUploadFile)
                    {
                        if (item.RecID > 0)
                        {
                            //编辑
                            BLL.KLUploadFile.Instance.Update(tran, item);
                        }
                        else
                        {

                            item.KLID = klid;
                            BLL.KLUploadFile.Instance.Insert(tran, item);
                        }
                    }
                }



                #endregion


                #region 删除文件

                if (deleteFilesID.Length > 0)
                {
                    string[] fileIdsList = deleteFilesID.Split(',');
                    foreach (string item in fileIdsList)
                    {
                        BLL.KLUploadFile.Instance.Delete(tran, long.Parse(item));
                    }
                }



                #endregion

                #region 保存FAQ

                if (faqinfo != null)
                {
                    foreach (KLFAQ item in faqinfo)
                    {
                        if (item.KLFAQID == 0)
                        {
                            //new
                            if (RequestSingleInfo.ToLower() == "singlefaq")
                            {
                                item.KCID = Convert.ToInt32(RequestKCID);
                                item.KLID = Convert.ToInt32(RequestCommonKID);
                            }
                            else
                            {
                                item.KLID = klid;
                            }


                            BLL.KLFAQ.Instance.Insert(tran, item);

                        }
                        else
                        {
                            int n = BLL.KLFAQ.Instance.Update(tran, item);
                        }
                    }
                }

                #endregion

                #region 删除FAQ

                if (deleteFaqIDs.Length > 0)
                {
                    string[] fileIdsList = deleteFaqIDs.Split(',');
                    foreach (string item in fileIdsList)
                    {
                        BLL.KLFAQ.Instance.Delete(tran, long.Parse(item));
                    }
                }
                #endregion

                #region 保存试题

                if (KLQuestionList != null)
                {
                    //删除删除的试题
                    if (!string.IsNullOrEmpty(deleteQuestionIDs))
                    {
                        foreach (string klqId in deleteQuestionIDs.Split(','))
                        {
                            BLL.KLQuestion.Instance.Delete(tran, long.Parse(klqId));
                        }
                    }
                    //删除删除的选项
                    if (!string.IsNullOrEmpty(deleteOptionIDs))
                    {
                        foreach (string klaoId in deleteOptionIDs.Split(','))
                        {
                            BLL.KLAnswerOption.Instance.Delete(tran, long.Parse(klaoId));
                        }
                    }
                    //修改试题
                    foreach (Entities.KLQuestion question in KLQuestionList)
                    {
                        //如果试题ID大于0，进行修改操作，否则进行新增操作
                        if (question.KLQID > 0)
                        {
                            #region 修改试题

                            //修改试题
                            BLL.KLQuestion.Instance.Update(tran, question);
                            if (question.OptionList != null)
                            {
                                List<long> answerOptionIDList = new List<long>();
                                foreach (Entities.KLAnswerOption option in question.OptionList)
                                {
                                    //如果选项存在ID，进行修改操作，否则进行新增操作
                                    if (option.KLAOID > 0)
                                    {
                                        BLL.KLAnswerOption.Instance.Update(tran, option);
                                        answerOptionIDList.Add(option.KLAOID);
                                    }
                                    else
                                    {
                                        option.CreateTime = DateTime.Now;
                                        option.CreateUserID = userID;
                                        option.KLQID = question.KLQID;
                                        answerOptionIDList.Add(BLL.KLAnswerOption.Instance.Insert(tran, option));
                                    }
                                }
                                if (question.AnswerOptionIndexList != null)
                                {
                                    //先删除该试题的所有答案
                                    BLL.KLQAnswer.Instance.Delete(tran, question.KLQID);
                                    //重新插入正确答案
                                    foreach (int index in question.AnswerOptionIndexList)
                                    {
                                        Entities.KLQAnswer answerModel = new KLQAnswer();
                                        answerModel.CreateTime = DateTime.Now;
                                        answerModel.CreateUserID = userID;
                                        answerModel.KLAOID = (int)answerOptionIDList[index];
                                        answerModel.KLQID = question.KLQID;

                                        BLL.KLQAnswer.Instance.Insert(tran, answerModel);
                                    }
                                }
                            }


                            #endregion
                        }
                        else
                        {
                            #region 新增试题

                            if (RequestSingleInfo.ToLower() == "singlequestion")
                            {
                                question.KCID = Convert.ToInt32(RequestKCID);
                                question.KLID = Convert.ToInt32(RequestCommonKID);
                            }
                            else
                            {
                                question.KLID = klid;//知识点ID
                            }
                            int klqId = BLL.KLQuestion.Instance.Insert(tran, question);

                            if (question.OptionList != null)
                            {
                                List<int> optionIDList = new List<int>();
                                //新增选项
                                foreach (Entities.KLAnswerOption option in question.OptionList)
                                {
                                    option.KLQID = klqId;

                                    optionIDList.Add(BLL.KLAnswerOption.Instance.Insert(tran, option));
                                }
                                if (question.AnswerOptionIndexList != null)
                                {
                                    //新增答案
                                    foreach (int index in question.AnswerOptionIndexList)
                                    {
                                        Entities.KLQAnswer answerModel = new KLQAnswer();
                                        answerModel.CreateTime = DateTime.Now;
                                        answerModel.CreateUserID = userID;
                                        answerModel.KLAOID = optionIDList[index];
                                        answerModel.KLQID = klqId;

                                        BLL.KLQAnswer.Instance.Insert(tran, answerModel);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    if (sbDeleteQuestionLogStr.Length > 0)
                    {
                        BLL.Util.InsertUserLog(tran, sbDeleteQuestionLogStr.ToString());
                    }
                    if (sbDeleteOptionLogStr.Length > 0)
                    {
                        BLL.Util.InsertUserLog(tran, sbDeleteOptionLogStr.ToString());
                    }


                }
                #endregion

                #region 保存知识库操作日志

                optionLog.KLID = klid;
                BLL.KLOptionLog.Instance.Insert(tran, optionLog);

                #endregion

                #region 保存用户操作日志

                foreach (StringBuilder sbStr in listLogStr)
                {
                    BLL.Util.InsertUserLog(tran, sbStr.ToString());
                }

                #endregion

                tran.Commit();

                #region 物理删除文件

                if (deleteFilesID != "")
                {
                    DeleteFilesByIDs(deleteFilesID);
                }

                #endregion

                if (KnowledgeLib != null)
                {
                    BLL.KnowledgeLib.Instance.UpdateHtml(KnowledgeLib.KLID, KnowledgeLib.Content);
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = ex.Message.ToString();

                if (newFilePath != "")
                {
                    newFilePath = newFilePath.Substring(1);
                    DeleteFilesByPath(newFilePath);
                }

            }
            finally
            {
                connection.Close();
            }

            #endregion



        }

        /// <summary>
        /// 事务失败后，根据路径删除物理文件
        /// </summary>
        /// <param name="newFilePath"></param>
        private void DeleteFilesByPath(string newFilePath)
        {
            string[] list = newFilePath.Split(',');
            foreach (string item in list)
            {
                FileInfo finfo = new FileInfo(BLL.Util.GetUploadWebRoot() + item);
                finfo.Delete();
            }
        }

        /// <summary>
        /// 根据IDs删除物理文件
        /// </summary>
        /// <param name="deleteFilesID"></param>
        private void DeleteFilesByIDs(string deleteFilesID)
        {
            Entities.KLUploadFile model;
            string filePath = "";
            string[] list = deleteFilesID.Split(',');
            foreach (string item in list)
            {
                filePath = "";
                model = BLL.KLUploadFile.Instance.GetKLUploadFile(long.Parse(item));
                if (model != null)
                {
                    filePath = BLL.Util.GetUploadWebRoot() + model.FilePath;
                    FileInfo finfo = new FileInfo(filePath);
                    finfo.Delete();
                }
            }
        }

        internal void BatchAuditKnowledgeLib(bool isPass, out string msg)
        {
            msg = string.Empty;

            #region 验证知识点ID
            if (string.IsNullOrEmpty(RequestKLIDS))
            {
                msg = "请选择知识点";
                return;
            }
            else
            {
                foreach (string klIdStr in RequestKLIDS.Split(','))
                {
                    int klid = 0;
                    if (!int.TryParse(klIdStr, out klid))
                    {
                        msg = "知识点ID转换成int类型失败";
                        return;
                    }
                }
            }

            #endregion
            //if (int.TryParse(RequestKLID, out klId))
            //{
            //   Entities.KnowledgeLib info = BLL.KnowledgeLib.Instance.GetKnowledgeLib(klId);
            //   if (info != null)
            //   {
            //       if(isPass)
            //   }
            //   else
            //   {
            //       msg = "不存在此ID的知识点";
            //   }
            //}
            //else
            //{
            //    msg = "知识点转换成int类型失败";
            //}
        }

        private void AuditKnowledgeLib(int klId, bool isPass, string RejectReason)
        {
            Entities.KnowledgeLib info = BLL.KnowledgeLib.Instance.GetKnowledgeLib(klId);
            if (info != null)
            {
                Entities.KLOptionLog OptionLogModel = new KLOptionLog();
                if (isPass)
                {
                    //审批通过
                    info.Status = 2;
                    OptionLogModel.OptStatus = (int)Entities.EnumOptStatus.Release;
                }
                else
                {
                    info.Status = -1;
                    info.RejectReason = RejectReason;
                    OptionLogModel.OptStatus = (int)Entities.EnumOptStatus.Reject;
                }
                BLL.KnowledgeLib.Instance.Update(info);

                OptionLogModel.CreateTime = DateTime.Now;
                OptionLogModel.CreateUserID = BLL.Util.GetLoginUserID();
                OptionLogModel.KLID = klId;
                OptionLogModel.Remark = RejectReason;
                BLL.KLOptionLog.Instance.Insert(OptionLogModel);
            }
        }
    }

    public class CheckInfos
    {
        public KnowledgeInfo Knowledgeinfo;
        public List<UploadFileInfo> fileinfo;
        public List<FAQInfo> faqinfo;
        public string DeleteFAQIDs;

        public string DeleteFilesIDs;
        public List<CheckKLQuestion> KLQuestions;
        public string DeleteQuestionIDs;
        public string DeleteOptionIDs;


        public void ValidateFAQ(out List<Entities.KLFAQ> faqList, out string deleteFaqIDs, out string msg)
        {
            deleteFaqIDs = string.Empty;
            msg = string.Empty;


            faqList = new List<KLFAQ>();
            KLFAQ tempKLFaq;
            foreach (FAQInfo item in faqinfo)
            {
                tempKLFaq = new KLFAQ();
                long longVal = 0;

                if (!long.TryParse(item.faqID, out longVal))
                {
                    msg += "FAQID转换成Int类型失败";
                }
                tempKLFaq.KLFAQID = longVal;

                tempKLFaq.Question = item.faq_Q;
                tempKLFaq.Ask = item.faq_A;
                //zxq
                faqList.Add(tempKLFaq);
            }



            #region 删除验证
            if (!string.IsNullOrEmpty(DeleteFAQIDs.Trim()))
            {
                DeleteFAQIDs = BLL.KLFAQ.Instance.removeLastComma(DeleteFAQIDs);
                foreach (string FAQId in DeleteFAQIDs.Trim().Split(','))
                {
                    int deleteQOID = 0;
                    if (!int.TryParse(FAQId, out deleteQOID))
                    {
                        msg += "删除FAQID转换成Int类型失败<br/>";
                        break;
                    }
                }
            }
            deleteFaqIDs = DeleteFAQIDs.Trim();
            #endregion
        }
        public void ValidateQuestion(out List<Entities.KLQuestion> questions, out string deleteQuestionIDs, out string deleteOptionIDs, out string msg)
        {
            msg = string.Empty;
            questions = null;
            deleteQuestionIDs = string.Empty;
            deleteOptionIDs = string.Empty;

            if (KLQuestions != null)
            {
                questions = new List<KLQuestion>();
                int i = 0;
                foreach (CheckKLQuestion klQuestion in KLQuestions)
                {
                    i = i + 1;
                    Entities.KLQuestion question = new KLQuestion();
                    int klqId = 0;

                    #region 试题验证
                    if (!string.IsNullOrEmpty(klQuestion.KLQID))
                    {
                        if (int.TryParse(klQuestion.KLQID, out klqId))
                        {
                            question.KLQID = klqId;
                        }
                        else
                        {
                            msg += "第" + i + "道试题【" + klQuestion.Ask + "】ID格式不正确<br />";
                        }
                    }
                    if (string.IsNullOrEmpty(klQuestion.Ask))
                    {
                        msg += "第" + i + "道试题问题不能为空<br />";
                    }
                    question.Ask = klQuestion.Ask;
                    int categoryId = 0;
                    if (!int.TryParse(klQuestion.AskCategory, out categoryId))
                    {
                        msg += "第" + i + "道试题【" + klQuestion.Ask + "】类别ID转换成int类型失败<br />";
                    }
                    question.AskCategory = categoryId;

                    #endregion

                    #region 试题选项验证
                    if (klQuestion.KLAnswerOptions != null)
                    {
                        List<Entities.KLAnswerOption> listOption = new List<KLAnswerOption>();
                        int j = 0;
                        foreach (CheckKLAnswerOption klAnswerOption in klQuestion.KLAnswerOptions)
                        {
                            j = j + 1;
                            Entities.KLAnswerOption option = new KLAnswerOption();
                            if (!string.IsNullOrEmpty(klAnswerOption.KLAOID))
                            {
                                int klaoId = 0;
                                if (!int.TryParse(klAnswerOption.KLAOID, out klaoId))
                                {
                                    msg += "第" + i + "道试题【" + klQuestion.Ask + "】第" + j + "个选项ID转换int类型失败<br/>";
                                }
                                option.KLAOID = klaoId;
                            }
                            if (string.IsNullOrEmpty(klAnswerOption.Answer))
                            {
                                msg += "第" + i + "道试题【" + klQuestion.Ask + "】第" + j + "个选项内容不能为空<br/>";
                            }
                            option.Answer = klAnswerOption.Answer;
                            option.KLQID = klqId;
                            listOption.Add(option);
                        }

                        question.OptionList = listOption;
                    }
                    #endregion

                    #region 正确答案验证
                    if (categoryId != 3 && string.IsNullOrEmpty(klQuestion.AnswerOptions))
                    {
                        msg += "第" + i + "道试题【" + klQuestion.Ask + "】没有选择答案<br/>";
                    }
                    else if (categoryId != 3 && !string.IsNullOrEmpty(klQuestion.AnswerOptions))
                    {
                        string[] answerOptionArry = klQuestion.AnswerOptions.Split(',');
                        List<int> answerOptionIndexList = new List<int>();
                        foreach (string answerOption in answerOptionArry)
                        {
                            int index = 0;
                            if (!int.TryParse(answerOption, out index))
                            {
                                msg += "第" + i + "道试题【" + klQuestion.Ask + "】答案转换成int类型失败<br/>";
                                break;
                            }
                            answerOptionIndexList.Add(index);
                        }

                        question.AnswerOptionIndexList = answerOptionIndexList;
                    }
                    #endregion

                    questions.Add(question);
                }

                #region 删除试题验证
                if (!string.IsNullOrEmpty(DeleteQuestionIDs))
                {
                    foreach (string questionId in DeleteQuestionIDs.Trim().Split(','))
                    {
                        int deleteQID = 0;
                        if (!int.TryParse(questionId, out deleteQID))
                        {
                            msg += "删除试题ID转换成Int类型失败<br/>";
                            break;
                        }
                    }
                }
                deleteQuestionIDs = DeleteQuestionIDs;
                #endregion

                #region 删除选项验证
                if (!string.IsNullOrEmpty(deleteOptionIDs.Trim()))
                {
                    foreach (string optionId in deleteOptionIDs.Trim().Split(','))
                    {
                        int deleteQOID = 0;
                        if (!int.TryParse(optionId, out deleteQOID))
                        {
                            msg += "删除试题选项ID转换成Int类型失败<br/>";
                            break;
                        }
                    }
                }
                deleteOptionIDs = DeleteOptionIDs;
                #endregion
            }
        }

        public void Validate(out List<Entities.KLFAQ> faqList, out string deleteFaqIDs, out List<Entities.KLUploadFile> kluploadfile, out string deleteFilesIDs, out Entities.KnowledgeLib knowledgelib, out List<Entities.KLQuestion> questions, out string deleteQuestionIDs, out string deleteOptionIDs, out string msg)//save or submit
        {

            deleteFilesIDs = string.Empty;

            msg = string.Empty;


            #region 试题验证

            ValidateQuestion(out questions, out deleteQuestionIDs, out deleteOptionIDs, out msg);

            #endregion

            #region 验证知识点

            knowledgelib = new Entities.KnowledgeLib();

            #region 验证

            int intVal = 0;

            if (Knowledgeinfo == null)
            {
                msg += "知识点信息为空！";
            }
            if (Knowledgeinfo.KLID != "")
            {
                if (!int.TryParse(Knowledgeinfo.KLID, out intVal))
                {
                    msg += "知识点ID应为数字！";
                }
            }
            knowledgelib.KLID = intVal;

            if (Knowledgeinfo.KCID == "")
            {
                msg += "知识点分类为空";
            }
            else
            {
                if (!int.TryParse(Knowledgeinfo.KCID, out intVal))
                {
                    msg += "知识点分类ID应为数字！";
                }
            }
            knowledgelib.KCID = intVal;


            #endregion

            #region 给试题类赋值

            knowledgelib.Title = Knowledgeinfo.Title;
            knowledgelib.Content = Knowledgeinfo.Content;
            knowledgelib.Abstract = Knowledgeinfo.KAbstract;

            #endregion

            #endregion

            #region 验证文件

            #region 验证新增和编辑的文件信息

            kluploadfile = new List<KLUploadFile>();
            KLUploadFile uploadFile;
            foreach (UploadFileInfo item in fileinfo)
            {
                long longVal = 0;

                uploadFile = new KLUploadFile();
                if (item.RecID != null && item.RecID != "" && !long.TryParse(item.RecID, out longVal))
                {
                    msg += "文件ID应该为数字";
                }
                uploadFile.RecID = longVal;
                uploadFile.FilePath = HttpUtility.UrlDecode(item.filePath);
                uploadFile.Filename = HttpUtility.UrlDecode(item.filename);
                uploadFile.ExtendName = HttpUtility.UrlDecode(item.ExtendName);

                if (!long.TryParse(item.fileSize, out longVal))
                {
                    msg += "文件大小应该为数字";
                }
                uploadFile.FileSize = intVal;

                kluploadfile.Add(uploadFile);
            }

            #endregion

            #region 验证删除的文件IDs

            if (DeleteFilesIDs.Length > 0)
            {
                long longVal = 0;
                string[] list = DeleteFilesIDs.Split(',');
                foreach (string item in list)
                {
                    if (!long.TryParse(item, out longVal))
                    {
                        msg += "删除的文件ID列表格式不正确";
                        break;
                    }
                }
                deleteFilesIDs = DeleteFilesIDs;
            }

            #endregion

            #endregion

            #region 验证FAQ

            ValidateFAQ(out faqList, out deleteFaqIDs, out msg);

            #endregion
        }

        #region 知识点相关

        /// <summary>
        /// 知识点信息
        /// </summary>
        public class KnowledgeInfo
        {
            private string klid;
            public string KLID { get { return klid; } set { klid = HttpUtility.UrlDecode(value); } }

            private string title;
            public string Title { get { return title; } set { title = HttpUtility.UrlDecode(value); } }

            private string content;
            public string Content
            {
                get { return content; }
                set
                {
                    content = HttpUtility.UrlDecode(value.Replace("+", "%2B"));

                }
            }

            /// <summary>
            /// 摘要
            /// </summary>
            private string Kabstract;
            public string KAbstract { get { return Kabstract; } set { Kabstract = HttpUtility.UrlDecode(value); } }

            private string kcid;
            public string KCID { get { return kcid; } set { kcid = HttpUtility.UrlDecode(value); } }

        }

        #endregion

        #region 上传文件相关

        public class UploadFileInfo
        {
            private string _result;
            public string result { get { return _result; } set { _result = HttpUtility.UrlDecode(value); } }

            private string recid;
            public string RecID { get { return recid; } set { recid = HttpUtility.UrlDecode(value); } }

            public string filePath;
            public string FilePath { get { return filePath; } set { filePath = HttpUtility.UrlDecode(value); } }

            public string filename;
            public string FileName { get { return filename; } set { filename = HttpUtility.UrlDecode(value); } }

            public string extName;
            public string ExtendName { get { return extName; } set { extName = HttpUtility.UrlDecode(value); } }

            public string fileSize;
            public string FileSize { get { return fileSize; } set { fileSize = HttpUtility.UrlDecode(value); } }
        }

        #endregion

        #region FAQ相关

        /// <summary>
        /// FAQ信息
        /// </summary>
        public class FAQInfo
        {
            private string faqid;
            public string faqID
            {
                get
                {
                    return faqid;
                }
                set
                {
                    faqid = HttpUtility.UrlDecode(value);
                }
            }
            private string ask;
            public string faq_A
            {
                get
                {
                    return ask;
                }
                set
                {
                    ask = HttpUtility.UrlDecode(value);
                }
            }

            private string question;
            public string faq_Q
            {
                get
                {
                    return question;
                }
                set
                {
                    question = HttpUtility.UrlDecode(value);
                }
            }
        }

        #endregion

        #region 试题相关
        public class CheckKLQuestion
        {
            private string klqid;
            public string KLQID
            {
                get
                {
                    return klqid;
                }
                set
                {
                    klqid = HttpUtility.UrlDecode(value);
                }
            }

            private string askcategory;
            public string AskCategory
            {
                get
                {
                    return askcategory;
                }
                set
                {
                    askcategory = HttpUtility.UrlDecode(value);
                }
            }

            private string ask;
            public string Ask
            {
                get
                {
                    return ask;
                }
                set
                {
                    ask = HttpUtility.UrlDecode(value);
                }
            }

            private string answeroptions;
            public string AnswerOptions
            {
                get
                {
                    return answeroptions;
                }
                set
                {
                    answeroptions = HttpUtility.UrlDecode(value);
                }
            }

            private List<CheckKLAnswerOption> klansweroptions;
            public List<CheckKLAnswerOption> KLAnswerOptions
            {
                get
                {
                    return klansweroptions;
                }
                set
                {
                    klansweroptions = value;
                }
            }
        }
        public class CheckKLAnswerOption
        {
            private string klaoid;
            public string KLAOID
            {
                get
                {
                    return klaoid;
                }
                set
                {
                    klaoid = HttpUtility.UrlDecode(value);
                }
            }
            private string klqid;
            public string KLQID
            {
                get
                {
                    return klqid;
                }
                set
                {
                    klqid = HttpUtility.UrlDecode(value);
                }
            }
            private string answer;
            public string Answer
            {
                get
                {
                    return answer;
                }
                set
                {
                    answer = HttpUtility.UrlDecode(value);
                }
            }
        }
        #endregion
    }

}