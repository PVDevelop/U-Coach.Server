using System;

namespace PVDevelop.UCoach.Server.Mongo.Exceptions
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
