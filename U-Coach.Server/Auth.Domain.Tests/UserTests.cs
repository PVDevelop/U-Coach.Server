using Auth.Domain.Tests;
using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;

namespace AuthService.Tests
{
    [TestFixture]
    public class UserTests
    {
        //[TestCase(null)]
        //[TestCase("")]
        //[TestCase("  ")]
        //public void Ctor_EmptyLogin_ThrowsException(string login)
        //{
        //    Assert.Throws(typeof(LoginNotSetException), () => new TestUserFactory().CreateUser(login, "pwd"));
        //}

        //[Test]
        //public void Ctor_ValidLogin_SetsCreationTime()
        //{
        //    var userFactory = new TestUserFactory();
        //    var user = userFactory.CreateUser("u", "pwd");
        //    Assert.AreEqual(userFactory.UtcTimeProvider.UtcNow, user.CreationTime);
        //}

        //[TestCase(null)]
        //[TestCase("")]
        //[TestCase("  ")]
        //public void SetPassword_InvalidPasswordFormat_ThrowsException(string password)
        //{
        //    Assert.Throws(typeof(InvalidPasswordFormatException), () => new TestUserFactory().CreateUser("a", password));
        //}

        //[Test]
        //public void Logon_InvalidPassword_ThrowsException()
        //{
        //    var user = new TestUserFactory().CreateUser("abc", "pwd");
        //    Assert.Throws(typeof(InvalidPasswordException), () => user.Logon("invalid_password"));
        //}

        //[Test]
        //public void Logon_ValidPassword_ReturnsExpectedToken()
        //{
        //    var user = new TestUserFactory().CreateUser("u", "pwd123");

        //    var token = user.Logon("pwd123");

        //    Assert.IsTrue(BCryptHelper.CheckPassword(user.Password, token));
        //}

        //[Test]
        //public void ValidateToken_ValidToken_DoesNothing()
        //{
        //    var u = new TestUserFactory().CreateUser("u", "p1");
        //    var token = u.Logon("p1");
        //    u.ValidateToken(token);
        //}

        //[Test]
        //public void ValidateToken_InvalidToken_ThrowsException()
        //{
        //    var u = new TestUserFactory().CreateUser("u", "p1");
        //    u.Logon("p1");
        //    Assert.Throws(typeof(InvalidTokenException), () => u.ValidateToken("abc"));
        //}
    }
}