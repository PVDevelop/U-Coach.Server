using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Core.Mail;
using PVDevelop.UCoach.Server.Core.Service;
using Rhino.Mocks;
using System;
using StructureMap.AutoMocking;
using PVDevelop.UCoach.Server.Configuration;
using Limilabs.Client.IMAP;
using System.Linq;
using System.Collections.Generic;
using TestNUnit;

namespace Core.Service.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class SportsmanConfirmationServiceIntegrTests
    {
        #region static

        private static T WithInboxImap<T>(
            string userName,
            string password,
            Func<Imap, T> callback)
        {
            using (var imap = new Imap())
            {
                imap.Connect("imap.yandex.ru", 993, true);
                imap.Login(userName, password);
                imap.SelectInbox();
                return callback(imap);
            }
        }

        private static long? GetLastUid(
            string userName,
            string password)
        {
            return WithInboxImap(userName, password, imap =>
            {
                var ids =
                    imap.
                    Search(Expression.UID(Range.Last()));

                return
                    ids.Any() ?
                    (long?)ids.Single() :
                    null;
            });
        }

        private static IList<long> GetAfter(
            string userName,
            string password,
            long? id)
        {
            return WithInboxImap(userName, password, imap =>
                id.HasValue ?
                imap.
                    Search(Expression.UID(Range.From(id.Value))).
                    Except(new[] { id.Value }).
                    ToList() :
                imap.GetAll());
        }

        private static bool FindMail(Imap imap, long id, string confirmationKey)
        {
            var builder = new Limilabs.Mail.MailBuilder();
            var eml = imap.GetMessageByUID(id);
            var email = builder.CreateFromEml(eml);

            return email.GetBodyAsText().Contains(confirmationKey);
        }

        #endregion

#warning 1 - тест не стабилен. 2 - надо поднимать локальный почтовый сервер
        //[Test]
        public void CreateUser_WithUserMailProducer_SendsEmail()
        {
            // отправляем
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
            usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return(new CreateUserResultDto() { Id = Guid.NewGuid().ToString() });

            autoMocker.Inject<ISportsmanConfirmationProducer>(new EmailConfirmationProducer(settingsProvider));

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Address = "PVDevelop@yandex.ru",
                ConfirmationKey = Guid.NewGuid().ToString()
            };

            // получаем последнее письмо
            var lastUid = GetLastUid(settings.UserName, settings.Password);

            // отправляем письма
            autoMocker.ClassUnderTest.CreateConfirmation(userParams);

            // получаем последние писаьма и находим нужное
            var newMails = GetAfter(settings.UserName, settings.Password, lastUid);
            Assert.IsTrue(newMails.Any());

            WithInboxImap(settings.UserName, settings.Password, imap =>
            {
                Assert.IsTrue(
                    newMails.Any(id => FindMail(imap, id, userParams.ConfirmationKey)),
                    "Ни одно письмо с ключом {0} не найдено",
                    userParams.ConfirmationKey);

                return "";  // просто фейковый return, т.к. func должен что-то вернуть
            });
        }
    }
}
