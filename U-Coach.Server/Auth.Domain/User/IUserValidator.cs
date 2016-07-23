#warning будем грохать лишнее?
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface IUserValidator
    {
        /// <summary>
        /// Валидация логина
        /// </summary>
        void ValidateLogin(string login);

        /// <summary>
        /// Валидация пароля
        /// </summary>
        void ValidatePassword(string password);
    }
}
