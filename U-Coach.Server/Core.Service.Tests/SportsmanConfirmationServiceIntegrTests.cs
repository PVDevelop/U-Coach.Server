using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using Rhino.Mocks;
using TestNUnit;

namespace Core.Service.Tests
{
    [TestFixture]
    //[Integration]
    public class SportsmanConfirmationServiceIntegrTests
    {
#warning доделать тест
        //[Test]
        public void CreateUser_UserMailProducer_SendsEmail()
        {
            var settings = MockRepository.GenerateStub<IEmailProducerSettings>();
            settings.Stub(s => s.SenderAddress).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.UserName).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.Password).Return("tkvXp7IvXlUEo9N7EBU7");
            settings.Stub(s => s.SmtpHost).Return("smtp.yandex.ru");
            settings.Stub(s => s.SmtpPort).Return(25);

            var mailProducer = new EmailConfirmationProducer(settings);

            var service = new SportsmanConfirmationService(
                MockRepository.GenerateStub<IUsersClient>(),
                MockRepository.GenerateStub<ISportsmanConfirmationRepository>(),
                mailProducer);

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Address = "beetlewar@mail.ru",
                ConfirmationKey = "some_key"
            };

            service.CreateUser(userParams);
        }
    }
}
