using MongoDB.Driver;
using NUnit.Framework;
using System.Linq;
using TestMongoUtilities;
using TestNUnit;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class MetaInitializerIntegrTests
    {
        [Test]
        public void Initialize_NotInitialized_CreatesCollectionWithIndex()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, contextDb =>
            {
                var initializer = new MongoMetaInitializer(settings);
                initializer.Initialize();

                // проверяем индекс
                var collection = MongoHelper.GetCollection<CollectionVersion>(settings);
                var indexName = MongoHelper.GetIndexName<CollectionVersion>(nameof(CollectionVersion.Name));
                var index = collection.Indexes.List().ToList().FirstOrDefault(i => i["name"] == indexName);

                Assert.NotNull(index);
                Assert.IsTrue(MongoHelper.IsUniqueIndex(index));
            });
        }

        [Test]
        public void Initialize_InitTwice_DoesNotFall()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, contextDb =>
            {
                var initializer = new MongoMetaInitializer(settings);
                initializer.Initialize();
                initializer.Initialize();
            });
        }
    }
}
