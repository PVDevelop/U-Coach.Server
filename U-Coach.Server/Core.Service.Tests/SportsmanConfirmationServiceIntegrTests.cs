using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Core.Service;
using Rhino.Mocks;

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
            settings.Stub(s => s.EnableSsl).Return(true);
            settings.Stub(s => s.SmtpPort).Return(25);

            var mailProducer = new EmailConfirmationProducer(settings);

            var usersClient = MockRepository.GenerateStub<IUsersClient>();
            usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return("1");

            var service = new SportsmanConfirmationService(
                usersClient,
                MockRepository.GenerateStub<ISportsmanConfirmationRepository>(),
                mailProducer);

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Address = "beetlewar@mail.ru",
                ConfirmationKey = "some_key"
            };

            service.CreateConfirmation(userParams);
        }
    }
}
