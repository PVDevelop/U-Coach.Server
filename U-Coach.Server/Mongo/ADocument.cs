using MongoDB.Bson;

namespace PVDevelop.UCoach.Server.Mongo
{
    public abstract class ADocument : IAmDocument
    {
        public ObjectId Id { get; set; }

        public int Version { get; set; }

        protected ADocument(int version)
        {
            Version = version;
        }
    }
}
