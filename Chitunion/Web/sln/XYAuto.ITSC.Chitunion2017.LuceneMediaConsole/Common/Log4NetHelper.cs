
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common
{
    public class Log4NetHelper
    {
        private readonly static log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 用于调试时，输出信息（DEBUG级消息）。
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(object message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }
        public static void Debug(object message, Exception ex)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message, ex);
            }
        }

        public static void Info(object message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }


        /// <summary>
        /// 将日志记录到数据库中
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string operateType, string describe, string username, string strSql, params string[] sqlparams)
        {
            if (log.IsInfoEnabled)
            {
                string strParams = string.Join(",", sqlparams);

                var obj = new { OperateType = operateType, Describe = describe, ExeSql = strSql, SqlParams = strParams, UserName = username, OperateTime = DateTime.Now };

                //log4net.MDC.Set("OperateType", operateType);
                //log4net.MDC.Set("Describe", describe);
                //log4net.MDC.Set("ExeSql", strSql);
                //log4net.MDC.Set("SqlParams",strParams );
                //log4net.MDC.Set("UserName", username);
                //log4net.MDC.Set("OperateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                log.Info(obj);
            }
        }

        /// <summary>
        /// 用于输出程序异常信息（ERROR级消息）。
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex">Exception</param>
        public static void Error(object message, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
                //异常时发邮件

                try
                {
                    ////int issendEmail = ConfigHelper.GetAppSettingInt32Value("IsSendEmail");

                    //if (issendEmail == 1)
                    //{
                    //    DateTime dateTime = DateTime.Now;

                    //    //EmailLogWriter email = new EmailLogWriter();
                    //    StringBuilder hmessage = new StringBuilder();

                    //    hmessage.AppendLine(dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    //    hmessage.AppendLine("输出信息：" + ex.Message);

                    //    hmessage.AppendLine("异常信息：\r\n" + ex.ToString());

                    //    hmessage.AppendLine("异常堆栈：\r\n" + ex.StackTrace);

                    //    //string strtoEmail = ConfigurationManager.AppSettings["ToEmail"].ToString(); //收件人


                    //    string[] toEmails = strtoEmail.Split(',');
                    //    foreach (string toEmail in toEmails)
                    //    {
                    //        email.SendEmail(toEmail, ex.Message, hmessage.ToString());
                    //    }
                    // }
                }
                catch (Exception e)
                {
                    log.Error("发送邮件错误", e);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op">操作名称</param>
        /// <param name="sql"></param>
        /// <param name="model">实体类型的参数</param>
        public static void DebugSqlAndParam(string op, string sql, object model)
        {
            //  var now = $"{DateTime.Now:yyMMdd}";
            Type modelType = model.GetType();
            StringBuilder mProperStr = new StringBuilder();
            foreach (var mt in modelType.GetProperties())
            {
                mProperStr.Append(mt.Name);
                mProperStr.Append("=");
                if (mt.GetValue(model) != null)
                    mProperStr.Append(mt.GetValue(model));
                mProperStr.Append("|");

            }
            Log4NetHelper.Debug($"{op}sql:{sql};model参数:{mProperStr.ToString()};");
        }
    }
}
