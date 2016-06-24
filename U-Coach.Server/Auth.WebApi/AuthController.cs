using PVDevelop.UCoach.Server.Auth.Service;
using System;
using System.Web.Http;

namespace PVDevelop.UCoach.Server.Auth.WebApi
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        [HttpGet]
        public void Logon()
        {
            var p = new LogonUserParams()
            {
                Login = "root",
                Password = "root"
            };

            _userService.Logon(p);
        }
    }
}
