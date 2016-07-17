using System;
using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Role.Domain;
using PVDevelop.UCoach.Server.Role.Mongo;
using StructureMap.AutoMocking;
using TestComparisonUtilities;
using TestMongoUtilities;
using TestNUnit;

namespace Role.Mongo.Tests
{
    [TestFixture]
    [Category(CategoryConst.INTEGRATION)]
    public class UserRepositoryIntegrTests
    {
        private static RhinoAutoMocker<UserRepository> CreateRepository(IConnectionStringProvider settings)
        {
            var autoMocker = new RhinoAutoMocker<UserRepository>();

            var mongoRepository = new MongoRepository<MongoUser>(settings);
            autoMocker.Inject(typeof(IMongoRepository<MongoUser>), mongoRepository);

            return autoMocker;
        }

        [Test]
        public void Insert_UserDoesNotExist_CreatesNew()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = CreateRepository(settings);

                var userId = new UserId("someSystem", "someId");
                var user = new User(userId);

                autoMocker.ClassUnderTest.Insert(user);

                var collection = MongoHelper.GetCollection<MongoUser>(settings);
                var mongoUser = 
                    collection.
                    Find(u => u.Id.AuthId == userId.AuthId && u.Id.AuthSystemName == userId.AuthSystemName).
                    SingleOrDefault();

                Assert.NotNull(mongoUser);
                string comparisonResult;
                Assert.IsTrue(new TestComparer().Compare(mongoUser, user, out comparisonResult), comparisonResult);
            });
        }

        [Test]
        public void Get_PrevioulsyAddUser_ReturnsValidUser()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = CreateRepository(settings);

                var userId = new UserId("someSystem", "someId");

                var user = new User(userId);

                autoMocker.ClassUnderTest.Insert(user);
                User userAfterInsert;
                var found = autoMocker.ClassUnderTest.TryGet(userId, out userAfterInsert);

                Assert.IsTrue(found);
                Assert.NotNull(userAfterInsert);
                string comparisonResult;
                Assert.IsTrue(
                    new TestComparer().
                    WithMongoDateTimeComparer().
                    Compare(user, userAfterInsert, out comparisonResult), comparisonResult);
            });
        }
    }
}
