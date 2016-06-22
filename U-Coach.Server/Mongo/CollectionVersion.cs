using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Mongo
{
    [MongoCollection("Versions")]
    public class CollectionVersion : IAmDocument
    {
        public const int VERSION = 1;

        public ObjectId Id { get; private set; }

        public int Version { get; private set; }

        /// <summary>
        /// Версия целевой коллекции.
        /// </summary>
        public int TargetVersion { get; set; }

        [MongoIndexName("name")]
        public string Name { get; set; }

        public CollectionVersion()
        {
            Version = 1;
        }
    }
}
