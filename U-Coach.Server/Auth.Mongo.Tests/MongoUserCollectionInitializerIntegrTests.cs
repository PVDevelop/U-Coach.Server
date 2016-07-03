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
            var contextSettings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(contextSettings, contextDb =>
            {
                var metaSettings = TestMongoHelper.CreateSettings();
                TestMongoHelper.WithDb(metaSettings, metaDb =>
                {
                    var initializer = new MongoUserCollectionInitializer(metaSettings: metaSettings, contextSettings: contextSettings);
                    initializer.Initialize();

                    // проверяем индекс
                    var userCollection = MongoHelper.GetCollection<MongoUser>(contextSettings);
                    var indexName = MongoHelper.GetIndexName<MongoUser>(nameof(MongoUser.Login));
                    var index = userCollection.Indexes.List().ToList().FirstOrDefault(i => i["name"] == indexName);

                    Assert.NotNull(index);
                    Assert.IsTrue(MongoHelper.IsUniqueIndex(index));

                    // проверяем версию
                    var versionCollection = MongoHelper.GetCollection<CollectionVersion>(metaSettings);
                    var userCollectionName = MongoHelper.GetCollectionName<MongoUser>();
                    var userCollectionVersion = MongoHelper.GetDataVersion<MongoUser>();
                    var ver =
                        versionCollection.Find(col => col.Name == userCollectionName && col.TargetVersion == userCollectionVersion).SingleOrDefault();
                    Assert.NotNull(ver);
                });
            });
        }

        [Test]
        public void Initialize_InitTwice_DoesNotFall()
        {
            var contextSettings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(contextSettings, contextDb =>
            {
                var metaSettings = TestMongoHelper.CreateSettings();
                TestMongoHelper.WithDb(metaSettings, metaDb =>
                {
                    var metaInitializer = new MongoMetaInitializer(metaSettings);
                    metaInitializer.Initialize();

                    var userInitializer = new MongoUserCollectionInitializer(metaSettings: metaSettings, contextSettings: contextSettings);
                    userInitializer.Initialize();
                    userInitializer.Initialize();
                });
            });
        }
    }
}
