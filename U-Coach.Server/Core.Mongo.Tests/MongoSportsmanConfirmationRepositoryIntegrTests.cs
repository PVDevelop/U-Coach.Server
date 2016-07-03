using System;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Exceptions;
using StructureMap.AutoMocking;
using TestMongoUtilities;
using TestNUnit;

namespace Core.Mongo.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class MongoSportsmanConfirmationRepositoryIntegrTests
    {
        private static RhinoAutoMocker<MongoSportsmanConfirmationRepository> CreateAndSetupAutoMocker(IMongoConnectionSettings settings)
        {
            var initializer = new MongoSportsmanConfirmationCollectionInitializer(settings, settings);
            initializer.Initialize();

            var autoMocker = new RhinoAutoMocker<MongoSportsmanConfirmationRepository>();

            var versionValidator = new MongoCollectionVersionValidatorByClassAttribute(settings);
            autoMocker.Inject(typeof(IMongoCollectionVersionValidator), versionValidator);

            var repository = new MongoRepository<MongoSportsmanConfirmation>(settings);
            autoMocker.Inject(typeof(IMongoRepository<MongoSportsmanConfirmation>), repository);

            return autoMocker;
        }

        [Test]
        public void Insert_NotInitialized_ThrowsException()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoSportsmanConfirmationRepository>();

                var versionValidator = new MongoCollectionVersionValidatorByClassAttribute(settings);
                autoMocker.Inject(typeof(IMongoCollectionVersionValidator), versionValidator);

                var repository = new MongoRepository<MongoSportsmanConfirmation>(settings);
                autoMocker.Inject(typeof(IMongoRepository<MongoSportsmanConfirmation>), repository);

                var confirmation = SportsmanConfirmationFactory.CreateSportsmanConfirmation("1", "2");
                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Insert(confirmation));
            });
        }

        [Test]
        public void Insert_Initialized_SavesToDb()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = CreateAndSetupAutoMocker(settings);

                var confirmation = SportsmanConfirmationFactory.CreateSportsmanConfirmation("1", "2");
                autoMocker.ClassUnderTest.Insert(confirmation);

                var collection = MongoHelper.GetCollection<MongoSportsmanConfirmation>(settings);
                var mongoConfirmation = collection.Find(sc => sc.Id == confirmation.Id).ToList().SingleOrDefault();

                Assert.NotNull(mongoConfirmation);
                Assert.AreEqual(MongoSportsmanConfirmation.VERSION, mongoConfirmation.Version);
                Assert.AreEqual(confirmation.AuthSystem, mongoConfirmation.AuthSystem);
                Assert.AreEqual(confirmation.AuthUserId, mongoConfirmation.AuthUserId);
                Assert.AreEqual(confirmation.ConfirmationKey, mongoConfirmation.ConfirmationKey);
                Assert.AreEqual(confirmation.State, mongoConfirmation.State);
            });
        }

        [Test]
        public void FindByConfirmationKey_KeyIsValid_ReturnsExpectedSportsman()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = CreateAndSetupAutoMocker(settings);

                var mongoConfirmation = new MongoSportsmanConfirmation()
                {
                    Id = Guid.NewGuid(),
                    AuthSystem = SportsmanConfirmationAuthSystem.UCoach,
                    AuthUserId = "someId",
                    ConfirmationKey = "confirm_key",
                    State = SportsmanConfirmationState.WaitingForConfirmation
                };

                var collecton = MongoHelper.GetCollection<MongoSportsmanConfirmation>(settings);
                collecton.InsertOne(mongoConfirmation);

                var confirmation = autoMocker.ClassUnderTest.FindByConfirmationKey(mongoConfirmation.ConfirmationKey);

                Assert.NotNull(confirmation);
                Assert.AreEqual(mongoConfirmation.Id, confirmation.Id);
                Assert.AreEqual(mongoConfirmation.AuthSystem, confirmation.AuthSystem);
                Assert.AreEqual(mongoConfirmation.AuthUserId, confirmation.AuthUserId);
                Assert.AreEqual(mongoConfirmation.ConfirmationKey, confirmation.ConfirmationKey);
                Assert.AreEqual(mongoConfirmation.State, confirmation.State);
            });
        }
    }
}
