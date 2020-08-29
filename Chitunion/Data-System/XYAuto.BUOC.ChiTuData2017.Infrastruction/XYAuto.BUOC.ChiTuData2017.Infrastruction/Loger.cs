using log4net;
using log4net.Repository.Hierarchy;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace XYAuto.BUOC.ChiTuData2017.Infrastruction
{
    /// <summary>
    /// 封装了日志类
    /// </summary>
    public class Loger
    {
        static Loger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// log4Net的引用成员
        /// </summary>
        private static log4net.ILog m_log4Net = LogManager.GetLogger("BaseLog");

        private static log4net.ILog zhyLogger = log4net.LogManager.GetLogger("ZHYLog");

        /// <summary>
        /// 通过此Property获得日志实例的引用
        /// </summary>
        public static log4net.ILog Log4Net
        {
            get { return Loger.m_log4Net; }
        }

        /// <summary>
        /// 对智慧云相关api的日志实例
        /// </summary>
        public static log4net.ILog ZhyLogger
        {
            get { return zhyLogger; }
        }
    }
}