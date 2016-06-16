using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;
using PVDevelop.UCoach.Server;
using PVDevelop.UCoach.Server.AuthService;
using PVDevelop.UCoach.Server.Exceptions.Auth;
using PVDevelop.UCoach.Server.Mongo;
using Rhino.Mocks;
using System;
using Utilities;

namespace AuthService.Tests
{
    [TestFixture]
    public class UserTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Ctor_EmptyLogin_ThrowsException(string login)
        {
            Assert.Throws(typeof(LoginNotSetException), () => new User(login));
        }

        [Test]
        public void Ctor_ValidLogin_SetsCreationTime()
        {
            UtcTime.SetUtcNow();
            var user = new User("u");
            Assert.AreEqual(UtcTime.UtcNow, user.CreationTime);
        }

        [Test]
        public void Ctor_ValidLogin_GeneratesId()
        {
            var user = new User("ggg");
            Assert.AreNotEqual(Guid.Empty, user.Id);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void SetPassword_InvalidPasswordFormat_ThrowsException(string password)
        {
            var user = new User("a");
            Assert.Throws(typeof(InvalidPasswordFormatException), () => user.SetPassword(password));
        }

        [Test]
        public void Logon_InvalidPassword_ThrowsException()
        {
            var user = new User("abc");
            user.SetPassword("pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => user.Logon("invalid_password"));
        }

        [Test]
        public void Logon_ValidPassword_SetsLoggedIn()
        {
            var user = new User("abc");
            user.SetPassword("pwd");
            user.Logon("pwd");
            Assert.IsTrue(user.IsLoggedIn);
        }

        [Test]
        public void Logon_ValidPassword_ReturnsExpectedToken()
        {
            var user = new User("u");

            user.SetPassword("pwd123");
            var token = user.Logon("pwd123");

            Assert.IsTrue(BCryptHelper.CheckPassword(user.Password, token));
        }

        [Test]
        public void Logon_ValidPassword_SetsCurrentAuthenticationTime()
        {
            UtcTime.SetUtcNow();
            var user = new User("abc");
            user.SetPassword("pwd3");
            user.Logon("pwd3");
            Assert.AreEqual(UtcTime.UtcNow, user.LastAuthenticationTime);
        }

        [Test]
        public void Logout_UserIsLoggedIn_SetsNotLoggedIn()
        {
            var user = new User("aaa");
            user.SetPassword("pwd");
            user.Logon("pwd");
            user.Logout("pwd");
            Assert.IsFalse(user.IsLoggedIn);
        }

        [Test]
        public void Logout_UserIsLoggedInButInvalidPassword_ThrowsException()
        {
            var user = new User("aaa");
            user.SetPassword("pwd");
            user.Logon("pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => user.Logout("invalid"));
        }

        [Test]
        public void ValidateToken_ValidToken_DoesNothing()
        {
            var u = new User("u");
            u.SetPassword("p1");
            var token = u.Logon("p1");
            u.ValidateToken(token);
        }

        [Test]
        public void ValidateToken_InvalidToken_ThrowsException()
        {
            var u = new User("u");
            u.SetPassword("p1");
            u.Logon("p1");
            Assert.Throws(typeof(InvalidTokenException), () => u.ValidateToken("abc"));
        }

        [Test]
        public void ValidateToken_UserIsNotLoggedIn_ThrowsException()
        {
            var u = new User("aaa");
            u.SetPassword("p2");
            var token = u.Logon("p2");
            u.Logout("p2");
            Assert.Throws(typeof(NotLoggedInException), () => u.ValidateToken(token));
        }
    }
}