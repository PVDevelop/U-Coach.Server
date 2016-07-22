using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public static class ControllerExtension
    {
        public const string TOKEN_NAME = "access_token";

        public static void SetToken(
            this ApiController controller,
            HttpResponseHeaders headers,
            TokenDto tokenDto)
        {
            var cookie = new CookieHeaderValue(TOKEN_NAME, tokenDto.Token);
            cookie.Expires = tokenDto.Expiration;
            cookie.Domain = controller.Request.RequestUri.Host;
            cookie.Path = "/";
            headers.AddCookies(new[] { cookie });
        }

        public static void DeleteToken(
            this ApiController controller,
            HttpResponseHeaders headers,
            IUtcTimeProvider utcTimeProvider)
        {
            var cookie = new CookieHeaderValue(TOKEN_NAME, string.Empty);
            cookie.Expires = utcTimeProvider.UtcNow.AddDays(-1);
            cookie.Domain = controller.Request.RequestUri.Host;
            cookie.Path = "/";
            headers.AddCookies(new[] { cookie });
        }

        public static string GetToken(
            this ApiController controller)
        {
            var tokenCookie =
                controller.
                Request.
                Headers.
                GetCookies(TOKEN_NAME).
                OrderByDescending(c => c.Expires).
                FirstOrDefault();


            if (tokenCookie == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Token cookie not found"
                });
            }

            var token = tokenCookie[TOKEN_NAME].Value;
            return token;
        }
    }
}
