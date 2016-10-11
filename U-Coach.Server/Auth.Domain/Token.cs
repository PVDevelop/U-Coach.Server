using PVDevelop.UCoach.Server.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Доменная модель - токен
    /// </summary>
    public sealed class Token
    {
        /// <summary>
        /// Идентификатор Id в системе. Уникален в БД.
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Унакальный ключ доступа в систему
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Срок годности токена
        /// </summary>
        public DateTime ExpiryDate { get; private set; }

        public Token(
            string userId, 
            string key,
            IUtcTimeProvider utcTimeProvider)
        {
            userId.NullOrEmptyValidate(nameof(userId));
            key.NullOrEmptyValidate(nameof(userId));
            utcTimeProvider.NullValidate(nameof(utcTimeProvider));

            this.UserId = userId;
            this.Key = key;
            this.ExpiryDate = utcTimeProvider.UtcNow.Date.AddDays(60).AddHours(2);
        }
    }
}
