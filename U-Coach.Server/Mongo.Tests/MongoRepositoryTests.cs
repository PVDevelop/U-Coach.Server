using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PVDevelop.UCoach.Server.Mongo;
using Rhino.Mocks;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        [Test]
        public void Find_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestObj>());

            var rep = new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
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
            mock.Expect(m => m.Validate<TestObj>());

            var rep = new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
            try
            {
                rep.Insert(new TestObj());
            }
            catch { }

            mock.VerifyAllExpectations();
        }

        [Test]
        public void Replace_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestObj>());

            var rep = new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);
            try
            {
                rep.ReplaceOne(o=>true, new TestObj());
            }
            catch { }

            mock.VerifyAllExpectations();
        }
    }
}
