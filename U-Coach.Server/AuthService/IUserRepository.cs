using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.AuthService
{
    public interface IUserRepository
    {
        void Insert(User user);

        User FindByLogin(string login);

        void Update(User user);
    }
}
