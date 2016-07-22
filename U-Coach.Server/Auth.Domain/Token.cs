using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string UserId { get; set; }

        /// <summary>
        /// Унакальный ключ доступа в систему
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Срок годности токена
        /// </summary>
        public DateTime ExpiryDate { get; set; }
    }
}
