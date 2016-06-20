using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Exceptions.Mongo
{
    public class InvalidDataVersionException : Exception
    {
        public int CurrentVersion { get; set; }
        public int RequiredVersion { get; set; }

        public InvalidDataVersionException(int currentVersion, int requiredVersion)
        {
            CurrentVersion = currentVersion;
            RequiredVersion = requiredVersion;
        }
    }
}
