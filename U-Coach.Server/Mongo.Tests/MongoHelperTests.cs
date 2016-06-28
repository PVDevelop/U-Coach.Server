using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PVDevelop.UCoach.Server.Mongo.Tests
{
    [TestFixture]
    public class MongoHelperTests
    {
        [MongoDataVersion(3)]
        [MongoCollection("SomeType")]
        private class SomeType
        {
            [MongoIndexName("index1")]
            public string MyPropWithAttr { get; set; }

            [MongoIndexName("index2")]
            public string MyPropWithAttr2 { get; set; }

            public string MyPropWithoutAttr { get; set; }
        }


        [Test]
        public void GetCollectionName_TypeWithoutCollectionAttribute_ReturnsTypesName()
        {
            var name = MongoHelper.GetCollectionName<object>();
            Assert.AreEqual(typeof(object).Name, name);
        }

        [Test]
        public void GetCollectionName_TypeWithCollectionAttribute_ReturnsNameFromAttribute()
        {
            var name = MongoHelper.GetCollectionName<SomeType>();
            Assert.AreEqual("SomeType", name);
        }

        [Test]
        public void GetDataVersion_TypeWithDataVersionAttribute_ReturnsVersionFromAttribute()
        {
            var version = MongoHelper.GetDataVersion<SomeType>();
            Assert.AreEqual(3, version);
        }

        [Test]
        public void GetIndexName_PropertyWithIndexAttribute_ReturnsNameFromAttribute()
        {
            var name = MongoHelper.GetIndexName<SomeType>(nameof(SomeType.MyPropWithAttr));
            Assert.AreEqual("index1", name);
        }

        [Test]
        public void GetIndexName_PropertyWithoutIndexAttribute_ReturnsPropertyName()
        {
            var name = MongoHelper.GetIndexName<SomeType>(nameof(SomeType.MyPropWithoutAttr));
            Assert.AreEqual(nameof(SomeType.MyPropWithoutAttr), name);
        }

        [Test]
        public void GetCompoundIndexName_PropertiesWithIndexAttributes_ReturnsDotSplittedName()
        {
            var name = MongoHelper.GetCompoundIndexName<SomeType>(nameof(SomeType.MyPropWithAttr), nameof(SomeType.MyPropWithAttr2));
            Assert.AreEqual(string.Format("{0}.{1}", "index1", "index2"), name);
        }
    }
}
