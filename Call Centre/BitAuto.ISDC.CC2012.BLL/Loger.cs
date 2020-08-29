using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using System.Linq;
using log4net.Appender;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BitAuto.ISDC.CC2012.BLL
{
    /// <summary>
    /// 封装了日志类
    /// </summary>
    public class Loger
    {
        private static readonly object lockobject = new object();
        private static Dictionary<string, log4net.ILog> Logdic = new Dictionary<string, ILog>();

        /// <summary>
        /// 通过此Property获得日志实例的引用
        /// </summary>
        public static log4net.ILog Log4Net
        {
            get
            {
                string key = "";
                string userid = Util.GetLoginUserIDNotCheck();
                if (userid == "" || userid == null)
                {
                    key = "{BitAuto.ISDC.CC2012.BLL.Loger}";
                }
                else
                {
                    key = "{" + userid + " - " + Util.GetLoginRealName() + "}";
                }
                if (Logdic == null)
                {
                    lock (lockobject)
                    {
                        if (Logdic == null)
                        {
                            Logdic = new Dictionary<string, ILog>();
                        }
                    }
                }
                if (!Logdic.ContainsKey(key))
                {
                    lock (lockobject)
                    {
                        if (!Logdic.ContainsKey(key))
                        {
                            Logdic.Add(key, LogManager.GetLogger(key));
                        }
                    }
                }
                return Logdic[key];
            }
        }
    }
}
