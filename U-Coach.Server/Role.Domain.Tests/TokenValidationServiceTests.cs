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
        public void Validate_TokenNotFound_ThrowsException()
        {
            var autoMocker = new RhinoAutoMocker<TokenValidationService>();
            var tokenId = new TokenId("token");

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            tokenRepository.Stub(r => r.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(null).Dummy)).Return(false);

            Assert.Throws<NotAuthorizedException>(() => autoMocker.ClassUnderTest.Validate(tokenId));
        }

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

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            var token = new Token(tokenId, userId, authSystemToken, DateTime.UtcNow.AddDays(1));
            tokenRepository.Stub(r => r.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);

            // act
            autoMocker.ClassUnderTest.Validate(tokenId);

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

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            var token = new Token(tokenId, userId, authSystemToken, dateTime.AddDays(-1));
            tokenRepository.Stub(r => r.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);

            // act
            Assert.Throws<NotAuthorizedException>(() => autoMocker.ClassUnderTest.Validate(tokenId));
        }
    }
}
