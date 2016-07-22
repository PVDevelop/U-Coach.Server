using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNUnit;

namespace Auth.Domain.Tests
{
    [TestFixture]
    [Category(CategoryConst.UNIT)]
    public class UserValidatorTests
    {
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("login")]
        [TestCase("login@a")]
        public void UserValidator_ValidateLogin_ExpectedException(string login)
        {
            UserValidator validator = new UserValidator();

            Assert.That(() => validator.ValidateLogin(login), Throws.TypeOf<ValidateLoginException>());
        }

        [TestCase("login@a.com")]
        public void UserValidator_ValidateLogin_Work(string login)
        {
            UserValidator validator = new UserValidator();
            validator.ValidateLogin(login);
        }


        [TestCase("")]
        [TestCase("   ")]
        [TestCase("1231111")]
        [TestCase("aaaaaaa")]
        [TestCase("aAaabbs")]
        [TestCase("aaaaAA")]
        public void UserValidator_ValidatePassword_ExpectedException(string password)
        {
            UserValidator validator = new UserValidator();

            Assert.That(() => validator.ValidatePassword(password), Throws.TypeOf<ValidatePasswordException>());
        }

        [TestCase("aAaabb1")]
        public void UserValidator_ValidatePassword_Work(string password)
        {
            UserValidator validator = new UserValidator();
            validator.ValidatePassword(password);
        }
    }
}
