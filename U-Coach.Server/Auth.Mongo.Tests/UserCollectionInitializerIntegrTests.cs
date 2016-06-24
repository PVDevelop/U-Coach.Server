using System.Linq;
using NUnit.Framework;
using TestNUnit;
using TestMongoUtilities;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Auth.Domain;
using MongoDB.Driver;

namespace Auth.Mongo.Tests
{
    [TestFixture]
    [Integration]
    public class UserCollectionInitializerIntegrTests
    {
        [Test]
        public void Initialize_NotInitialized_Initializes()
        {
            var metaSettings = TestMongoHelper.CreateSettings();
            var contextSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(contextSettings, contextDb =>
            {
                TestMongoHelper.WithDb(metaSettings, metaDb =>
                {
                    var initializer = new MongoUserCollectionInitializer(metaSettings: metaSettings, contextSettings: contextSettings);
                    initializer.Initialize();

                    // проверяем индекс
                    var userCollection = MongoHelper.GetCollection<User>(contextSettings);
                    var indexName = MongoHelper.GetIndexName<User>(nameof(User.Login));
                    var index = userCollection.Indexes.List().ToList().FirstOrDefault(i => i["name"] == indexName);

                    Assert.NotNull(index);
                    Assert.IsTrue(MongoHelper.IsUniqueIndex(index));

                    // проверяем версию
                    var versionCollection = MongoHelper.GetCollection<CollectionVersion>(metaSettings);
                    var userCollectionName = MongoHelper.GetCollectionName<User>();
                    var userCollectionVersion = MongoHelper.GetDataVersion<User>();
                    var ver =
                        versionCollection.Find(col => col.Name == userCollectionName && col.TargetVersion == userCollectionVersion).SingleOrDefault();
                    Assert.NotNull(ver);
                });
            });
        }

        [Test]
        public void Initialize_InitTwice_DoesNotFall()
        {
            var metaSettings = TestMongoHelper.CreateSettings();
            var contextSettings = TestMongoHelper.CreateSettings();

            TestMongoHelper.WithDb(contextSettings, contextDb =>
            {
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
