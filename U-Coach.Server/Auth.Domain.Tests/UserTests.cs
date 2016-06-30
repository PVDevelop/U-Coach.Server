﻿using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;

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
            Assert.Throws(typeof(LoginNotSetException), () => UserFactory.CreateUser(login, "pwd"));
        }

        [Test]
        public void Ctor_ValidLogin_SetsCreationTime()
        {
            UtcTime.SetUtcNow();
            var user = UserFactory.CreateUser("u", "pwd");
            Assert.AreEqual(UtcTime.UtcNow, user.CreationTime);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void SetPassword_InvalidPasswordFormat_ThrowsException(string password)
        {
            Assert.Throws(typeof(InvalidPasswordFormatException), () => UserFactory.CreateUser("a", password));
        }

        [Test]
        public void Logon_InvalidPassword_ThrowsException()
        {
            var user = UserFactory.CreateUser("abc", "pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => user.Logon("invalid_password"));
        }

        [Test]
        public void Logon_ValidPassword_SetsLoggedIn()
        {
            var user = UserFactory.CreateUser("abc", "pwd");
            user.Logon("pwd");
            Assert.IsTrue(user.IsLoggedIn);
        }

        [Test]
        public void Logon_ValidPassword_ReturnsExpectedToken()
        {
            var user = UserFactory.CreateUser("u", "pwd123");

            var token = user.Logon("pwd123");

            Assert.IsTrue(BCryptHelper.CheckPassword(user.Password, token));
        }

        [Test]
        public void Logon_ValidPassword_SetsCurrentAuthenticationTime()
        {
            UtcTime.SetUtcNow();
            var user = UserFactory.CreateUser("abc", "pwd3");
            user.Logon("pwd3");
            Assert.AreEqual(UtcTime.UtcNow, user.LastAuthenticationTime);
        }

        [Test]
        public void Logout_UserIsLoggedIn_SetsNotLoggedIn()
        {
            var user = UserFactory.CreateUser("aaa", "pwd");
            user.Logon("pwd");
            user.Logout("pwd");
            Assert.IsFalse(user.IsLoggedIn);
        }

        [Test]
        public void Logout_UserIsLoggedInButInvalidPassword_ThrowsException()
        {
            var user = UserFactory.CreateUser("aaa", "pwd");
            user.Logon("pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => user.Logout("invalid"));
        }

        [Test]
        public void ValidateToken_ValidToken_DoesNothing()
        {
            var u = UserFactory.CreateUser("u", "p1");
            var token = u.Logon("p1");
            u.ValidateToken(token);
        }

        [Test]
        public void ValidateToken_InvalidToken_ThrowsException()
        {
            var u = UserFactory.CreateUser("u", "p1");
            u.Logon("p1");
            Assert.Throws(typeof(InvalidTokenException), () => u.ValidateToken("abc"));
        }

        [Test]
        public void ValidateToken_UserIsNotLoggedIn_ThrowsException()
        {
            var u = UserFactory.CreateUser("aaa", "p2");
            var token = u.Logon("p2");
            u.Logout("p2");
            Assert.Throws(typeof(NotLoggedInException), () => u.ValidateToken(token));
        }
    }
}