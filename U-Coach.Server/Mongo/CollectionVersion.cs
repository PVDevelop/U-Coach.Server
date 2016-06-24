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
    public class CollectionVersion
    {
        public const int VERSION = 1;

        /// <summary>
        /// Id документа
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Версия документа
        /// </summary>
        public int Version { get; set; }

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
