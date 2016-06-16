using DevOne.Security.Cryptography.BCrypt;
using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Tests;
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

            var settings = MongoHelper.CreateSettings(UserService.USER_COLLECITION_NAME);

            UtcTime.SetUtcNow();
            var userService = new UserService(new MongoRepository<User>(settings));
            userService.Create(userParams);

            MongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(settings.CollectionName).Find(u => u.Login == userParams.Login).ToList();
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
            var settings = MongoHelper.CreateSettings(UserService.USER_COLLECITION_NAME);

            var userService = new UserService(new MongoRepository<User>(settings));

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

            MongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(settings.CollectionName).Find(u => u.Login == createUserParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.NotNull(user.Password);
                Assert.IsTrue(user.IsLoggedIn);
                Assert.That(user.LastAuthenticationTime, Is.EqualTo(UtcTime.UtcNow).Within(1).Seconds);
            });
        }

        [Test]
        public void Logout_LoggedInUser_SetsNotLoggedIn()
        {
            var settings = MongoHelper.CreateSettings(UserService.USER_COLLECITION_NAME);

            var userService = new UserService(new MongoRepository<User>(settings));

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

            var logoutUserParams = new LogoutUserParams()
            {
                Login = "some_login"
            };

            userService.Logout(logoutUserParams);

            MongoHelper.WithDb(settings, db =>
            {
                var users = db.GetCollection<User>(settings.CollectionName).Find(u => u.Login == createUserParams.Login).ToList();
                Assert.AreEqual(1, users.Count);
                var user = users[0];
                Assert.IsFalse(user.IsLoggedIn);
            });
        }
    }
}
