using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage
{
    /// <summary>
    /// ExamScoreManagement 的摘要说明
    /// </summary>
    public class ExamScoreManagement : IHttpHandler, IRequiresSessionState
    {
        #region 属性定义
        //请求处理程序
        private string RequestAction
        {
            get
            {
                return HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }
        //大题：小题：分数
        private string subscore
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["subscore"]) == true ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["subscore"]);
            }
        }
        //客户id
        private string EOLID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["EOLID"]) == true ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["EOLID"]);
            }
        }

        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            switch (RequestAction)
            {
                //提交阅卷
                case "SubScore":
                    subscoreM(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        public void subscoreM(out string msg)
        {
            msg = "";
            try
            {
                if (subscore != "" && EOLID != "")
                {
                    string[] score = subscore.Split(',');
                    Entities.ExamOnline mode = null;
                    mode = BLL.ExamOnline.Instance.GetExamOnline(Convert.ToInt32(EOLID));
                    if (mode.IsMarking != 1)
                    {
                        int sumscore = 0;
                        for (int i = 0; i < score.Length; i++)
                        {
                            BLL.ExamOnlineDetail.Instance.UpdateByEOLID(EOLID, score[i].Split(':')[0], score[i].Split(':')[1], score[i].Split(':')[2]);

                            BLL.Util.InsertUserLog("更新ExamOnlineDetail表，把在线考试ID为" + EOLID + ",大题ID为" + score[i].Split(':')[0] + "小题ID为" + score[i].Split(':')[1] + "的记录的Score更新为" + score[i].Split(':')[2]);
                            sumscore += Convert.ToInt32(score[i].Split(':')[2]);
                        }
                        BLL.Util.InsertUserLog("更新ExamOnline表，把在线考试ID为" + EOLID + "的记录的SumScore从" + mode.SumScore + "更新为" + (mode.SumScore + sumscore) + ",isMarking更新为" + "1");
                        mode.IsMarking = 1;
                        mode.SumScore = mode.SumScore + sumscore;
                        BLL.ExamOnline.Instance.Update(mode);
                        msg = "success";
                    }
                    else
                    {
                        msg = "此试卷已阅，不能重复阅卷！";
                    }
                }
                
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
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