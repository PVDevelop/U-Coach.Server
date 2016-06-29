using System;
using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Core.AutoMapper;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Exceptions;
using StructureMap.AutoMocking;
using TestMongoUtilities;

namespace Core.Mongo.Tests
{
    [TestFixture]
    //[Integration]
    public class MongoSportsmanConfirmationRepositoryIntegrTests
    {
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

                var confirmation = autoMocker.Get<ISportsmanConfirmation>();
                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Insert(confirmation));
            });
        }

        [Test]
        public void Insert_Initialized_SavesToDb()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var initializer = new MongoSportsmanConfirmationCollectionInitializer(settings, settings);
                initializer.Initialize();

                var autoMocker = new RhinoAutoMocker<MongoSportsmanConfirmationRepository>();

                //var mapper = new MapperImpl(new Action<AutoMapper.IMapperConfiguration>(cfg => cfg.AddProfile<SportsmanConfirmationProfile>()));

                var versionValidator = new MongoCollectionVersionValidatorByClassAttribute(settings);
                autoMocker.Inject(typeof(IMongoCollectionVersionValidator), versionValidator);

                var repository = new MongoRepository<MongoSportsmanConfirmation>(settings);
                autoMocker.Inject(typeof(IMongoRepository<MongoSportsmanConfirmation>), repository);

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
    }
}
