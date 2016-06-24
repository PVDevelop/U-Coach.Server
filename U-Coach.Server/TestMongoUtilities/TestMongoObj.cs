using MongoDB.Bson;
using PVDevelop.UCoach.Server.Mongo;

namespace TestMongoUtilities
{
    [MongoCollection("TestObj")]
    [MongoDataVersion(456)]
    public sealed class TestMongoObj
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }

}
