using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public static class UserFactory
    {
        public static User CreateUser(string login, string password)
        {
            if(string.IsNullOrWhiteSpace(login))
            {
                throw new LoginNotSetException();
            }

            var user = new User()
            {
                Login = login,
                CreationTime = UtcTime.UtcNow
            };

            user.SetPassword(password);

            return user;
        }
    }
}
