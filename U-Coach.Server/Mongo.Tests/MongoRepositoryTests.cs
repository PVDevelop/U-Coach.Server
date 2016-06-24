using NUnit.Framework;
using Rhino.Mocks;
using TestMongoUtilities;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        [Test]
        public void Find_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestMongoObj>());

            var rep = new MongoRepository<TestMongoObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
            try
            {
                rep.Find(t => true);
            }
            catch { }

            mock.VerifyAllExpectations();
        }
        [Test]

        public void Insert_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestMongoObj>());

            var rep = new MongoRepository<TestMongoObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
            try
            {
                rep.Insert(new TestMongoObj());
            }
            catch { }

            mock.VerifyAllExpectations();
        }

        [Test]
        public void Replace_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestMongoObj>());

            var rep = new MongoRepository<TestMongoObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
            try
            {
                rep.ReplaceOne(o=>true, new TestMongoObj());
            }
            catch { }

            mock.VerifyAllExpectations();
        }
    }
}
