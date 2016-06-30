using System;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Timing;
using TestTimingUtilities;

namespace Auth.Domain.Tests
{
    public class TestUserFactory : IUserFactory
    {
        public IUtcTimeProvider UtcTimeProvider { get; private set; }

        public TestUserFactory()
        {
            UtcTimeProvider = new FixedUtcTimeProvider();
        }

        public User CreateUser(string login, string password)
        {
            var userFactory = new UserFactory(UtcTimeProvider);
            return userFactory.CreateUser(login, password);
        }
    }
}
