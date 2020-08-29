using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage;
using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamPaperStorage
{
    /// <summary>
    /// ExamPaperSave 的摘要说明
    /// </summary>
    public class ExamPaperSave : IHttpHandler, IRequiresSessionState
    {
        public string ExamPaperInfoStr
        {
            get
            {
                return HttpContext.Current.Request["ExamPaperInfoStr"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["ExamPaperInfoStr"].ToString());
            }
        }

        /// <summary>
        /// 保存或者提交 （保存：save  提交: sub）
        /// </summary>
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";
            int userID = 0;
            int epid = 0;//返回的试卷ID
            try
            {
                if (BLL.Util.CheckButtonRight("SYS024BUT3201"))
                {
                    userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                    Submit(out msg, userID, out epid);
                }
                else
                {
                    msg = "您没有操作试卷的权限！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                BLL.Util.SendErrorEmail(msg, ex.Source, ex.StackTrace);
            }

            string retJson = "";

            if (msg == "")
            {
                retJson += "{'result':'";
                retJson += "success','epid':'";
                retJson += epid.ToString() + "'}";
                context.Response.Write(retJson);
            }
            else
            {
                retJson += "{'result':'";
                retJson += "err','epid':'";
                retJson += msg + "'}";
                context.Response.Write(retJson);
            }
        }

        private void Submit(out string msg, int userID, out int epid)
        {
            msg = "";
            epid = 0;//返回的试卷ID

            //将页面的上的数据转换成类
            ExamPaperPageInfo Info = (ExamPaperPageInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(ExamPaperInfoStr, typeof(ExamPaperPageInfo));

            Entities.ExamPaper examPaper = null;
            List<Entities.ExamBigQuestion> bigQList = null;
            List<Entities.ExamBSQuestionShip> shipList = null;
            string delBigQIDs = string.Empty;
            string DelShipIDs = string.Empty;


            //验证数据并转换成实体类
            Info.Validate(out examPaper, out bigQList, out shipList, out delBigQIDs, out msg);

            if (string.IsNullOrEmpty(msg))
            {
                //如果验证通过               

                //记录用户操作日志
                List<StringBuilder> listLogStr = new List<StringBuilder>();
                StringBuilder sblogstr = new StringBuilder();

                #region 获得最终的试卷实体类

                if (examPaper.EPID > 0)
                {
                    //编辑
                    Entities.ExamPaper oldExamPaper = null;
                    oldExamPaper = BLL.ExamPaper.Instance.GetExamPaper((long)examPaper.EPID);
                    if (oldExamPaper != null)
                    {

                        #region 判断

                        if (oldExamPaper.Status == (int)ExamPaperState.InUsed)
                        {
                            msg = "已使用状态的试卷不能编辑";
                            return;
                        }
                        if (oldExamPaper.Status == (int)ExamPaperState.NotUsed && Action == "save")
                        {
                            msg = "未使用状态的试卷不能进行保存操作，只能提交";
                            return;
                        }

                        if (oldExamPaper.Status == -1)
                        {
                            msg = "已删除的试卷不能编辑";
                            return;
                        }

                        #endregion

                        #region 记日志

                        sblogstr = new StringBuilder();
                        string logstr = "";

                        if (oldExamPaper.Name != examPaper.Name)
                        {
                            logstr += "将试卷名称由‘" + oldExamPaper.Name + "’改为‘" + examPaper.Name + "’,";
                        }
                        if (oldExamPaper.ECID != examPaper.ECID)
                        {
                            logstr += "将试卷分类由‘" + oldExamPaper.ECID + "’改为‘" + examPaper.ECID + "’,";
                        }
                        if (oldExamPaper.ExamDesc != examPaper.ExamDesc)
                        {
                            logstr += "将考试说明由‘" + oldExamPaper.ExamDesc + "’改为‘" + examPaper.ExamDesc + "’,";
                        }
                        if (oldExamPaper.TotalScore != examPaper.TotalScore)
                        {
                            logstr += "将总分由‘" + oldExamPaper.TotalScore + "’改为‘" + examPaper.TotalScore + "’,";
                        }
                        if (logstr != "")
                        {
                            logstr += "编辑了试卷【" + oldExamPaper.Name + "】:" + logstr;
                        }
                        if (logstr != "")
                        {
                            sblogstr.Append(logstr);

                            listLogStr.Add(sblogstr);
                        }

                        #endregion

                        oldExamPaper.Name = examPaper.Name;
                        oldExamPaper.ECID = examPaper.ECID;
                        oldExamPaper.ExamDesc = examPaper.ExamDesc;
                        oldExamPaper.TotalScore = examPaper.TotalScore;
                        oldExamPaper.LastModifyTime = DateTime.Now;
                        oldExamPaper.LastModifyUserID = userID;
                        oldExamPaper.BGID = examPaper.BGID;
                        examPaper = oldExamPaper;

                    }
                    else
                    {
                        msg = "没有找到对应的试卷信息";
                        return;
                    }
                }
                else
                {

                    #region 记日志

                    sblogstr = new StringBuilder();
                    sblogstr.Append("添加了试卷【" + examPaper.Name + "】");
                    listLogStr.Add(sblogstr);

                    #endregion

                    //新增
                    examPaper.CreateTime = DateTime.Now;
                    examPaper.CreaetUserID = userID;
                    examPaper.LastModifyTime = DateTime.Now;
                    examPaper.LastModifyUserID = userID;
                }

                #region 根据保存或者提交，设置试卷状态

                if (Action == "save")
                {
                    #region 记日志

                    sblogstr = new StringBuilder();
                    sblogstr.Append("保存了试卷【" + examPaper.Name + "】");

                    listLogStr.Add(sblogstr);

                    #endregion

                    examPaper.Status = (int)Entities.ExamPaperState.NotComplete;
                }
                else if (Action == "sub")
                {
                    #region 记日志

                    sblogstr = new StringBuilder();
                    sblogstr.Append("提交了试卷【" + examPaper.Name + "】");

                    listLogStr.Add(sblogstr);

                    #endregion

                    examPaper.Status = (int)Entities.ExamPaperState.NotUsed;
                }
                else
                {
                    msg = "没有找到对应的试卷信息";
                    return;
                }
                #endregion

                #endregion

                #region 获得大题实体类

                if (bigQList != null)
                {
                    ExamBigQuestion oldBigQModel;

                    for (int i = 0; i < bigQList.Count; i++)
                    {
                        if (bigQList[i].BQID > 0)
                        {
                            //编辑
                            oldBigQModel = null;
                            oldBigQModel = BLL.ExamBigQuestion.Instance.GetExamBigQuestion((long)bigQList[i].BQID);

                            if (oldBigQModel == null)
                            {
                                msg = "没有找到ID为" + bigQList[i].BQID + "大题信息";
                                return;
                            }
                            else
                            {
                                #region 记日志

                                sblogstr = new StringBuilder();
                                string logstr = "";

                                sblogstr.Append("编辑了大题【" + oldBigQModel.Name + "】:");

                                if (oldBigQModel.Name != bigQList[i].Name)
                                {
                                    logstr += "将大题名称由‘" + oldBigQModel.Name + "’改为‘" + bigQList[i].Name + "’,";
                                }
                                if (oldBigQModel.BQDesc != bigQList[i].BQDesc)
                                {
                                    logstr += "将大题描述由‘" + oldBigQModel.BQDesc + "’改为‘" + bigQList[i].BQDesc + "’,";
                                }
                                if (oldBigQModel.AskCategory != bigQList[i].AskCategory)
                                {
                                    logstr += "将题型由‘" + oldBigQModel.AskCategory + "’改为‘" + bigQList[i].AskCategory + "’,";
                                }
                                if (oldBigQModel.EachQuestionScore != bigQList[i].EachQuestionScore)
                                {
                                    logstr += "将每题分值由‘" + oldBigQModel.EachQuestionScore + "’改为‘" + bigQList[i].EachQuestionScore + "’,";
                                }
                                if (oldBigQModel.QuestionCount != bigQList[i].QuestionCount)
                                {
                                    logstr += "将试题总量由‘" + oldBigQModel.QuestionCount + "’改为‘" + bigQList[i].QuestionCount + "’,";
                                }

                                if (logstr != "")
                                {
                                    sblogstr.Append(logstr);

                                    listLogStr.Add(sblogstr);
                                }

                                #endregion

                                oldBigQModel.Name = bigQList[i].Name;
                                oldBigQModel.BQDesc = bigQList[i].BQDesc;
                                oldBigQModel.AskCategory = bigQList[i].AskCategory;
                                oldBigQModel.EachQuestionScore = bigQList[i].EachQuestionScore;
                                oldBigQModel.QuestionCount = bigQList[i].QuestionCount;
                                oldBigQModel.NO = bigQList[i].NO;

                                bigQList[i] = oldBigQModel;
                            }
                        }
                        else
                        {
                            #region 记日志

                            sblogstr = new StringBuilder();
                            sblogstr.Append("添加了大题【" + bigQList[i].Name + "】");

                            listLogStr.Add(sblogstr);

                            #endregion

                            //新增
                            bigQList[i].Status = 0;
                            bigQList[i].CreateTime = DateTime.Now;
                            bigQList[i].CreateUserID = userID;
                            bigQList[i].ModifyTime = DateTime.Now;
                            bigQList[i].ModifyUserID = userID;
                        }
                    }
                }

                #endregion

                #region 获得大小题关系实体类

                if (shipList != null)
                {
                    Entities.ExamBSQuestionShip oldshipMode;

                    for (int i = 0; i < shipList.Count; i++)
                    {
                        if (shipList[i].BQID > 0)
                        {
                            //编辑的大题

                            oldshipMode = null;
                            oldshipMode = BLL.ExamBSQuestionShip.Instance.GetExamBSQuestionShip((long)shipList[i].BQID, (long)shipList[i].KLQID);
                            if (oldshipMode != null)
                            {
                                //编辑

                                Entities.KLQuestion klQuestion = BLL.KLQuestion.Instance.GetKLQuestion((long)shipList[i].KLQID);
                                if (klQuestion.Status == -1)
                                {
                                    msg = "小题【"+klQuestion.Ask+"】已经被删除";
                                    return;
                                }
                                oldshipMode.BQID = shipList[i].BQID;
                                oldshipMode.KLQID = shipList[i].KLQID;
                                oldshipMode.NO = shipList[i].NO;
                                shipList[i] = oldshipMode;
                            }
                            else
                            {
                                #region 记日志

                                sblogstr = new StringBuilder();
                                sblogstr.Append("添加了大题【" + shipList[i].BQID + "】下的小题【" + shipList[i].KLQID + "】");
                                listLogStr.Add(sblogstr);

                                #endregion

                                //新增
                                shipList[i].CreateTime = DateTime.Now;
                                shipList[i].CreateUserID = userID;
                            }

                        }
                        else
                        {
                            //新增的大题
                            shipList[i].CreateTime = DateTime.Now;
                            shipList[i].CreateUserID = userID;
                        }
                    }

                    #region 查出所有删除的小题

                    Entities.QueryExamBSQuestionShip shipquery;
                    foreach (Entities.ExamBigQuestion item in bigQList)//循环大题
                    {
                        if (item.BQID <= 0)
                        {
                            continue;
                        }
                        shipquery = new QueryExamBSQuestionShip();
                        shipquery.BQID = item.BQID;
                        int totalCount = 0;

                        //查找大题下的所有对应关系
                        DataTable olddt = BLL.ExamBSQuestionShip.Instance.GetExamBSQuestionShip(shipquery, "", 1, 9999, out totalCount);
                        if (olddt != null)
                        {
                            int f = 0;
                            foreach (DataRow dr in olddt.Rows)
                            {
                                f = 0;
                                foreach (Entities.ExamBSQuestionShip shipItem in shipList)
                                {
                                    if (dr["BQID"].ToString() == shipItem.BQID.ToString() && dr["KLQID"].ToString() == shipItem.KLQID.ToString())
                                    {
                                        //存在
                                        f = 1;
                                        break;
                                    }
                                }
                                if (f == 0)
                                {
                                    //如果不存在，就是删除了
                                    DelShipIDs = DelShipIDs + dr["BQID"].ToString() + "," + dr["KLQID"].ToString() + ";";
                                }
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region 事务提交

                string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
                SqlConnection connection = new SqlConnection(connectionstrings);
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

                //真实的数据操作
                try
                {
                    #region 保存试卷信息

                    if (examPaper.EPID > 0)
                    {
                        //edit
                        epid = (int)examPaper.EPID;

                        BLL.ExamPaper.Instance.Update(tran, examPaper);
                    }
                    else
                    {
                        //add
                        epid = BLL.ExamPaper.Instance.Insert(tran, examPaper);
                    }

                    #endregion

                    #region 保存大题和大小题关系

                    int bigQId = 0;
                    if (bigQList != null)
                    {
                        foreach (Entities.ExamBigQuestion item in bigQList)
                        {
                            if (item.BQID > 0)
                            {
                                //编辑
                                bigQId = (int)item.BQID;
                                BLL.ExamBigQuestion.Instance.Update(tran, item);
                            }
                            else
                            {
                                //添加
                                item.EPID = epid;
                                bigQId = BLL.ExamBigQuestion.Instance.Insert(tran, item);
                            }

                            #region 保存大小题关系信息

                            if (shipList != null)
                            {
                                foreach (Entities.ExamBSQuestionShip shipItem in shipList)
                                {
                                    if (shipItem.NO == item.NO)//当前大题的
                                    {
                                        shipItem.BQID = bigQId;
                                        BLL.ExamBSQuestionShip.Instance.Delete(tran, shipItem.BQID, shipItem.KLQID);
                                        BLL.ExamBSQuestionShip.Instance.Insert(tran, shipItem);
                                    }
                                }
                            }

                            #endregion


                        }
                    }

                    #endregion

                    #region 删除小题关系

                    if (DelShipIDs != string.Empty)
                    {
                        DelShipIDs = DelShipIDs.Substring(0, DelShipIDs.Length - 1);

                        string[] idGroup = DelShipIDs.Split(';');
                        foreach (string group in idGroup)
                        {
                            BLL.ExamBSQuestionShip.Instance.Delete(tran, long.Parse(group.Split(',')[0]), long.Parse(group.Split(',')[1]));
                        }
                    }

                    #endregion

                    #region 删除大题

                    if (!String.IsNullOrEmpty(delBigQIDs))
                    {
                        #region 记日志

                        sblogstr = new StringBuilder();
                        sblogstr.Append("删除了大题【" + delBigQIDs + "】");
                        listLogStr.Add(sblogstr);

                        #endregion

                        foreach (string item in delBigQIDs.Split(','))
                        {
                            BLL.ExamBigQuestion.Instance.Delete(tran, long.Parse(item));
                        }
                    }
                    #endregion

                    #region 保存用户操作日志

                    foreach (StringBuilder sbStr in listLogStr)
                    {
                        BLL.Util.InsertUserLog(tran, sbStr.ToString());
                    }
                    #endregion

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    msg = ex.Message.ToString();
                    BLL.Loger.Log4Net.Info(ex.ToString());
                }
                finally
                {
                    connection.Close();
                }

                #endregion

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