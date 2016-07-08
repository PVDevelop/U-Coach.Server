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

            var dto = new RegisterFacebookUserDto()
            {
                Id = "1"
            };
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, dto.Id);

            var repository = autoMocker.Get<IUserRepository>();
            repository.Stub(r => r.Contains(userId)).Return(false);
            repository.
                Expect(r => r.Insert(Arg<IUser>.Matches(u => u.Id.Equals(userId))));

            var factory = new UserFactory();
            autoMocker.Inject(typeof(IUserFactory), factory);

            autoMocker.ClassUnderTest.RegisterFacebookUser(dto);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void RegisterFacebookUser_UserExists_DoesNotCallsInsert()
        {
            var autoMocker = new RhinoAutoMocker<UserService>();

            var dto = new RegisterFacebookUserDto()
            {
                Id = "2"
            };
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, dto.Id);

            var repository = autoMocker.Get<IUserRepository>();
            repository.Stub(r => r.Contains(userId)).Return(true);
            repository.Expect(r => r.Insert(Arg<IUser>.Matches(u => u.Id.Equals(userId)))).Repeat.Never();

            var factory = new UserFactory();
            autoMocker.Inject(typeof(IUserFactory), factory);
            autoMocker.ClassUnderTest.RegisterFacebookUser(dto);

            repository.VerifyAllExpectations();
        }
    }
}
