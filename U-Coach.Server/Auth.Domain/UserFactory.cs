using System;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public class UserFactory : IUserFactory
    {
        private readonly IUtcTimeProvider _utcTimeProvider;

        public UserFactory(IUtcTimeProvider utcTimeProvider)
        {
            if(utcTimeProvider == null)
            {
                throw new ArgumentNullException("utcTimeProvider");
            }
            _utcTimeProvider = utcTimeProvider;
        }

        public User CreateUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new LoginNotSetException();
            }

            var user = new User()
            {
                Login = login,
                CreationTime = _utcTimeProvider.UtcNow
            };

            user.SetPassword(password);

            return user;
        }
    }
}
