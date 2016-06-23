using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService
{
    [MongoCollection("Users")]
    [MongoDataVersion(1)]
    public class UserOrm : IAmDocument
    {
        public const int VERSION = 1;

        public ObjectId Id { get; private set; }

        public int Version { get; private set; }

        [MongoIndexName("login")]
        public string Login { get; private set; }

        public string Password { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public DateTime LastAuthenticationTime { get; private set; }

        public DateTime CreationTime { get; private set; }

        internal UserOrm()
        {
            Version = VERSION;
        }
    }
}
