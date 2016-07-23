using System;
using MongoDB.Bson.Serialization.Attributes;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.Auth.Mongo
{
    [MongoCollection("Users")]
    [MongoDataVersion(VERSION)]
    public class MongoUser
    {
        /// <summary>
        /// Текущая версия документа
        /// </summary>
        public const int VERSION = 1;

        public string Id { get; set; }

        public int Version { get; private set; }

        [MongoIndexName("login")]
        /// <summary>
        /// Логин пользователя. Уникален в БД.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя. Закодирован.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Возвращает true, если пользователь залогинен
        /// </summary>
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime CreationTime { get; set; }

        public Domain.UserStatus Status { get; set; }

        public MongoUser()
        {
            Version = VERSION;
        }
    }
}
