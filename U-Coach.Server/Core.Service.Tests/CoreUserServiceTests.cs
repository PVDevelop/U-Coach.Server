using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using Rhino.Mocks;

namespace Core.Service.Tests
{
    [TestFixture]
    public class CoreUserServiceTests
    {
        private static IMapper WithCoreUser(IMapper mapper, CreateUCoachUserParams userParams)
        {
            mapper.
                Stub(m => m.Map<PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams>(null)).
                IgnoreArguments().
                Return(new PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams()
                {
                    Login = userParams.Login,
                    Password = userParams.Password
                });

            return mapper;
        }

        private static IMapper WithProducer(IMapper mapper, CreateUCoachUserParams userParams)
        {
            mapper.
                Stub(m => m.Map<ProduceConfirmationKeyParams>(null)).
                IgnoreArguments().
                Return(new ProduceConfirmationKeyParams()
                {
                    Address = userParams.Address,
                    ConfirmationKey = userParams.ConfirmationKey
                });

            return mapper;
        }

        [Test]
        public void CreateUser_MockUsersClient_CallsCreate()
        {
            var client = MockRepository.GenerateMock<IUsersClient>();
            client.Expect(c => c.Create(Arg<PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams>.Matches(p=> p.Login == "login1" && p.Password == "pwd1")));

            var userParams = new CreateUCoachUserParams()
            {
                Login = "login1",
                Password = "pwd1"
            };
            var service = new CoreUserService(
                client, 
                MockRepository.GenerateStub<ICoreUserRepository>(), 
                WithCoreUser(MockRepository.GenerateStub<IMapper>(), userParams),
                MockRepository.GenerateStub<ICoreUserConfirmationProducer>());

            service.CreateUser(userParams);

            client.VerifyAllExpectations();
        }

        [Test]
        public void CreateUser_MockUserRepository_CallsInsert()
        {
            var confirmKey = "abc";
            var rep = MockRepository.GenerateMock<ICoreUserRepository>();
            rep.Expect(r => r.Insert(
                Arg<ICoreUser>.Matches(u => 
                    u.State == CoreUserState.WaitingForConfirmation &&
                    u.AuthSystem == CoreUserAuthSystem.UCoach &&
                    u.ConfirmationKey == confirmKey)));

            var service = new CoreUserService(
                MockRepository.GenerateStub<IUsersClient>(), 
                rep, 
                MockRepository.GenerateStub<IMapper>(),
                MockRepository.GenerateStub<ICoreUserConfirmationProducer>());

            var userParams = new CreateUCoachUserParams()
            {
                Login = "l1",
                Password = "p1",
                ConfirmationKey = confirmKey
            };

            service.CreateUser(userParams);

            rep.VerifyAllExpectations();
        }

        [Test]
        public void CreateUser_MockProducer_CallsProduce()
        {
            var confirmKey = "cde";
            var producer = MockRepository.GenerateMock<ICoreUserConfirmationProducer>();
            producer.Expect(r => r.Produce(
                Arg<ProduceConfirmationKeyParams>.Matches(p =>
                    p.ConfirmationKey == confirmKey &&
                    p.Address == "kuda-to")));

            var userParams = new CreateUCoachUserParams()
            {
                ConfirmationKey = confirmKey,
                Address = "kuda-to"
            };

            var service = new CoreUserService(
                MockRepository.GenerateStub<IUsersClient>(),
                MockRepository.GenerateStub<ICoreUserRepository>(),
                WithProducer(MockRepository.GenerateStub<IMapper>(), userParams),
                producer);

            service.CreateUser(userParams);

            producer.VerifyAllExpectations();
        }
    }
}
