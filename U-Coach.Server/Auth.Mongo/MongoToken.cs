using PVDevelop.UCoach.Server.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    [MongoCollection("Tokens")]
    [MongoDataVersion(VERSION)]
    public class MongoToken
    {
        /// <summary>
        /// Текущая версия документа
        /// </summary>
        public const int VERSION = 1;

        public ObjectId Id { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// Идентификатор Id в системе. Уникален в БД.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Унакальный ключ доступа в систему
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Срок годности токена
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        public MongoToken()
        {
            Version = VERSION;
        }
    }

}
