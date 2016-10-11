using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IKeyGeneratorService
    {
        /// <summary>
        /// Генерация ключа для подтверждения
        /// </summary>
        string GenerateConfirmationKey();

        /// <summary>
        /// Генерация ключа для токена
        /// </summary>
        string GenerateTokenKey();

        /// <summary>
        /// Генерация Id для пользователя
        /// </summary>
        string GenerateUserId();
    }
}
