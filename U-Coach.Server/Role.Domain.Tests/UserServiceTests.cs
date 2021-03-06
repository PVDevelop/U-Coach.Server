﻿using System;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Timing;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using TestComparisonUtilities;

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

            autoMocker.Get<IUtcTimeProvider>().Stub(t => t.UtcNow).Return(DateTime.UtcNow);

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("bbb");

            var authUserId = new AuthUserId("system", "user");
            var userRepository = autoMocker.Get<IUserRepository>();
            var expectedUser = new User(new UserId(Guid.NewGuid()), authUserId);
            var comparer = new TestComparer().IgnoreProperty<User>(u => u.Id);
            userRepository.Expect(r => r.Insert(Arg<User>.Matches(u => comparer.Compare(u, expectedUser))));

            // act
            autoMocker.ClassUnderTest.RegisterUserToken(authUserId, new AuthSystemToken("token", DateTime.UtcNow));

            // assert
            userRepository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterUserToken_UserExists_DoesNotInsertUser()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            autoMocker.Get<IUtcTimeProvider>().Stub(t => t.UtcNow).Return(DateTime.UtcNow);

            var tokenGenerator = autoMocker.Get<ITokenGenerator>();
            tokenGenerator.Stub(t => t.Generate(null, null)).IgnoreArguments().Return("bbb");

            var authUserId = new AuthUserId("system", "user");
            var userRepository = autoMocker.Get<IUserRepository>();

            var user = new User(new UserId(Guid.NewGuid()), authUserId);
            userRepository.Stub(r => r.TryGetByAuthUserId(Arg<AuthUserId>.Is.Equal(authUserId), out Arg<User>.Out(user).Dummy)).Return(true);


            userRepository.Expect(r => r.Insert(Arg<User>.Matches(u => new TestComparer().Compare(user, u)))).Repeat.Never();

            // act
            autoMocker.ClassUnderTest.RegisterUserToken(authUserId, new AuthSystemToken("token", DateTime.UtcNow));

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

            var dateTime = DateTime.UtcNow;

            var timeProvider = autoMocker.Get<IUtcTimeProvider>();
            timeProvider.Stub(t => t.UtcNow).Return(dateTime);

            var tokenid = new TokenId("aaa");
            var userId = new UserId(Guid.NewGuid());
            var authUserId = new AuthUserId("system", "id");
            var authSystemToken = new AuthSystemToken("token", dateTime);
            var expiration = dateTime + UserService.TokenDuration;
            var expectedToken = new Token(tokenid, userId, authSystemToken, expiration);

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            var comparer = new TestComparer().IgnoreProperty<Token>(t => t.UserId);
            tokenRepository.Expect(r => r.Insert(Arg<Token>.Matches(t => comparer.Compare(expectedToken, t))));

            // act
            var token = autoMocker.ClassUnderTest.RegisterUserToken(
                authUserId, 
                authSystemToken);

            // assert
            string comparison;
            Assert.IsTrue(comparer.Compare(expectedToken, token, out comparison), comparison);
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
            var userId = new UserId(Guid.NewGuid());
            var authUserId = new AuthUserId("system", "user");
            var authSystemToken = new AuthSystemToken("token", DateTime.UtcNow);
            var token = new Token(tokenId, userId, authSystemToken, DateTime.UtcNow);
            tokenRepository.Stub(r => r.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);
            tokenRepository.Expect(r => r.Insert(null)).IgnoreArguments().Repeat.Never();

            // act
            autoMocker.ClassUnderTest.RegisterUserToken(authUserId, authSystemToken);

            // assert
            tokenRepository.VerifyAllExpectations();
        }

        [Test]
        public void GetUserByToken_TokenNotFound_ThrowsException()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            // act / assert
            var tokenId = new TokenId("t");
            Assert.Throws<NotAuthorizedException>(() => autoMocker.ClassUnderTest.GetUserByToken(tokenId));
        }

        [Test]
        public void GetUserByToken_UserNotFound_ThrowsException()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenId = new TokenId("t");
            var token = new Token(
                tokenId, 
                new UserId(Guid.NewGuid()), 
                new AuthSystemToken("t", DateTime.UtcNow.AddDays(1)), 
                DateTime.UtcNow.AddDays(2));

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            tokenRepository.Stub(t => t.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);

            // act / assert
            Assert.Throws<ApplicationException>(() => autoMocker.ClassUnderTest.GetUserByToken(tokenId));
        }

        [Test]
        public void GetUserByToken_TokenAndUserExist_ReturnsExpectedUser()
        {
            // arrange
            var autoMocker = new RhinoAutoMocker<UserService>();

            var tokenId = new TokenId("t");
            var userId = new UserId(Guid.NewGuid());
            var authUserId = new AuthUserId("1", "2");

            var token = new Token(
                tokenId,
                userId,
                new AuthSystemToken("t", DateTime.UtcNow.AddDays(1)),
                DateTime.UtcNow.AddDays(2));

            var tokenRepository = autoMocker.Get<ITokenRepository>();
            tokenRepository.Stub(t => t.TryGet(Arg<TokenId>.Is.Equal(tokenId), out Arg<Token>.Out(token).Dummy)).Return(true);

            var expectedUser = new User(userId, authUserId);

            var userRepository = autoMocker.Get<IUserRepository>();
            userRepository.Stub(u => u.TryGet(Arg<UserId>.Is.Equal(userId), out Arg<User>.Out(expectedUser).Dummy)).Return(true);

            // act
            var user = autoMocker.ClassUnderTest.GetUserByToken(tokenId);

            // assert
            Assert.AreSame(expectedUser, user);
        }
    }
}
