using MongoDB.Bson;

namespace PVDevelop.UCoach.Server.Mongo
{
    public interface IAmDocument
    {
        ObjectId Id { get; }
        int Version { get; }
    }
}
