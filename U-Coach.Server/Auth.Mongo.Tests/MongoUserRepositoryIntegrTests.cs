using System;
using System.Linq;
using Auth.Domain.Tests;
using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Mongo;
using PVDevelop.UCoach.Server.Mongo;
using PVDevelop.UCoach.Server.Mongo.Exceptions;
using StructureMap.AutoMocking;
using TestComparisonUtilities;
using TestMongoUtilities;

namespace Auth.Mongo.Tests
{
    [TestFixture]
    public class MongoUserRepositoryIntegrTests
    {
        private static void CheckMongoUserEquals(User user, MongoUser mongoUser)
        {
            var builder = 
                new TestComparer().
                WithMongoDateTimeComparer().
                IgnoreProperty<MongoUser>(mu => mu.Version);

            string result;
            Assert.IsTrue(builder.Compare(user, mongoUser, out result), result);
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

                var user = new TestUserFactory().CreateUser("login", "pwd");
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

                var user = new TestUserFactory().CreateUser("new_user", "pwd");
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

                var user = new TestUserFactory().CreateUser("login", "pwd");
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

                var user = new TestUserFactory().CreateUser("new_user", "pwd");

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
