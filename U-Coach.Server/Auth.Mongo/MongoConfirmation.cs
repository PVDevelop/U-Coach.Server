using PVDevelop.UCoach.Server.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    [MongoCollection("Confirmations")]
    [MongoDataVersion(VERSION)]
    public class MongoConfirmation
    {
        /// <summary>
        /// Текущая версия документа
        /// </summary>
        public const int VERSION = 1;

        public int Version { get; set; }

        /// <summary>
        /// Идентификатор Id пользователя в системе. Уникален в БД.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Ключ подтверждения создания спортсмена
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Время генерации ключа
        /// </summary>
        public DateTime CreationTime { get; set; }

        public MongoConfirmation()
        {
            Version = VERSION;
        }
    }
}
