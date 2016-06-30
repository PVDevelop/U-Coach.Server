using System;
using MongoDB.Driver;
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
        private static void CheckMongoUserEquals(User user, MongoUser mongoUser)
        {
#warning прикрутить сравнение по reflection
            Assert.NotNull(mongoUser);
            Assert.AreEqual(MongoUser.VERSION, mongoUser.Version);
            Assert.AreEqual(user.Id, mongoUser.Id);
            Assert.AreEqual(user.Login, mongoUser.Login);
            Assert.AreEqual(user.Password, mongoUser.Password);
            Assert.AreEqual(user.IsLoggedIn, mongoUser.IsLoggedIn);
            Assert.That(mongoUser.LastAuthenticationTime, Is.EqualTo(user.LastAuthenticationTime).Within(1).Milliseconds);
            Assert.That(mongoUser.CreationTime, Is.EqualTo(user.CreationTime).Within(1).Milliseconds);
        }

        [Test]
        public void Insert_CollectionNotInitialized_ThrowsException()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var validator = new MongoCollectionVersionValidatorByClassAttribute(settings);
                autoMocker.Inject(typeof(IMongoCollectionVersionValidator), validator);

                var user = UserFactory.CreateUser("login");
                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Insert(user));
            });
        }

        [Test]
        public void Insert_NewUser_InsertsUser()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db => 
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var mongoRepository = new MongoRepository<MongoUser>(settings);
                autoMocker.Inject(typeof(IMongoRepository<MongoUser>), mongoRepository);

                var user = UserFactory.CreateUser("new_user");
                user.SetPassword("pwd");
                user.Logon("pwd");

                autoMocker.ClassUnderTest.Insert(user);

                var coll = MongoHelper.GetCollection<MongoUser>(settings);
                var mongoUser = coll.Find(u => u.Id == user.Id).SingleOrDefault();

                CheckMongoUserEquals(user, mongoUser);
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

                var user = UserFactory.CreateUser("login");
                Assert.Throws<MongoCollectionNotInitializedException>(() => autoMocker.ClassUnderTest.Update(user));
            });
        }

        [Test]
        public void Update_ExistingUser_UpdatesUser()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var mongoRepository = new MongoRepository<MongoUser>(settings);
                autoMocker.Inject(typeof(IMongoRepository<MongoUser>), mongoRepository);

                var user = UserFactory.CreateUser("new_user");
                user.SetPassword("pwd");

                var mongoUser = new MongoUser()
                {
                    Id = user.Id,
                    CreationTime = user.CreationTime,
                    IsLoggedIn = false,
                    Password = user.Password,
                };

                var coll = MongoHelper.GetCollection<MongoUser>(settings);
                coll.InsertOne(mongoUser);

                user.Logon("pwd");
                autoMocker.ClassUnderTest.Update(user);

                var updatedUser = coll.Find(u => u.Id == user.Id).SingleOrDefault();

                CheckMongoUserEquals(user, updatedUser);
            });
        }

        [Test]
        public void FindByLogin_ExistingUser_FindsUser()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var autoMocker = new RhinoAutoMocker<MongoUserRepository>();

                var mongoRepository = new MongoRepository<MongoUser>(settings);
                autoMocker.Inject(typeof(IMongoRepository<MongoUser>), mongoRepository);

                var mongoUser = new MongoUser()
                {
                    Id = Guid.NewGuid(),
                    Login = "login"
                };

                var coll = MongoHelper.GetCollection<MongoUser>(settings);
                coll.InsertOne(mongoUser);

                var foundUser = autoMocker.ClassUnderTest.FindByLogin(mongoUser.Login);

                Assert.NotNull(foundUser);
                Assert.AreEqual(mongoUser.Id, foundUser.Id);
            });
        }
    }
}
