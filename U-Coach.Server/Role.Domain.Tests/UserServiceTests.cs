using NUnit.Framework;
using PVDevelop.UCoach.Server.Role.Domain;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace Role.Domain.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void RegisterUserToken_UserDoesNotExist_InsertsNewUser()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("bbb");

            var userId = new UserId("system", "user");
            var userRepository = autoMocker.Get<IUserRepository>();
            userRepository.Expect(r => r.Insert(Arg<User>.Matches(u => u.Id.Equals(userId))));

            // act
            var tokenParams = new AuthTokenParams(userId.AuthSystemName, userId.AuthId, "token");
            autoMocker.ClassUnderTest.RegisterUserToken(tokenParams);

            // assert
            userRepository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterUserToken_UserExists_DoesNotInsertUser()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("bbb");

            var userId = new UserId("system", "user");
            var userRepository = autoMocker.Get<IUserRepository>();

            var user = new User(userId);
            userRepository.Stub(r => r.TryGet(Arg<UserId>.Is.Equal(userId), out Arg<User>.Out(user).Dummy)).Return(true);
            userRepository.Expect(r => r.Insert(Arg<User>.Matches(u => u.Id.Equals(userId)))).Repeat.Never();

            // act
            var tokenParams = new AuthTokenParams(userId.AuthSystemName, userId.AuthId, "token");
            autoMocker.ClassUnderTest.RegisterUserToken(tokenParams);

            // assert
            userRepository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterUserToken_GenerateNewToken_InsertsNewToken()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("aaa");

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            var expectedTokenId = new TokenId("aaa");
            tokenRepository.Expect(r => r.Insert(Arg<Token>.Matches(t => t.Id.Equals(expectedTokenId))));

            // act
            var tokenId = autoMocker.ClassUnderTest.RegisterUserToken(new AuthTokenParams("system", "user", "token"));

            // assert
            Assert.AreEqual(expectedTokenId, tokenId);
            tokenRepository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterUserToken_TokenExists_DoesNotInsertToken()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("aaa");

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            var tokenId = new TokenId("aaa");
            var userId = new UserId("system", "user");
            var tokenParams = new AuthTokenParams(userId.AuthSystemName, userId.AuthId, "token");
            var token = new Token(tokenId, userId, tokenParams);
            tokenRepository.Stub(r => r.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);
            tokenRepository.Expect(r => r.Insert(null)).IgnoreArguments().Repeat.Never();

            // act
            autoMocker.ClassUnderTest.RegisterUserToken(tokenParams);

            // assert
            tokenRepository.VerifyAllExpectations();
        }
    }
}
