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
    /// ExamInfoDel 的摘要说明
    /// </summary>
    public class ExamInfoDel : IHttpHandler,IRequiresSessionState
    {
        #region 参数
        public string EIID
        {
            get
            {
                if (HttpContext.Current.Request["EIID"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["EIID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string State
        {
            get
            {
                if (HttpContext.Current.Request["State"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["State"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            Entities.ExamInfo examInfo = new Entities.ExamInfo();
            examInfo = BLL.ExamInfo.Instance.GetExamInfo(long.Parse(EIID));
            examInfo.Status = Convert.ToInt32(State);
            examInfo.LastModifyTime = DateTime.Now;
            examInfo.LastModifyUserID =BLL.Util.GetLoginUserID();
            BLL.ExamInfo.Instance.Update(examInfo);
            string log = "";
            if (State == "1")
            {
                log = "将EIID为'"+examInfo.EIID+"',名称为'"+examInfo.Name+"'的考试项目设置为完成。";
            }
            else if(State=="-1")
            {
                log = "将EIID为'" + examInfo.EIID + "',名称为'" + examInfo.Name + "'的考试项目删除。";
            }
            BLL.Util.InsertUserLog(log);

            context.Response.Write("success");
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