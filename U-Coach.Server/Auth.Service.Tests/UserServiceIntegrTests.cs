using NUnit.Framework;
using TestNUnit;

namespace PVDevelop.UCoach.Server.AuthService.Tests
{
    [TestFixture]
    [Integration]
    public class UserServiceIntegrTests
    {
#warning перенести в репозиторий
        //[Test]
        //public void Create_NewUser_CreatesEncodedAndSavesInDb()
        //{
        //    var userParams = new CreateUserParams()
        //    {
        //        Login = "login",
        //        Password = "password"
        //    };

        //    var settings = TestMongoHelper.CreateSettings();
        //    TestMongoHelper.WithDb(settings, db =>
        //    {
        //        UtcTime.SetUtcNow();
        //        var userService = new UserService(new MongoUserRepository(
        //            new MongoRepository<MongoUser>(settings),
        //            new MongoCollectionVersionValidatorByClassAttribute(settings),
        //            CreateUserMapper()));
        //        userService.Create(userParams);

        //        var users = db.GetCollection<MongoUser>(MongoHelper.GetCollectionName<MongoUser>()).Find(u => u.Login == userParams.Login).ToList();
        //        Assert.AreEqual(1, users.Count);
        //        var user = users[0];
        //        Assert.NotNull(user.Password);

        //        Assert.That(user.CreationTime, Is.EqualTo(UtcTime.UtcNow).Within(1).Milliseconds);
        //        BCryptHelper.CheckPassword(userParams.Password, user.Password);
        //    });
        //}

        //[Test]
        //public void Logon_ValidPassword_SetsLoggedInAndCurrentTime()
        //{
        //    var settings = TestMongoHelper.CreateSettings();
        //    TestMongoHelper.WithDb(settings, db =>
        //    {
        //        var userService = new UserService(new MongoUserRepository(
        //            new MongoRepository<MongoUser>(settings),
        //            new MongoCollectionVersionValidatorByClassAttribute(settings),
        //            CreateUserMapper()));

        //        var createUserParams = new CreateUserParams()
        //        {
        //            Login = "some_login",
        //            Password = "some_password"
        //        };

        //        userService.Create(createUserParams);

        //        var authUserParams = new LogonUserParams()
        //        {
        //            Login = "some_login",
        //            Password = "some_password"
        //        };

        //        UtcTime.SetUtcNow();
        //        userService.Logon(authUserParams);

        //        var users = db.GetCollection<MongoUser>(MongoHelper.GetCollectionName<MongoUser>()).Find(u => u.Login == createUserParams.Login).ToList();
        //        Assert.AreEqual(1, users.Count);
        //        var user = users[0];
        //        Assert.NotNull(user.Password);
        //        Assert.IsTrue(user.IsLoggedIn);
        //        Assert.That(user.LastAuthenticationTime, Is.EqualTo(UtcTime.UtcNow).Within(1).Seconds);
        //    });
        //}

        //[Test]
        //public void LogoutByPassword_LoggedInUser_SetsNotLoggedIn()
        //{
        //    var settings = TestMongoHelper.CreateSettings();
        //    TestMongoHelper.WithDb(settings, db =>
        //    {
        //        var userService = new UserService(new MongoUserRepository(
        //            new MongoRepository<MongoUser>(settings),
        //            new MongoCollectionVersionValidatorByClassAttribute(settings),
        //            CreateUserMapper()));

        //        var createUserParams = new CreateUserParams()
        //        {
        //            Login = "some_login",
        //            Password = "some_password"
        //        };

        //        userService.Create(createUserParams);

        //        var authUserParams = new LogonUserParams()
        //        {
        //            Login = "some_login",
        //            Password = "some_password"
        //        };

        //        userService.Logon(authUserParams);

        //        var logoutUserParams = new LogoutByPasswordUserParams()
        //        {
        //            Login = "some_login",
        //            Password = "some_password"
        //        };

        //        userService.LogoutByPassword(logoutUserParams);

        //        var users = db.GetCollection<MongoUser>(MongoHelper.GetCollectionName<MongoUser>()).Find(u => u.Login == createUserParams.Login).ToList();
        //        Assert.AreEqual(1, users.Count);
        //        var user = users[0];
        //        Assert.IsFalse(user.IsLoggedIn);
        //    });
        //}
    }
}
