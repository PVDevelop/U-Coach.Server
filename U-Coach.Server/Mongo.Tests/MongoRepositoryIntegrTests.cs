using MongoDB.Driver;
using NUnit;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Linq;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    [Integration]
    class MongoRepositoryIntegrTests
    {
        private class TestObj : IHaveId
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void Insert_ValidObject_SavesToDb()
        {
            var settings = MongoHelper.CreateSettings();

            var testObj = new TestObj()
            {
                Name = "SomeName"
            };

            MongoHelper.WithDb(settings, db =>
            {
                var rep = new MongoRepository<TestObj>(settings);
                rep.Insert("some_coll", testObj);

                var coll =
                    new MongoClient(settings.ConnectionString).
                    GetDatabase(settings.DatabaseName).
                    GetCollection<TestObj>("some_coll");

                var foundObj = coll.Find(o => o.Id == testObj.Id && o.Name == testObj.Name).FirstOrDefault();
                Assert.NotNull(foundObj, "Объект не был сохранен в MongoDb");
            });
        }
    }
}
