using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.WebApi.Client
{
    /// <summary>
    /// Интерфейс доступа к пользователям
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="userParams">Параметры создания.</param>
        void Create(CreateUserParams userParams);
    }
}
