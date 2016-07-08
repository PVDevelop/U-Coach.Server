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
using SmtpServer;
using System.Threading;
using SmtpServer.Storage;
using SmtpServer.Mail;
using System.Threading.Tasks;
using SmtpServer.Authentication;
using SmtpServer.Protocol.Text;

namespace Core.Service.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class SportsmanConfirmationServiceIntegrTests
    {
        #region support classes

        private class TestMessageStoreFactory : IMessageStoreFactory
        {
            private readonly MessageStore _store;

            public TestMessageStoreFactory(MessageStore store)
            {
                if (store == null)
                {
                    throw new ArgumentNullException(nameof(store));
                }
                _store = store;
            }

            public IMessageStore CreateInstance(ISessionContext context)
            {
                return _store;
            }
        }

        private class TestMessageStore : MessageStore
        {
            private readonly List<IMimeMessage> _messages = new List<IMimeMessage>();

            public IMimeMessage[] Messages
            {
                get { return _messages.ToArray(); }
            }

            public override Task<string> SaveAsync(
                ISessionContext context,
                IMimeMessage message,
                CancellationToken cancellationToken)
            {
                _messages.Add(message);
                return Task.Run(() => "hello, it's me", cancellationToken);
            }
        }

        #endregion

        #region support methods

        private static string ConvertFromBase64(string base64String)
        {
            var stringBytes = Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(stringBytes);
        }

        #endregion

        [Test]
        public void CreateUser_WithUserMailProducer_SendsEmail()
        {
            // arrange
            var store = new TestMessageStore();

            var options =
                new OptionsBuilder().
                ServerName("localhost").
                Port(6000).
                MessageStore(new TestMessageStoreFactory(store));

            var server = new SmtpServer.SmtpServer(options.Build());

            var tokenSource = new CancellationTokenSource();
            var serverTask = server.StartAsync(tokenSource.Token);

            var autoMocker = new RhinoAutoMocker<SportsmanConfirmationService>();

            var settings = autoMocker.Get<IEmailProducerSettings>();
            settings.Stub(s => s.SenderAddress).Return("from@test.ru");
            settings.Stub(s => s.UserName).Return("some_user");
            settings.Stub(s => s.Password).Return("some_password");
            settings.Stub(s => s.SmtpHost).Return("localhost");
            settings.Stub(s => s.SmtpPort).Return(6000);

            var settingsProvider = autoMocker.Get<ISettingsProvider<IEmailProducerSettings>>();
            settingsProvider.Stub(s => s.Settings).Return(settings);

            var usersClient = autoMocker.Get<IUsersClient>();
            usersClient.Stub(uc => uc.Create(null)).IgnoreArguments().Return(new CreateUserResultDto() { Id = Guid.NewGuid().ToString() });

            autoMocker.Inject<ISportsmanConfirmationProducer>(new EmailConfirmationProducer(settingsProvider));

            var userParams = new CreateSportsmanConfirmationParams()
            {
                Address = "to@test.ru",
                ConfirmationKey = Guid.NewGuid().ToString()
            };

            // act
            autoMocker.ClassUnderTest.CreateConfirmation(userParams);

            // assert
            Assert.AreEqual(1, store.Messages.Length);
            var message = store.Messages.Single();
            Assert.AreEqual("test.ru", message.From.Host);
            Assert.AreEqual("from", message.From.User);

            Assert.AreEqual(1, message.To.Count);
            var to = message.To.Single();
            Assert.AreEqual("test.ru", to.Host);
            Assert.AreEqual("to", to.User);

            var headerAndBody = message.Mime.ToString().Split(new[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var body = ConvertFromBase64(headerAndBody[1]);
            Assert.True(body.Contains(userParams.ConfirmationKey));
        }
    }
}
