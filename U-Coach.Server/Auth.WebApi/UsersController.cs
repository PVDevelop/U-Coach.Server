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
            var result = _userService.Create(createUserDto.Login, createUserDto.Password);
            return Content(HttpStatusCode.Created, result);
        }

        [HttpPut]
        [Route(Routes.LOGON_USER)]
        public IHttpActionResult LogonUser([FromUri] string login, [FromBody] string password)
        {
            var result = _userService.Logon(login, password);
            return Ok(result);
        }

        [HttpPut]
        [Route(Routes.VALIDATE_USER_TOKEN)]
        public IHttpActionResult ValidateToken([FromBody] string token)
        {
            _userService.ValidateToken(token);
            return Ok();
        }
    }
}
