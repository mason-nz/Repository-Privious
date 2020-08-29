using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.SessionState;
using System.Data.SqlClient;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.Utils.Config;
using System.Data;
namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject
{
    /// <summary>
    /// SubExamProjec 的摘要说明
    /// </summary>
    public class SubExamProjec : IHttpHandler,IRequiresSessionState
    {
        #region 参数
        /// <summary>
        /// 操作类型（save:保存  complate:完成）
        /// </summary>
        public string Action
        {
            get
            {
                if (HttpContext.Current.Request["Action"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ExamPersonIDs
        {
            get
            {
                if (HttpContext.Current.Request["ExamPersonIDs"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ExamPersonIDs"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string MakeExamPersonIDs
        {
            get
            {
                if (HttpContext.Current.Request["MakeExamPersonIDs"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MakeExamPersonIDs"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public string ExamStartTime
        {
            get
            {
                if (HttpContext.Current.Request["ExamStartTime"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ExamStartTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public string ExamEndTime
        {
            get
            {
                if (HttpContext.Current.Request["ExamEndTime"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ExamEndTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public string MakeStartTime
        {
            get
            {
                if (HttpContext.Current.Request["MakeStartTime"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MakeStartTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        
        public string MakeEndTime
        {
            get
            {
                if (HttpContext.Current.Request["MakeEndTime"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MakeEndTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public IList<Entities.ExamPerson> examPersionList = new List<Entities.ExamPerson>();
        public IList<Entities.ExamPerson> makeExamPersionList = new List<Entities.ExamPerson>();
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            formateModel();
            if (checkData(out msg))
            {
                if (subDate(out msg))
                {
                    context.Response.Write("{'result':'success','msg':'" + msg + "'}");
                }
                else
                {
                    context.Response.Write("{'result':'error','msg':'" + msg + "'}"); 
                }
                
            }
            else
            {
                context.Response.Write("{'result':'error','msg':'" + msg + "'}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 对象化数据

        public Entities.ExamInfo examObj = new Entities.ExamInfo();
        public Entities.MakeUpExamInfo makeExamOjb = new Entities.MakeUpExamInfo();
        
        public void formateModel()
        {
            //考试项目
            examObj.EIID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ExObjID"].ToString()));
            examObj.BusinessGroup = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Group"].ToString());
            examObj.CreaetUserID = BLL.Util.GetLoginUserID();
            examObj.CreateTime = DateTime.Now;
            examObj.Description = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Description"].ToString());
            examObj.ECID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ECID"].ToString()));//分类ID
            //examObj.EPID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["EPID"].ToString()));//试卷ID
            //examObj.ExamEndTime
            examObj.IsMakeUp = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["IsMakeUp"].ToString()));
            //examObj.JoinNum
            examObj.LastModifyTime = DateTime.Now;
            examObj.LastModifyUserID = BLL.Util.GetLoginUserID();
            examObj.Name = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Title"].ToString());

            examObj.BGID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"].ToString()));

            //补考
            makeExamOjb.CreateTime = DateTime.Now;
            makeExamOjb.CreateUserID = BLL.Util.GetLoginUserID();
            makeExamOjb.EIID = examObj.EIID;
            //makeExamOjb.JoinNum
            //makeExamOjb.MakeUpEPID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["makeUpEcid"].ToString()));

            

        }
        #endregion

        #region 数据验证

        public bool checkData(out string meg)
        {
            meg = "";

            #region 普通考试验证

            #region 常规验证
            if (Action == "complate")
            {
                examObj.Status = 1;
            }
            else
            {
                examObj.Status = 0;
            }
            if (examObj.Name == "")
            {
                meg = "考试名称不可为空！";
                return false;
            }
            if (examObj.Description == "")
            {
                meg = "考试说明不可为空！";
                return false;
            }
            int epID = 0;
            //试卷ID
            if (!int.TryParse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["EPID"].ToString()),out epID))
            {
                meg = "试卷ID转化失败，请重试。";
                return false;
            }
            else
            {
                examObj.EPID = epID;
            }

            //参考人员
            string examPersonIDs = BLL.Util.removeLastComma(ExamPersonIDs);
            string[] examPersonIDsArr = examPersonIDs.Split(',');
            foreach (string EmID in examPersonIDsArr)
            {
                int emID = 0;
                if (int.TryParse(EmID, out emID))
                {
                    Entities.ExamPerson examPersion = new Entities.ExamPerson();
                    examPersion.CreateTime = DateTime.Now;
                    examPersion.CreateUserID = BLL.Util.GetLoginUserID();
                    //examPersion.EIID
                    examPersion.ExamPerSonID = emID;
                    examPersion.ExamType = 0;
                    examPersionList.Add(examPersion);
                }
                else
                {
                    meg = "参考（普通）人员ID装换失败！";
                    return false;
                }
            }

            #endregion

            #region 考试时间验证
            DateTime time;
            if (!DateTime.TryParse(ExamStartTime, out time))
            {
                meg = "考试开始时间格式不正确！";
                return false;
            }
            else
            {
                examObj.ExamStartTime = time;
            }

            if (!DateTime.TryParse(ExamEndTime, out time))
            {
                meg = "考试开始时间格式不正确！";
                return false;
            }
            else
            {
                examObj.ExamEndTime = time;
            }
            if (examObj.EIID == 0 )
            {
                if (examObj.ExamStartTime < DateTime.Now)
                {
                    meg = "考试开始时不得晚于当前时间！";
                    return false;
                }
            }
            if (examObj.ExamStartTime > examObj.ExamEndTime)
            {
                meg = "考试结束时不得早于开考时间！";
                return false;
            }
            #endregion

            #endregion

            #region 补考验证
            if (examObj.IsMakeUp == 1)
            {
                if (!DateTime.TryParse(MakeStartTime, out time))
                {
                    meg = "考试开始时间格式不正确！";
                    return false;
                }
                else
                {
                    makeExamOjb.MakeUpExamStartTime = time;
                }

                if (!DateTime.TryParse(MakeEndTime, out time))
                {
                    meg = "考试开始时间格式不正确！";
                    return false;
                }
                else
                {
                    makeExamOjb.MakeupExamEndTime = time;
                }

                if (examObj.EIID == 0 || examObj.Status == 0)
                {
                    if (makeExamOjb.MakeUpExamStartTime < DateTime.Now)
                    {
                        meg = "补考考试开始时不得晚于当前时间！";
                        return false;
                    }
                }

                if (makeExamOjb.MakeupExamEndTime < makeExamOjb.MakeUpExamStartTime)
                {
                    meg = "补考考试结束时不得晚于开考时间！";
                    return false;
                }
                if (makeExamOjb.MakeUpExamStartTime < examObj.ExamEndTime)
                {
                    meg = "补考开始时间不得早于正常考试结束时间！";
                    return false;
                }
                makeExamOjb.MakeUpEPID = Convert.ToInt32(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["makeUpEcid"].ToString()));
                //
                //参考人员
                string makeExamPersonIDs = BLL.Util.removeLastComma(MakeExamPersonIDs);
                string[] makeExamPersonIDsArr = makeExamPersonIDs.Split(',');
                foreach (string EmID in makeExamPersonIDsArr)
                {
                    int emID = 0;
                    if (int.TryParse(EmID, out emID))
                    {
                        Entities.ExamPerson examPersion = new Entities.ExamPerson();
                        examPersion.CreateTime = DateTime.Now;
                        examPersion.CreateUserID = BLL.Util.GetLoginUserID();
                        //examPersion.EIID
                        examPersion.ExamPerSonID = emID;
                        examPersion.ExamType = 1;
                        makeExamPersionList.Add(examPersion);
                    }
                    else
                    {
                        meg = "参考（补考）人员ID装换失败！";
                        return false;
                    }
                }
            }

            #endregion

            return true;
        }
        
        #endregion

        #region 数据操作
        public bool subDate(out string msg)
        {
            bool result = false;
            msg = "";
            string log = "";
            #region 事务保存信息

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            //真实的数据操作
            try
            {
                if (examObj.EIID > 0)
                {//编辑
                    Entities.ExamInfo examObjOri = new Entities.ExamInfo();
                    examObjOri = BLL.ExamInfo.Instance.GetExamInfo(examObj.EIID);
                    examObj.JoinNum = examPersionList.Count;
                    BLL.ExamInfo.Instance.Update(tran, examObj);
                    //先清除参考人员
                    BLL.ExamPerson.Instance.Delete(examObj.EIID);
                    foreach (Entities.ExamPerson examPersion in examPersionList)
                    {
                        examPersion.EIID = examObj.EIID;
                        try
                        {
                            BLL.ExamPerson.Instance.Insert(tran, examPersion);
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message.ToString().Replace("'", "");
                            msg = msg.Replace("\r\n", "");
                        }
                    }
                    //清除补考
                    BLL.MakeUpExamInfo.Instance.Delete(tran,(int)examObj.EIID);
                    
                    if (examObj.IsMakeUp == 1)
                    {//添加新补考
                        makeExamOjb.JoinNum = makeExamPersionList.Count;
                        makeExamOjb.EIID = examObj.EIID;
                        makeExamOjb.MEIID = BLL.MakeUpExamInfo.Instance.Insert(tran, makeExamOjb);
                        foreach (Entities.ExamPerson examPersion in makeExamPersionList)
                        {
                            examPersion.MEIID = makeExamOjb.MEIID;
                            examPersion.EIID = makeExamOjb.EIID;
                            try
                            {
                                BLL.ExamPerson.Instance.Insert(tran, examPersion);
                            }
                            catch (Exception ex)
                            {
                                msg = ex.Message.ToString().Replace("'", "");
                                msg = msg.Replace("\r\n", "");
                            }
                        }
                        log += "；补考信息更新为-补考MEIID：" + makeExamOjb.MEIID + "；开始时间："
                            + makeExamOjb.MakeUpExamStartTime + "；结束时间：" + makeExamOjb.MakeupExamEndTime;
                    }
                }
                else
                {//添加
                    examObj.JoinNum = examPersionList.Count;
                    examObj.EIID = BLL.ExamInfo.Instance.Insert(tran,examObj);

                    foreach(Entities.ExamPerson examPersion in examPersionList)
                    {
                        examPersion.EIID = examObj.EIID;
                        try {
                            BLL.ExamPerson.Instance.Insert(tran, examPersion);
                        }
                        catch (Exception ex)
                        {
                            msg = ex.Message.ToString().Replace("'", "");
                            msg = msg.Replace("\r\n", "");
                        }
                    }
                    log = "添加考试项目-EIID：" + examObj.EIID + "；标题：" + examObj.Name + "；考试说明："
                        + examObj.Description + "；开始时间：" + examObj.ExamStartTime + "；结束时间：" + examObj.ExamEndTime;
                    if (examObj.IsMakeUp == 1)
                    {//补考
                        makeExamOjb.JoinNum = makeExamPersionList.Count;
                        makeExamOjb.EIID = examObj.EIID;
                        makeExamOjb.MEIID = BLL.MakeUpExamInfo.Instance.Insert(tran,makeExamOjb);
                        foreach (Entities.ExamPerson examPersion in makeExamPersionList)
                        {
                            examPersion.MEIID = makeExamOjb.MEIID;
                            examPersion.EIID = makeExamOjb.EIID;
                            try
                            {
                                BLL.ExamPerson.Instance.Insert(tran, examPersion);
                            }
                            catch (Exception ex)
                            {
                                msg = ex.Message.ToString().Replace("'", "");
                                msg = msg.Replace("\r\n", "");
                            }
                        }
                        log += "；补考信息-补考MEIID：" + makeExamOjb.MEIID + "；开始时间："
                            + makeExamOjb.MakeUpExamStartTime + "；结束时间：" + makeExamOjb.MakeupExamEndTime;
                    }
                }            

                tran.Commit();
                //写入日志
                BLL.Util.InsertUserLog(log);
                result = true;
            }
            catch (Exception ex)
            {

                tran.Rollback();
                msg = ex.Message.ToString().Replace("'","");
                msg = msg.Replace("\r\n","");
            }
            finally
            {
                connection.Close();
            }
            return result;
            #endregion
        }
        #endregion
    }
}