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
        public void Ctor_MockVersionValidator_CallsValidate()
        {
            var mock = MockRepository.GenerateMock<IMongoCollectionVersionValidator>();
            mock.Expect(m => m.Validate<TestObj>());

            var rep = new MongoRepository<TestObj>(MockRepository.GenerateStub<IMongoConnectionSettings>(), mock);

            mock.VerifyAllExpectations();
        }
    }
}
