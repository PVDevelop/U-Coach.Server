using System.Net;
using System.Net.Http;
using System.Web.Http;
using PVDevelop.UCoach.Server.HttpGateway.Contract;

namespace PVDevelop.UCoach.Server.HttpGateway.WebApi
{
    public class AuthrorizationController : ApiController
    {
        [HttpPost]
        [Route(Routes.LOGOUT)]
        public IHttpActionResult Logout()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            this.DeleteToken(response.Headers);
            return ResponseMessage(response);
        }
    }
}
