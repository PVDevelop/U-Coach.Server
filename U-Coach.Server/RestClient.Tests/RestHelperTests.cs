using System;
using NUnit.Framework;
using PVDevelop.UCoach.Server.RestClient;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestHelperTests
    {
        [Test]
        public void FormatUri_ValidFormat_ReturnsExpectedValue()
        {
            string uri = "api/users/{id}/parameters/{property}/update";
            var id = Guid.NewGuid();
            var property = "some_field";

            var result = RestHelper.FormatUri(uri, id, property);
            var expectedUri = string.Format(
                "api/users/{0}/parameters/{1}/update",
                id,
                property);
            Assert.AreEqual(expectedUri, result);
        }
    }
}
