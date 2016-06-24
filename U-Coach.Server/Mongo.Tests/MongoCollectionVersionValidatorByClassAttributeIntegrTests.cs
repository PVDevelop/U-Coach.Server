using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo.Exceptions;
using TestMongoUtilities;
using TestNUnit;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    [Integration]
    public class MongoCollectionVersionValidatorByClassAttributeIntegrTests
    {
        [Test]
        public void Validate_TypeWithMongoVersionAttributeAndValidVersion_PassesValidation()
        {
            var validationSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(validationSettings, db =>
            {
                var collectionVersion = new CollectionVersion()
                {
                    Name = MongoHelper.GetCollectionName<TestMongoObj>(),
                    TargetVersion = 456
                };

                var collectionVersionName = MongoHelper.GetCollectionName<CollectionVersion>();
                var coll = db.GetCollection<CollectionVersion>(collectionVersionName);
                coll.InsertOne(collectionVersion);

                var validator = new MongoCollectionVersionValidatorByClassAttribute(validationSettings);
                validator.Validate<TestMongoObj>();
            });
        }

        [TestCase(null)]
        [TestCase(13)]
        [TestCase(500)]
        public void Validate_TypeWithMongoVersionAttributeAndInvalidVersion_ThrowsException(int? version)
        {
            var validationSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(validationSettings, db =>
            {
                if (version.HasValue)
                {
                    var collectionVersion = new CollectionVersion()
                    {
                        Name = MongoHelper.GetCollectionName<TestMongoObj>(),
                        TargetVersion = version.Value
                    };

                    var collectionVersionName = MongoHelper.GetCollectionName<CollectionVersion>();
                    var coll = db.GetCollection<CollectionVersion>(collectionVersionName);
                    coll.InsertOne(collectionVersion);
                }

                Assert.Throws(
                    typeof(MongoCollectionNotInitializedException),
                    () =>
                    {
                        var validator = new MongoCollectionVersionValidatorByClassAttribute(validationSettings);
                        validator.Validate<TestMongoObj>();
                    });
            });
        }
    }
}
