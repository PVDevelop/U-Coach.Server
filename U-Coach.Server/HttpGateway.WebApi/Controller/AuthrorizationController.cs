using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi.Controller
{
    public class AuthrorizationController : ApiController
    {
        private readonly IUtcTimeProvider _utcTimeProvider;

        public AuthrorizationController(IUtcTimeProvider utcTimeProvider)
        {
            if(utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }

            _utcTimeProvider = utcTimeProvider;
        }

        [HttpPost]
        [Route(Routes.LOGOUT)]
        public IHttpActionResult Logout()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            this.DeleteToken(response.Headers, _utcTimeProvider);
            return ResponseMessage(response);
        }
    }
}
