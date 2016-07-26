using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class UsersController : ApiController
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            _userService = userService;
        }

        [HttpGet]
        [Route(Routes.USER_INFO)]
        public IHttpActionResult GetUserInfo([FromUri(Name = "token")]string token)
        {
            var tokenId = new TokenId(token);
            var user = _userService.GetUserByToken(tokenId);
            var userInfoDto = new UserInfoDto(user.Id.ToString());

            return Ok(userInfoDto);
        }
    }
}
