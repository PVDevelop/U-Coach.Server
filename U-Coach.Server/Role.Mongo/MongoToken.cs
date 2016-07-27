using System;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    [MongoCollection("Tokens")]
    [MongoDataVersion(VERSION)]
    public class MongoToken
    {
        public const int VERSION = 1;

        public int Version { get; private set; }

        public TokenId Id { get; set; }

        public UserId UserId { get; set; }

        public AuthUserId AuthUserId { get; set; } 

        public DateTime Expiration { get; set; }

        public AuthSystemToken AuthToken { get; set; }

        public bool IsDeleted { get; set; }

        public MongoToken()
        {
            Version = VERSION;
        }
    }
}
