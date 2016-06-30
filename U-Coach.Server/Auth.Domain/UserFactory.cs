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
        public static User CreateUser(string login)
        {
            if(string.IsNullOrWhiteSpace(login))
            {
                throw new LoginNotSetException();
            }

            return new User()
            {
                Login = login,
                CreationTime = UtcTime.UtcNow
            };
        }
    }
}
