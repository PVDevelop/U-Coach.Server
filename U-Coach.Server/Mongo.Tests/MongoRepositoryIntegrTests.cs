using MongoDB.Driver;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo;
using Rhino.Mocks;
using System.Linq;
using TestMongoUtilities;
using TestNUnit;

namespace Auth.Domain.Tests
{
    [TestFixture]
    [Integration]
    public class MongoRepositoryIntegrTests
    {
        [Test]
        public void Insert_ValidObject_SavesToDb()
        {
            var settings = TestMongoHelper.CreateSettings();
            TestMongoHelper.WithDb(settings, db =>
            {
                var rep = new MongoRepository<TestMongoObj>(settings);

                var testObj = new TestMongoObj()
                {
                    Name = "SomeName"
                };
                rep.Insert(testObj);

                var coll =
                    new MongoClient(settings.ConnectionString).
                    GetDatabase(settings.DatabaseName).
                    GetCollection<TestMongoObj>(MongoHelper.GetCollectionName<TestMongoObj>());

                var foundObj = coll.Find(o => o.Id == testObj.Id && o.Name == testObj.Name).FirstOrDefault();
                Assert.NotNull(foundObj, "Объект не был сохранен в MongoDb");
            });
        }
    }
}
