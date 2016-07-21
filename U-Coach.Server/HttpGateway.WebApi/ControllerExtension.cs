using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public static class ControllerExtension
    {
        public static void SetToken(
            this ApiController controller,
            HttpResponseHeaders headers,
            TokenDto tokenDto)
        {
            var cookie = new CookieHeaderValue("access_token", tokenDto.Token);
            cookie.Expires = tokenDto.Expiration;
            cookie.Domain = controller.Request.RequestUri.Host;
            cookie.Path = "/";
            headers.AddCookies(new[] { cookie });
        }

        public static void DeleteToken(
            this ApiController controller)
        {

        }
    }
}
