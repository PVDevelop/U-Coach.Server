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
        //[Test]
        //public void CreateUser_UserMailProducer_SendsEmail()
        //{
        //    var header = string.Format("TEST_{0}", Guid.NewGuid());

        //    var autoMocker = new RhinoAutoMocker<EmailConfirmationProducer>();
        //    var settings = MockRepository.GenerateStub<IEmailProducerSettings>();
        //    settings.Stub(s => s.SenderAddress).Return("PVDevelop@yandex.ru");
        //    settings.Stub(s => s.UserName).Return("PVDevelop@yandex.ru");
        //    settings.Stub(s => s.Password).Return("tkvXp7IvXlUEo9N7EBU7");
        //    settings.Stub(s => s.SmtpHost).Return("smtp.yandex.ru");
        //    settings.Stub(s => s.EnableSsl).Return(true);
        //    settings.Stub(s => s.SmtpPort).Return(25);
        //    settings.Stub(s => s.Header).Return(header);

        //    var settingsProvider = autoMocker.Get<ISettingsProvider<IEmailProducerSettings>>();
        //    settingsProvider.Stub(sp => sp.Settings).Return(settings);

        //    var usersClient = MockRepository.GenerateStub<IUsersClient>();
        //    usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return("1");

        //    var service = new SportsmanConfirmationService(
        //        usersClient,
        //        MockRepository.GenerateStub<ISportsmanConfirmationRepository>(),
        //        mailProducer);

        //    var userParams = new CreateSportsmanConfirmationParams()
        //    {
        //        Address = "PVDevelop@yandex.ru",
        //        ConfirmationKey = "some_key"
        //    };

        //    service.CreateConfirmation(userParams);
        //}
    }
}
