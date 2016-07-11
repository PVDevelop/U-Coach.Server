using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    [MongoCollection("Users")]
    public class MongoUser
    {
        public UserId Id { get; set; }
        public AuthToken Token { get; set; }
    }
}
