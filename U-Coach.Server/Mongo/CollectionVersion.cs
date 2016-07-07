using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization.Attributes;

namespace PVDevelop.UCoach.Server.Mongo
{
    [MongoCollection("Versions")]
    public class CollectionVersion
    {
        public const int VERSION = 1;

        /// <summary>
        /// Имя коллекциии, на которую ссылается данный документ
        /// </summary>
        [BsonId]
        public string TargetCollectionName { get; private set; }

        /// <summary>
        /// Версия документа
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Версия целевой коллекции.
        /// </summary>
        public int TargetVersion { get; set; }

        public CollectionVersion(string targetCollectionName)
        {
            if(targetCollectionName == null)
            {
                throw new ArgumentNullException("targetCollectionName");
            }

            Version = 1;
            TargetCollectionName = targetCollectionName;
        }
    }
}
