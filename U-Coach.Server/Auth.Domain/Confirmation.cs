using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Доменная модель - подтверждение
    /// </summary>
    public sealed class Confirmation
    {
        /// <summary>
        /// Идентификатор Id пользователя в системе. Уникален в БД.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Ключ подтверждения
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Время генерации ключа
        /// </summary>
        public DateTime CreationTime { get; private set; }

        public Confirmation(string userId, string key, DateTime creationTime)
        {
#warning проверка аргументов
            this.UserId = UserId;
            this.Key = key;
            this.CreationTime = creationTime;
        }
    }
}
