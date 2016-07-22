using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Service
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
    }
}
