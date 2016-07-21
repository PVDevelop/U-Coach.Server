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
            this.DeleteToken();
            return Ok();
        }
    }
}
