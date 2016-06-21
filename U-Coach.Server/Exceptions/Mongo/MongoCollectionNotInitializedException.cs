using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Exceptions.Mongo
{
    public class MongoCollectionNotInitializedException : Exception
    {
        public int CurrentVersion { get; private set; }

        public int ExpectedVersion { get; private set; }

        public MongoCollectionNotInitializedException(int currentVersion, int expectedVersion)
        {
            CurrentVersion = currentVersion;
            ExpectedVersion = expectedVersion;
        }
    }
}
