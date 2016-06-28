using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Core.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using System.Linq;
using TestMongoUtilities;
using TestNUnit;

namespace Core.Mongo.Tests
{
    [TestFixture]
    //[Integration]
    public class MongoCoreUserCollectionInitializerIntegrTests
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
                    var initializer = new MongoCoreUserCollectionInitializer(
                        metaSettings: metaSettings,
                        contextSettings: contextSettings);
                    initializer.Initialize();

                    // проверяем индексы
                    var userCollection = MongoHelper.GetCollection<MongoCoreUser>(contextSettings);

                    var indexAuthSystemName = MongoHelper.GetIndexName<MongoCoreUser>(nameof(MongoCoreUser.AuthSystem));
                    var indexAuthIdName = MongoHelper.GetIndexName<MongoCoreUser>(nameof(MongoCoreUser.AuthId));
                    var indexName = string.Format("{0}.{1}", indexAuthSystemName, indexAuthIdName);

                    var authSystemIndex =
                        userCollection.
                        Indexes.
                        List().
                        ToList().
                        FirstOrDefault(i => i["name"] == indexName);

                    Assert.NotNull(authSystemIndex);
                    Assert.IsTrue(MongoHelper.IsUniqueIndex(authSystemIndex));

                    // проверяем версию
                    var versionCollection = MongoHelper.GetCollection<CollectionVersion>(metaSettings);
                    var userCollectionName = MongoHelper.GetCollectionName<MongoCoreUser>();
                    var userCollectionVersion = MongoHelper.GetDataVersion<MongoCoreUser>();
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

                    var userInitializer = new MongoCoreUserCollectionInitializer(
                        metaSettings: metaSettings, 
                        contextSettings: contextSettings);

                    userInitializer.Initialize();
                    userInitializer.Initialize();
                });
            });
        }
    }
}
