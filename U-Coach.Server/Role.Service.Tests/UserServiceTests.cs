using System;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Service;
using Rhino.Mocks;
using StructureMap.AutoMocking;

namespace Role.Service.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void RegisterFacebookUser_UserDoesNotExist_CallsInsert()
        {
            var autoMocker = new RhinoAutoMocker<UserService>();

            var dto = new FacebookConnectionDto()
            {
                TokenExpiration = DateTime.UtcNow,
                Id = "1",
                Token = "token"
            };
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, dto.Id);
            var token = new AuthToken(dto.Token, dto.TokenExpiration);

            var repository = autoMocker.Get<IUserRepository>();

            repository.Stub(r => r.TryGet(
                Arg<UserId>.Is.Equal(userId), 
                out Arg<IUser>.Out(null).Dummy)).Return(false);

            repository.
                Expect(r => r.Insert(Arg<IUser>.Matches(u =>
                u.Id.Equals(userId) &&
                u.Token.Equals(token))));

            var factory = new UserFactory();
            autoMocker.Inject(typeof(IUserFactory), factory);

            autoMocker.ClassUnderTest.RegisterFacebookUser(dto);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterFacebookUser_UserExists_CallsUpdate()
        {
            var autoMocker = new RhinoAutoMocker<UserService>();

            var dto = new FacebookConnectionDto()
            {
                Id = "2",
                Token = "t",
                TokenExpiration = DateTime.UtcNow
            };
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, dto.Id);
            var userToken = new AuthToken(dto.Token, dto.TokenExpiration);

            var repository = autoMocker.Get<IUserRepository>();

            var user = new UserFactory().CreateUser(userId);
            repository.
                Stub(r => r.TryGet(
                    Arg<UserId>.Is.Equal(userId), 
                    out Arg<IUser>.Out(user).Dummy)).
                Return(true);

            repository.Expect(r => r.Update(Arg<IUser>.Matches(u =>
                u.Id.Equals(userId) &&
                u.Token.Equals(userToken))));

            var factory = new UserFactory();
            autoMocker.Inject(typeof(IUserFactory), factory);
            autoMocker.ClassUnderTest.RegisterFacebookUser(dto);

            repository.VerifyAllExpectations();
        }
    }
}
