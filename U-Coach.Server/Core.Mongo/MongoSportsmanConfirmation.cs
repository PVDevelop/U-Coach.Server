using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Mongo;
using System;

namespace PVDevelop.UCoach.Server.Core.Mongo
{
    [MongoCollection("SportsmanConfirmations")]
    [MongoDataVersion(VERSION)]
    public class MongoSportsmanConfirmation
    {
        /// <summary>
        /// Текущая версия документа.
        /// </summary>
        private const int VERSION = 1;

        /// <summary>
        /// Идентификатор подтверждения.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Версия документа.
        /// </summary>
        public int Version { get; set; }

        [MongoIndexName("auth_system")]
        /// <summary>
        /// Система аутентификации.
        /// </summary>
        public SportsmanConfirmationAuthSystem AuthSystem { get; set; }

        [MongoIndexName("auth_id")]
        /// <summary>
        /// Идентификатор пользователя в системе аутентификации.
        /// </summary>
        public string AuthUserId { get; set; }

        /// <summary>
        /// Ключ подтверждения.
        /// </summary>
        public string ConfirmationKey { get; set; }

        /// <summary>
        /// Состояние подтверждения.
        /// </summary>
        public SportsmanConfirmationState State { get; set; }

        public MongoSportsmanConfirmation()
        {
            Version = VERSION;
        }
    }
}
