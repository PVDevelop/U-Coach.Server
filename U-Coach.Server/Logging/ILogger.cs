using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Logging
{
    public interface ILogger
    {
        void Debug(Exception exception, string message, params object[] args);

        void Debug(string message, params object[] args);

        void Info(Exception exception, string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(Exception exception, string message, params object[] args);

        void Warning(string message, params object[] args);

        void Error(Exception exception, string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(Exception exception, string message, params object[] args);

        void Fatal(string message, params object[] args);
    }
}
