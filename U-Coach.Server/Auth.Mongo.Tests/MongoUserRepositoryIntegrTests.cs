using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Exceptions;
using StructureMap.AutoMocking;
using TestMongoUtilities;

namespace Auth.Mongo.Tests
{
    [TestFixture]
    public class MongoUserRepositoryIntegrTests
    {
        [Test]
        public void Insert_CollectionNotInitialized_ThrowsException()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var validator = new MongoCollectionVersionValidatorByClassAttribute(settings);
                autoMocker.Inject(typeof(IMongoCollectionVersionValidator), validator);

                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Insert(new User("login")));
            });
        }

        [Test]
        public void Update_CollectionNotInitialized_ThrowsException()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var validator = new MongoCollectionVersionValidatorByClassAttribute(settings);
                autoMocker.Inject(typeof(IMongoCollectionVersionValidator), validator);

                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Update(new User("login")));
            });
        }
    }
}
