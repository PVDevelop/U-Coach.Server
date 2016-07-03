using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Core.Service;
using Rhino.Mocks;
using System;
using StructureMap.AutoMocking;
using PVDevelop.UCoach.Server.Configuration;

namespace Core.Service.Tests
{
    [TestFixture]
    //[Integration]
    public class SportsmanConfirmationServiceIntegrTests
    {
#warning доделать тест
        [Test]
        public void CreateUser_UserMailProducer_SendsEmail()
        {
            var autoMocker = new RhinoAutoMocker<SportsmanConfirmationService>();

            var settings = MockRepository.GenerateStub<IEmailProducerSettings>();
            settings.Stub(s => s.SenderAddress).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.UserName).Return("PVDevelop@yandex.ru");
            settings.Stub(s => s.Password).Return("tkvXp7IvXlUEo9N7EBU7");
            settings.Stub(s => s.SmtpHost).Return("smtp.yandex.ru");
            settings.Stub(s => s.EnableSsl).Return(true);
            settings.Stub(s => s.SmtpPort).Return(25);

            var settingsProvider = autoMocker.Get<ISettingsProvider<IEmailProducerSettings>>();
            settingsProvider.Stub(s => s.Settings).Return(settings);

            var usersClient = autoMocker.Get<IUsersClient>();
            usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return(Guid.NewGuid().ToString());

            autoMocker.Inject<ISportsmanConfirmationProducer>(new EmailConfirmationProducer(settingsProvider));

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Address = "PVDevelop@yandex.ru",
                ConfirmationKey = "some_key"
            };

            autoMocker.ClassUnderTest.CreateConfirmation(userParams);
        }
    }
}
