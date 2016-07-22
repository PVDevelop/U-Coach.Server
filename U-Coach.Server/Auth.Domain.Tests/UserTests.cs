using Auth.Domain.Tests;
using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using TestNUnit;

namespace AuthService.Tests
{
    [TestFixture]
    [Category(CategoryConst.UNIT)]
    public class UserTests
    {
        [Test]
        public void Ctor_ValidLogin_SetsCreationTime()
        {
            var userFactory = new TestUserFactory();
            var user = userFactory.CreateUser("u", "pwd");
            Assert.AreEqual(userFactory.UtcTimeProvider.UtcNow, user.CreationTime);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void SetPassword_InvalidPasswordFormat_ThrowsException(string password)
        {
            Assert.Throws(typeof(InvalidPasswordFormatException), () => new TestUserFactory().CreateUser("a", password));
        }

        [Test]
        public void Logon_InvalidPassword_ThrowsException()
        {
            var user = new TestUserFactory().CreateUser("abc", "pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => user.CheckPassword("invalid_password"));
        }

        [Test]
        public void ValidateToken_ValidPassword_DoesNothing()
        {
            var u = new TestUserFactory().CreateUser("u", "p1");
            u.CheckPassword("p1");
        }
    }
}