using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamPaperStorage
{
    /// <summary>
    /// ExamPaper 的摘要说明
    /// </summary>
    public class ExamPaper : IHttpHandler, IRequiresSessionState
    {

        #region 参数

        /// <summary>
        /// 操作
        /// </summary>
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["action"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["action"].ToString());
            }
        }

        /// <summary>
        /// 试卷ID
        /// </summary>
        public string Epid
        {
            get
            {
                return HttpContext.Current.Request["epid"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["epid"].ToString());
            }
        }


        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";



            switch (Action)
            {
                case "DelPager":
                    DeletePager(out msg);
                    break;
            }
            if (msg != "")
            {
                context.Response.Write(msg);
            }
            else
            {
                context.Response.Write("success");
            }
        }

        #region 删除试卷

        /// <summary>
        /// 删除试卷
        /// </summary>
        /// <param name="msg"></param>
        private void DeletePager(out string msg)
        {
            msg = "";
            CheckedDelPar(out msg);
            if (msg == "")
            {


                Entities.ExamPaper paperModel = BLL.ExamPaper.Instance.GetExamPaper(long.Parse(Epid));
                if (paperModel != null)
                {
                    Entities.QueryExamBigQuestion query = new Entities.QueryExamBigQuestion();
                    query.EPID = paperModel.EPID;

                    int totalCount = 0;
                    DataTable dt = BLL.ExamBigQuestion.Instance.GetExamBigQuestion(query, "", 1, 9999, out totalCount);

                    string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
                    SqlConnection connection = new SqlConnection(connectionstrings);
                    connection.Open();
                    SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

                    try
                    {
                        int retVal = BLL.ExamPaper.Instance.Delete(tran, int.Parse(Epid));
                        if (retVal > 0)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    retVal = BLL.ExamBigQuestion.Instance.Delete(tran, long.Parse(dr["BQID"].ToString()));
                                }
                            }
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        msg = ex.Message.ToString();
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
                else
                {
                    msg = "没找到对应试卷";
                }


            }
        }

        /// <summary>
        /// 检查删除参数
        /// </summary>
        /// <param name="msg"></param>
        private void CheckedDelPar(out string msg)
        {
            msg = "";
            long intval = 0;
            if (Epid == string.Empty)
            {
                msg = "缺少试卷ID参数";
                return;
            }
            if (!long.TryParse(Epid, out intval))
            {
                msg = "试卷ID参数格式不正确";
                return;
            }
            if (BLL.ExamInfo.Instance.GetExamPaperUsedCount(intval) > 0)
            {
                msg = "此试卷已经被使用，不能删除";
                return;
            }
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}