using log4net;
using log4net.Config;
using System;

namespace PVDevelop.UCoach.Server.Logging
{
    public class Logger<T>
    {
        private readonly Lazy<ILog> _log;

        static Logger()
        {
            XmlConfigurator.Configure();
        }

        public Logger()
        {
            _log = new Lazy<ILog>(() => LogManager.GetLogger(typeof(T)));
        }

        public void Debug(Exception exception, string message, params object[] args)
        {
            _log.Value.Debug(string.Format(message, args), exception);
        }

        public void Debug(string message, params object[] args)
        {
            _log.Value.DebugFormat(message, args);
        }

        public void Info(Exception exception, string message, params object[] args)
        {
            _log.Value.Info(string.Format(message, args), exception);
        }

        public void Info(string message, params object[] args)
        {
            _log.Value.InfoFormat(message, args);
        }

        public void Warning(Exception exception, string message, params object[] args)
        {
            _log.Value.Warn(string.Format(message, args), exception);
        }

        public void Warning(string message, params object[] args)
        {
            _log.Value.WarnFormat(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _log.Value.Error(string.Format(message, args), exception);
        }

        public void Error(string message, params object[] args)
        {
            _log.Value.ErrorFormat(message, args);
        }
    }
}
