using System;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Domain.AuthTokenValidation;
using PVDevelop.UCoach.Server.Timing;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace Role.Domain.Tests
{
    [TestFixture]
    public class TokenValidationServiceTests
    {
        [Test]
        public void Validate_MockAuthTokenValidator_CallsValidate()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<TokenValidationService>();

            var tokenId = new TokenId("token");
            var userId = new UserId("system", "id");
            var authSystemToken = new AuthSystemToken("authToken", DateTime.UtcNow.AddDays(1));

            var validator = autoMocker.Get<IAuthTokenValidator>();
            validator.Expect(v => v.Validate(authSystemToken));

            var validatorContainer = autoMocker.Get<IAuthTokenValidatorContainer>();
            validatorContainer.Stub(c => c.GetValidator(userId.AuthSystemName)).Return(validator);

            var token = new Token(tokenId, userId, authSystemToken, DateTime.UtcNow.AddDays(1));

            // act
            autoMocker.ClassUnderTest.Validate(token);

            // assert
            validator.VerifyAllExpectations();
        }

        [Test]
        public void Validate_TokenIsExpired_ThrowsException()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<TokenValidationService>();

            var utcTimeProvider = autoMocker.Get<IUtcTimeProvider>();
            var dateTime = DateTime.UtcNow;
            utcTimeProvider.Stub(t => t.UtcNow).Return(dateTime);

            var tokenId = new TokenId("token");
            var userId = new UserId("system", "id");
            var authSystemToken = new AuthSystemToken("authToken", dateTime.AddDays(1));

            var token = new Token(tokenId, userId, authSystemToken, dateTime.AddDays(-1));

            // act
            Assert.Throws<NotAuthorizedException>(() => autoMocker.ClassUnderTest.Validate(token));
        }
    }
}
