﻿using DevOne.Security.Cryptography.BCrypt;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Auth.Service;
using TestNUnit;
using Rhino.Mocks;
using PVDevelop.UCoach.Server.Timing;

namespace AuthService.Tests
{
    [TestFixture]
    [Category(CategoryConst.UNIT)]
    public class UserTests
    {
        private UserService GenerateUserService()
        {
            var userValidator = MockRepository.GenerateMock<IUserValidator>();
            var userRepository = MockRepository.GenerateMock<IUserRepository>();
            var tokenRepository = MockRepository.GenerateMock<ITokenRepository>();
            var confirmationRepository = MockRepository.GenerateMock<IConfirmationRepository>();
            var confirmationProducer = MockRepository.GenerateMock<IConfirmationProducer>();
            var keyGeneratorService = MockRepository.GenerateMock<IKeyGeneratorService>();
            var utcTimeProvider = MockRepository.GenerateMock<IUtcTimeProvider>();

            return new UserService(
                userValidator,
                userRepository,
                tokenRepository,
                confirmationRepository,
                confirmationProducer,
                keyGeneratorService,
                utcTimeProvider);
        }

        [Test]
        public void Ctor_ValidLogin_SetsCreationTime()
        {
            var userService = GenerateUserService();
            userService.CreateUser("u", "pwd");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void SetPassword_InvalidPasswordFormat_ThrowsException(string password)
        {
            var userService = GenerateUserService();
            Assert.Throws(typeof(InvalidPasswordFormatException), () => userService.CreateUser("a", password));
        }

        [Test]
        public void Logon_InvalidPassword_ThrowsException()
        {
            var userService = GenerateUserService();
            userService.CreateUser("abc", "pwd");
            Assert.Throws(typeof(InvalidPasswordException), () => userService.Logon("abc", "invalid_password"));
        }

        [Test]
        public void ValidateToken_ValidPassword_DoesNothing()
        {
            var userService = GenerateUserService();
            userService.CreateUser("abc", "pwd");
            userService.Logon("abc", "pwd");
        }
    }
}