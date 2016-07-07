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
    [Category(CategoryConst.INTEGRATION)]
    public class MongoSportsmanConfirmationCollectionInitializerIntegrTests
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
                    var initializer = new MongoSportsmanConfirmationCollectionInitializer(
                        metaSettings: metaSettings,
                        contextSettings: contextSettings);
                    initializer.Initialize();

                    // проверяем индексы
                    var userCollection = MongoHelper.GetCollection<MongoSportsmanConfirmation>(contextSettings);

                    var indexAuthSystemName = MongoHelper.GetIndexName<MongoSportsmanConfirmation>(nameof(MongoSportsmanConfirmation.AuthSystem));
                    var indexAuthIdName = MongoHelper.GetIndexName<MongoSportsmanConfirmation>(nameof(MongoSportsmanConfirmation.AuthUserId));
                    var indexConfirmationKeyName = MongoHelper.GetIndexName<MongoSportsmanConfirmation>(nameof(MongoSportsmanConfirmation.ConfirmationKey));
                    var indexName = string.Format("{0}.{1}.{2}", indexAuthSystemName, indexAuthIdName, indexConfirmationKeyName);

                    var authSystemIndex =
                        userCollection.
                        Indexes.
                        List().
                        ToList().
                        FirstOrDefault(i => i["name"] == indexName);

                    Assert.NotNull(authSystemIndex, "Индекс {0} не найден", indexName);
                    Assert.IsTrue(MongoHelper.IsUniqueIndex(authSystemIndex));

                    // проверяем версию
                    var versionCollection = MongoHelper.GetCollection<CollectionVersion>(metaSettings);
                    var userCollectionName = MongoHelper.GetCollectionName<MongoSportsmanConfirmation>();
                    var userCollectionVersion = MongoHelper.GetDataVersion<MongoSportsmanConfirmation>();
                    var ver =
                        versionCollection.Find(col => col.TargetCollectionName == userCollectionName && col.TargetVersion == userCollectionVersion).SingleOrDefault();
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
                    var userInitializer = new MongoSportsmanConfirmationCollectionInitializer(
                        metaSettings: metaSettings, 
                        contextSettings: contextSettings);

                    userInitializer.Initialize();
                    userInitializer.Initialize();
                });
            });
        }
    }
}
