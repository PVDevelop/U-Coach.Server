using MongoDB.Bson;
using System;

namespace PVDevelop.UCoach.Server.Mongo
{
    [MongoCollection("Versions")]
    public class CollectionVersion
    {
        public const int VERSION = 1;

        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public Guid Id { get; set; }

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
            Id = Guid.NewGuid();
        }
    }
}
