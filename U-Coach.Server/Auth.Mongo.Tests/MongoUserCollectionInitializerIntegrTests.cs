using System.Linq;
using NUnit.Framework;
using TestNUnit;
using TestMongoUtilities;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Auth.Mongo;
using MongoDB.Driver;

namespace Auth.Mongo.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class MongoUserCollectionInitializerIntegrTests
    {
        [Test]
        public void Initialize_NotInitialized_Initializes()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var initializer = new MongoUserCollectionInitializer(settings);
                initializer.Initialize();

                // проверяем индекс
                var userCollection = MongoHelper.GetCollection<MongoUser>(settings);
                var indexName = MongoHelper.GetIndexName<MongoUser>(nameof(MongoUser.Login));
                var index = userCollection.Indexes.List().ToList().FirstOrDefault(i => i["name"] == indexName);

                Assert.NotNull(index);
                Assert.IsTrue(MongoHelper.IsUniqueIndex(index));

                // проверяем версию
                var versionCollection = MongoHelper.GetCollection<CollectionVersion>(settings);
                var userCollectionName = MongoHelper.GetCollectionName<MongoUser>();
                var userCollectionVersion = MongoHelper.GetDataVersion<MongoUser>();
                var ver =
                    versionCollection.Find(col => col.TargetCollectionName == userCollectionName && col.TargetVersion == userCollectionVersion).SingleOrDefault();
                Assert.NotNull(ver);
            });
        }

        [Test]
        public void Initialize_InitTwice_DoesNotFall()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var userInitializer = new MongoUserCollectionInitializer(settings);
                userInitializer.Initialize();
                userInitializer.Initialize();
            });
        }
    }
}
