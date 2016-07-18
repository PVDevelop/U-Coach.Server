using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Auth.Domain;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public interface ITokenRepository
    {
        /// <summary>
        /// Добавление токена 
        /// </summary>
        /// <param name="token">Токен доступа</param>
        void AddToken(Token token);

        /// <summary>
        /// Закрытие токена доступа
        /// </summary>
        /// <param name="token">Токен доступа</param>
        void CloseToken(string token);

        /// <summary>
        /// Получить нужный токен
        /// </summary>
        /// <param name="localToken"></param>
        /// <returns></returns>
        Token GetToken(string token);
    }
}
