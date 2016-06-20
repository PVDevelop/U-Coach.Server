using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Linq;
using PVDevelop.UCoach.Server.Exceptions.Mongo;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    [Integration]
    class MongoRepositoryIntegrTests
    {
        [Test]
        public void Insert_ValidObject_SavesToDb()
        {
            var settings = TestMongoHelper.CreateSettings();

            var testObj = new TestObj()
            {
                Name = "SomeName"
            };

            TestMongoHelper.WithDb(settings, db =>
            {
                var rep = new MongoRepository<TestObj>(settings, MockRepository.GenerateStub<IMongoCollectionVersionValidator>());
                rep.Insert(testObj);

                var coll =
                    new MongoClient(settings.ConnectionString).
                    GetDatabase(settings.DatabaseName).
                    GetCollection<TestObj>(MongoHelper.GetCollectionName<TestObj>());

                var foundObj = coll.Find(o => o.Id == testObj.Id && o.Name == testObj.Name).FirstOrDefault();
                Assert.NotNull(foundObj, "Объект не был сохранен в MongoDb");
            });
        }

        [Test]
        public void Ctor_TypeWithMongoVersionAttributeAndValidVersion_PassesValidation()
        {
            var validationSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(validationSettings, db =>
            {
                var collectionVersion = new CollectionVersion()
                {
                    Name = MongoHelper.GetCollectionName<TestObj>(),
                    Version = 456
                };

                var collectionVersionName = MongoHelper.GetCollectionName<CollectionVersion>();
                var coll = db.GetCollection<CollectionVersion>(collectionVersionName);
                coll.InsertOne(collectionVersion);

                var validator = new MongoCollectionVersionValidatorByClassAttribute(validationSettings);
                new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), validator);
            });
        }

        [TestCase(null)]
        [TestCase(13)]
        [TestCase(500)]
        public void Ctor_TypeWithMongoVersionAttributeAndInvalidVersion_ThrowsException(int? version)
        {
            var validationSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(validationSettings, db =>
            {
                if (version.HasValue)
                {
                    var collectionVersion = new CollectionVersion()
                    {
                        Name = MongoHelper.GetCollectionName<TestObj>(),
                        Version = version.Value
                    };

                    var collectionVersionName = MongoHelper.GetCollectionName<CollectionVersion>();
                    var coll = db.GetCollection<CollectionVersion>(collectionVersionName);
                    coll.InsertOne(collectionVersion);
                }

                var validator = new MongoCollectionVersionValidatorByClassAttribute(validationSettings);
                Assert.Throws(
                    typeof(InvalidDataVersionException),
                    () => new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), validator));
            });
        }
    }
}
