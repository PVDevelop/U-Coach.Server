using DevOne.Security.Cryptography.BCrypt;
using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Mongo;
using Rhino.Mocks;
using TestMongoUtilities;
using TestNUnit;
using Utilities;

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

            UtcTime.SetUtcNow();
            var userService = new UserService(new MongoUserRepository(
                new MongoRepository<MongoUser>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()),
                null));
            userService.Create(userParams);

            TestMongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<MongoUser>(MongoHelper.GetCollectionName<MongoUser>()).Find(u => u.Login == userParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.NotNull(user.Password);

                Assert.That(user.CreationTime, Is.EqualTo(UtcTime.UtcNow).Within(1).Milliseconds);
                BCryptHelper.CheckPassword(userParams.Password, user.Password);
            });
        }

        [Test]
        public void Logon_ValidPassword_SetsLoggedInAndCurrentTime()
        {
            var settings = TestMongoHelper.CreateSettings();

            var userService = new UserService(new MongoUserRepository(
                new MongoRepository<MongoUser>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()),
                null));

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

            UtcTime.SetUtcNow();
            userService.Logon(authUserParams);

            TestMongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(MongoHelper.GetCollectionName<User>()).Find(u => u.Login == createUserParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.NotNull(user.Password);
                Assert.IsTrue(user.IsLoggedIn);
                Assert.That(user.LastAuthenticationTime, Is.EqualTo(UtcTime.UtcNow).Within(1).Seconds);
            });
        }

        [Test]
        public void LogoutByPassword_LoggedInUser_SetsNotLoggedIn()
        {
            var settings = TestMongoHelper.CreateSettings();

            var userService = new UserService(new MongoUserRepository(
                new MongoRepository<MongoUser>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>()),
                null));

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
