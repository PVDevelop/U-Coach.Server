using System;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Timing;

namespace Auth.Domain.Tests
{
    public class TestUserFactory : IUserFactory
    {
        public IUtcTimeProvider UtcTimeProvider { get; private set; }
        public IUserValidator UserValidator { get; private set; }

        public TestUserFactory()
        {
            UtcTimeProvider = new TestUtcTimeProvider();
            UserValidator = new TestUserValidator();
        }

        public User CreateUser(string login, string password)
        {
            var userFactory = new UserFactory(UtcTimeProvider, UserValidator);
            return userFactory.CreateUser(login, password);
        }
    }
}
