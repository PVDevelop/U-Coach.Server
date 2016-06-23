using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Tests;

namespace PVDevelop.UCoach.Server.AuthService.Tests
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
                    var initializer = new UserCollectionInitializer(metaSettings: metaSettings, contextSettings: contextSettings);
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

                    var userInitializer = new UserCollectionInitializer(metaSettings: metaSettings, contextSettings: contextSettings);
                    userInitializer.Initialize();
                    userInitializer.Initialize();
                });
            });
        }
    }
}
