using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PVDevelop.UCoach.Server.Mongo
{
    public interface IHaveId
    {
        [BsonId]
        ObjectId Id { get; }
    }
}
