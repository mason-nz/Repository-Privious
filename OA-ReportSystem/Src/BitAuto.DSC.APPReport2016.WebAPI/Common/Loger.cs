using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BitAuto.DSC.APPReport2016.WebAPI.Common
{
    /// <summary>
    /// 封装了日志类
    /// </summary>
    public class Loger
    {
        /// <summary>
        /// log4Net的引用成员
        /// </summary>
        private static log4net.ILog m_log4Net = LogManager.GetLogger("APPReport_WebApi");

        /// <summary>
        /// 通过此Property获得日志实例的引用
        /// </summary>
        public static log4net.ILog Log4Net
        {
            get { return Loger.m_log4Net; }
        }
    }
}