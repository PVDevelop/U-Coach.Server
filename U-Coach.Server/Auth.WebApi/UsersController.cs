using System;
using System.Net;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Domain;

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
        public IHttpActionResult CreateUser([FromBody] UserDto user)
        {
            _userService.CreateUser(user.Login, user.Password, user.Url4Confirmation);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.LOGON_USER)]
        public IHttpActionResult LogonUser([FromUri] string login, [FromBody] PasswordDto password)
        {
            var result = _userService.Logon(login, password.Password);
            return Ok(result);
        }

        [HttpPut]
        [Route(Routes.VALIDATE_USER_TOKEN)]
        public IHttpActionResult ValidateToken([FromBody] TokenDto token)
        {
            _userService.ValidateToken(token.Key);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.CONFIRM_USER)]
        public IHttpActionResult ConfirmUser([FromBody] ConfirmationDto confirmation)
        {
            _userService.Confirm(confirmation.Key);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.RESEND_CONFIRM)]
        public IHttpActionResult ResendConfirmation([FromUri] string login, [FromBody] ConfirmUrlDTO url)
        {
            _userService.ResendConfirmation(login, url.Url);
            return Ok();
        }
    }
}
