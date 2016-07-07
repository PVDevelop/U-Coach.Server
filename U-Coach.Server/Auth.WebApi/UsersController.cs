using System;
using System.Net;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Service;

namespace PVDevelop.UCoach.Server.Auth.WebApi
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            _userService = userService;
        }

        [HttpPost]
        [Route(Routes.CREATE_USER)]
        public IHttpActionResult CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var result = _userService.Create(createUserDto);
            return Content(HttpStatusCode.Created, result);
        }

        [HttpPost]
        [Route(Routes.LOGON_USER)]
        public IHttpActionResult LogonUser(string login, [FromBody] string password)
        {
            var dto = new LogonUserDto()
            {
                Login = login,
                Password = password
            };

            var result = _userService.Logon(dto);
            return Content(HttpStatusCode.Created, result);
        }

        [HttpPost]
        [Route(Routes.VALIDATE_USER_TOKEN)]
        public IHttpActionResult ValidateToken(string login, [FromBody] string token)
        {
            var dto = new ValidateTokenDto()
            {
                Login = login,
                Token = token
            };

            _userService.ValidateToken(dto);
            return StatusCode(HttpStatusCode.Created);
        }
    }
}
