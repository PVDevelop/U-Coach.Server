﻿using System;
using NLog;

namespace PVDevelop.UCoach.Server.Logging
{
    public class Logger<T>
    {
        private readonly Lazy<ILogger> _log;

        static Logger()
        {
            //XmlConfigurator.Configure();
        }

        public Logger()
        {
            _log = new Lazy<ILogger>(() => LogManager.GetLogger(typeof(T).AssemblyQualifiedName));
        }

        public void Debug(Exception exception, string message, params object[] args)
        {
            _log.Value.Debug(exception, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            _log.Value.Debug(message, args);
        }

        public void Info(Exception exception, string message, params object[] args)
        {
            _log.Value.Info(exception, message, args);
        }

        public void Info(string message, params object[] args)
        {
            _log.Value.Info(message, args);
        }

        public void Warning(Exception exception, string message, params object[] args)
        {
            _log.Value.Warn(exception, message, args);
        }

        public void Warning(string message, params object[] args)
        {
            _log.Value.Warn(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _log.Value.Error(exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            _log.Value.Error(message, args);
        }

        public void Fatal(Exception exception, string message, params object[] args)
        {
            _log.Value.Fatal(exception, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            _log.Value.Fatal(message, args);
        }
    }
}
