using System;
using log4net;

namespace XYAuto.BUOC.ChiTuData2017.Test.Logging
{
    public interface ILogHelper
    {
        void Debug(string format, params object[] args);

        void Debug(string message);

        void Error(string message, params object[] args);

        void Error(string message, Exception ex);

        void Error(string message);

        void Info(string format, params object[] args);

        void Info(string message);

        void Warn(string format, params object[] args);

        void Warn(string message);
    }

    public sealed class LogHelper : ILogHelper
    {
        private static readonly ILog Logger;

        static LogHelper()
        {
            log4net.Config.XmlConfigurator.Configure();

            Logger = LogManager.GetLogger("BaseLog");
        }

        public void Debug(string format, params object[] args)
        {
            Logger.Debug(string.Format(format, args));
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Error(string message, params object[] args)
        {
            Logger.Error(string.Format(message, args));
        }

        public void Error(string message, Exception ex)
        {
            Logger.Error(message, ex);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Info(string format, params object[] args)
        {
            Logger.Info(string.Format(format, args));
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            Logger.Info(message, ex);
        }

        public void Warn(string format, params object[] args)
        {
            Logger.Warn(string.Format(format, args));
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public bool IsDebugEnabled
        {
            get { return Logger.IsDebugEnabled; }
        }
    }
}