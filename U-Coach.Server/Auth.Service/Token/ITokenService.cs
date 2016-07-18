using PVDevelop.UCoach.Server.Auth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public interface ITokenService
    {
        /// <summary>
        /// Метод получения токена доступа в системе
        /// </summary>
        /// <param name="userId">Уникальный Id пользователя в системе</param>
        /// <returns>Token доступа в системе</returns>
        Token ReqistrToken(string userId);

        /// <summary>
        /// Принудительное завершение доступ токену
        /// </summary>
        void CloseToken(string tokenKey);

        /// <summary>
        /// Проверка валидности токена
        /// </summary>
        /// <param name="token">Токен доступа</param>
        /// <returns></returns>
        bool ValidateToken(string token);
    }
}
