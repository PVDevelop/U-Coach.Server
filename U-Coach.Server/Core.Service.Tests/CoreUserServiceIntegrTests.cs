using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.AutoMapper;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Core.Service;
using PVDevelop.UCoach.Server.Mapper;
using Rhino.Mocks;
using TestNUnit;

namespace Core.Service.Tests
{
    [TestFixture]
   // [Integration]
    public class CoreUserServiceIntegrTests
    {
        [Test]
        public void CreateUser_UserMailProducer_SendEmail()
        {
            var mapper = new MapperImpl(config => config.AddProfile<CoreUserProfile>());

            var settings = MockRepository.GenerateStub<IEmailProducerSettings>();
            settings.Stub(s => s.SenderAddress).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.UserName).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.Password).Return("tkvXp7IvXlUEo9N7EBU7");
            settings.Stub(s => s.SmtpHost).Return("smtp.yandex.ru");
            settings.Stub(s => s.SmtpPort).Return(25);

            var mailProducer = new EmailCoreUserConfirmationProducer(settings);

            var service = new CoreUserService(
                MockRepository.GenerateStub<IUsersClient>(),
                MockRepository.GenerateStub<ICoreUserRepository>(),
                mapper,
                mailProducer);

            var userParams = new CreateUCoachUserParams()
            {
                Address = "beetlewar@mail.ru",
                ConfirmationKey = "some_key"
            };

            service.CreateUser(userParams);
        }
    }
}
