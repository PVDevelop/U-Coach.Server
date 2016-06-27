using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using Rhino.Mocks;

namespace Core.Service.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public void CreateUser_MockUsersClient_CallsCreate()
        {
            var client = MockRepository.GenerateMock<IUsersClient>();
            client.Expect(c => c.Create(Arg<PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams>.Matches(p=> p.Login == "login1" && p.Password == "pwd1")));

            var mapper = MockRepository.GenerateStub<IMapper>();
            mapper.
                Stub(m => m.Map<PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams>(null)).
                IgnoreArguments().
                Return(new PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams() { Login = "login1", Password = "pwd1" });

            var service = new UserService(client, mapper);
            var userParams = new CreateUserParams()
            {
                Login = "login1",
                Password = "pwd1"
            };

            service.CreateUser(userParams);

            client.VerifyAllExpectations();
        }
    }
}
