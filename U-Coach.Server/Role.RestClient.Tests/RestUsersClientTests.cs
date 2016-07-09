using NUnit.Framework;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.RestClient;
using PVDevelop.UCoach.Server.Role.Service;
using Rhino.Mocks;
using StructureMap.AutoMocking;
using TestComparisonUtilities;
using TestNUnit;
using TestWebApiUtilities;

namespace Role.RestClient.Tests
{
    [TestFixture]
    [Category(CategoryConst.REST)]
    public class RestUsersClientTests
    {
        [Test]
        public void RedirectToFacebookPage_MockUserService_ReturnsExpectedContent()
        {
            var mockUserService = MockRepository.GenerateMock<IUserService>();
            var expectedContent = new OAuthRedirectDto()
            {
                Content = "abc"
            };
            mockUserService.Expect(s => s.RedirectToFacebookPage()).Return(expectedContent);

            var content = TestWebApiHelper.WithServer(
                5100,
                x => x.For<IUserService>().Use(ctx => mockUserService),
                server =>
                {
                    var autoMocker = new RhinoAutoMocker<RestUsersClient>();

                    var webConnString = autoMocker.Get<IConnectionStringProvider>();
                    webConnString.Stub(sp => sp.ConnectionString).Return(server.ConnectionString);

                    var clientFactory = new RestClientFactory(webConnString);
                    autoMocker.Inject(typeof(IRestClientFactory), clientFactory);

                    return autoMocker.ClassUnderTest.RedirectToFacebookPage();
                });

            mockUserService.VerifyAllExpectations();
            string comparison;
            Assert.IsTrue(new TestComparer().Compare(expectedContent, content, out comparison), comparison);
        }
    }
}
