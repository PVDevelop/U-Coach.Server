using MongoDB.Bson.Serialization.Attributes;
using System;

namespace PVDevelop.UCoach.Server.Mongo
{
    public interface IHaveId
    {
        [BsonId]
        Guid Id { get; }
    }
}
