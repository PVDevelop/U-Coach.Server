using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using Rhino.Mocks;

namespace Core.Service.Tests
{
    [TestFixture]
    public class SportsmanConfirmationServiceTests
    {
        private static IUsersClient CreateUsersClientStub()
        {
            var usersClient = MockRepository.GenerateStub<IUsersClient>();
            usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return("1");

            return usersClient;
        }

        [Test]
        public void CreateUser_MockUsersClient_CallsCreate()
        {
            var client = MockRepository.GenerateMock<IUsersClient>();
            client.
                Expect(c => c.Create(Arg<PVDevelop.UCoach.Server.Auth.WebDto.CreateUserParams>.Matches(p=> p.Login == "login1" && p.Password == "pwd1"))).
                Return("2");

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Login = "login1",
                Password = "pwd1",
                ConfirmationKey = "someKey"
            };
            var service = new SportsmanConfirmationService(
                client, 
                MockRepository.GenerateStub<ISportsmanConfirmationRepository>(), 
                MockRepository.GenerateStub<ISportsmanConfirmationProducer>());

            service.CreateUser(userParams);

            client.VerifyAllExpectations();
        }

        [Test]
        public void CreateUser_MockUserRepository_CallsInsert()
        {
            var confirmKey = "abc";
            var rep = MockRepository.GenerateMock<ISportsmanConfirmationRepository>();
            rep.Expect(r => r.Insert(
                Arg<SportsmanConfirmation>.Matches(u => 
                    u.State == SportsmanConfirmationState.WaitingForConfirmation &&
                    u.AuthSystem == SportsmanConfirmationAuthSystem.UCoach &&
                    u.ConfirmationKey == confirmKey)));

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Login = "l1",
                Password = "p1",
                ConfirmationKey = confirmKey
            };

            var service = new SportsmanConfirmationService(
                CreateUsersClientStub(), 
                rep, 
                MockRepository.GenerateStub<ISportsmanConfirmationProducer>());

            service.CreateUser(userParams);

            rep.VerifyAllExpectations();
        }

        [Test]
        public void CreateUser_MockProducer_CallsProduce()
        {
            var confirmKey = "cde";
            var producer = MockRepository.GenerateMock<ISportsmanConfirmationProducer>();
            producer.Expect(r => r.Produce(
                Arg<ProduceConfirmationKeyParams>.Matches(p =>
                    p.ConfirmationKey == confirmKey &&
                    p.Address == "kuda-to")));

            var userParams = new CreateSportsmanConfirmationParams()
            {
                ConfirmationKey = confirmKey,
                Address = "kuda-to"
            };

            var service = new SportsmanConfirmationService(
                CreateUsersClientStub(),
                MockRepository.GenerateStub<ISportsmanConfirmationRepository>(),
                producer);

            service.CreateUser(userParams);

            producer.VerifyAllExpectations();
        }
    }
}
