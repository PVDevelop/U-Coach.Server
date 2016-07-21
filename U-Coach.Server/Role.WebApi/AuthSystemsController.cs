using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class AuthSystemsController : ApiController
    {
        private readonly IUserService _userService;

        public AuthSystemsController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            _userService = userService;
        }

        [HttpPut]
        [Route(Routes.REGISTER_USER)]
        public IHttpActionResult RegisterUser(
            [FromUri(Name = "system")] string authSystemName,
            [FromUri(Name = "id")] string authUserId,
            [FromBody]AuthUserRegisterDto authRegisterDto)
        {
            var userId = new UserId(authSystemName, authUserId);
            var authToken = new AuthSystemToken(authRegisterDto.Token, authRegisterDto.Expiration);
            var token = _userService.RegisterUserToken(userId, authToken);

            var tokenDto = new TokenDto(token.Id.Token, token.Expiration);
            return Ok(tokenDto);
        }
    }
}
