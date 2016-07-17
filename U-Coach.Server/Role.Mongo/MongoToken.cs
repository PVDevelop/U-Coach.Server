using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Mongo
{
    public class MongoToken
    {
        public TokenId Id { get; set; }
        public UserId UserId { get; set; }
        public AuthTokenParams TokenParams { get; set; }
    }
}
