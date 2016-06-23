using System;
using MongoDB.Bson;
using PVDevelop.UCoach.Server.Mongo;

namespace PVDevelop.UCoach.Server.AuthService.Persist
{
    [MongoCollection("Users")]
    [MongoDataVersion(1)]
    public class UserOrm :
        IAmDocument
    {
        public const int VERSION = 1;

        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Версия документа.
        /// </summary>
        public int Version { get; set; }

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
        /// Время последней аутентификации пользователя
        /// </summary>
        public DateTime LastAuthenticationTime { get; set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime CreationTime { get; set; }

        public UserOrm()
        {
            Version = VERSION;
        }
    }
}
