using System;
using Timing;

namespace PVDevelop.UCoach.Server.AuthService
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

        public User CreateNewUser(string login)
        {
            return new User(_utcTimeProvider, login);
        }
    }
}
