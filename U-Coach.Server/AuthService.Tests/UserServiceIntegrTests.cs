using DevOne.Security.Cryptography.BCrypt;
using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Tests;
using Rhino.Mocks;
using TestUtils;

namespace PVDevelop.UCoach.Server.AuthService.Tests
{
    [TestFixture]
    [Integration]
    public class UserServiceIntegrTests
    {
        [Test]
        public void Create_NewUser_CreatesEncodedAndSavesInDb()
        {
            var userParams = new CreateUserParams()
            {
                Login = "login",
                Password = "password"
            };

            var settings = TestMongoHelper.CreateSettings();

            var timeProvider = new FixedUtcTimeProvider();
            var factory = new UserFactory(timeProvider);

            var userService = new UserService(factory, new MongoRepository<User>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()));
            userService.Create(userParams);

            TestMongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(MongoHelper.GetCollectionName<User>()).Find(u => u.Login == userParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.NotNull(user.Password);

                Assert.That(user.CreationTime, Is.EqualTo(timeProvider.UtcTime).Within(1).Milliseconds);
                BCryptHelper.CheckPassword(userParams.Password, user.Password);
            });
        }

        [Test]
        public void Logon_ValidPassword_SetsLoggedInAndCurrentTime()
        {
            var settings = TestMongoHelper.CreateSettings();

            var timeProvider = new FixedUtcTimeProvider();
            var userService = new UserService(new UserFactory(timeProvider), new MongoRepository<User>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()));

            var createUserParams = new CreateUserParams()
            {
                Login = "some_login",
                Password = "some_password"
            };

            userService.Create(createUserParams);

            var authUserParams = new LogonUserParams()
            {
                Login = "some_login",
                Password = "some_password"
            };

            timeProvider.SetUtcNow();
            userService.Logon(authUserParams);

            TestMongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(MongoHelper.GetCollectionName<User>()).Find(u => u.Login == createUserParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.NotNull(user.Password);
                Assert.IsTrue(user.IsLoggedIn);
                Assert.That(user.LastAuthenticationTime, Is.EqualTo(timeProvider.UtcTime).Within(1).Seconds);
            });
        }

        [Test]
        public void LogoutByPassword_LoggedInUser_SetsNotLoggedIn()
        {
            var settings = TestMongoHelper.CreateSettings();

            var userService = new UserService(
                new UserFactory(new FixedUtcTimeProvider()), 
                new MongoRepository<User>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()));

            var createUserParams = new CreateUserParams()
            {
                Login = "some_login",
                Password = "some_password"
            };

            userService.Create(createUserParams);

            var authUserParams = new LogonUserParams()
            {
                Login = "some_login",
                Password = "some_password"
            };

            userService.Logon(authUserParams);

            var logoutUserParams = new LogoutByPasswordUserParams()
            {
                Login = "some_login",
                Password = "some_password"
            };

            userService.LogoutByPassword(logoutUserParams);

            TestMongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(MongoHelper.GetCollectionName<User>()).Find(u => u.Login == createUserParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.IsFalse(user.IsLoggedIn);
            });
        }
    }
}
