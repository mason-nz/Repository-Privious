/********************************************************
*创建人：hant
*创建时间：2018/1/31 14:22:21 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/
using log4net;

namespace XYAuto.BUOC.Chitunion2018.SyncWeiXinUser.Common
{
    public class Loger
    {
        static Loger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// log4Net的引用成员
        /// </summary>
        /// 
        private static log4net.ILog infoLog = LogManager.GetLogger("infoLog");
        private static log4net.ILog errorLog = log4net.LogManager.GetLogger("errorLog");

        /// <summary>
        /// 通过此Property获得日志实例的引用
        /// </summary>
        public static log4net.ILog InfoLog
        {
            get { return Loger.infoLog; }
        }
        public static log4net.ILog ErrorLog
        {
            get { return errorLog; }
        }
    }
}
