using PVDevelop.UCoach.Server.Auth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public interface IConfirmationService
    {
        /// <summary>
        /// Метод генерации кода подтверждения для пользователя
        /// </summary>
        /// <param name="userParams">Id - пользователя</param>
        void CreateConfirmation(string userId);

        /// <summary>
        /// Подтверждение для пользователя
        /// </summary>
        /// <param name="userId">Id пользователя в системе</param>
        /// <param name="key">ключ подтверждения</param>
        bool Confirm(string userId, string key);
    }
}
