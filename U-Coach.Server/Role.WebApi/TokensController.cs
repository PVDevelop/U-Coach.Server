using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.WebApi
{
    public class TokensController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenValidationService _tokenValidationService;

        public TokensController(
            IUserService userService,
            ITokenValidationService tokenValidationService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if(tokenValidationService == null)
            {
                throw new ArgumentNullException(nameof(tokenValidationService));
            }

            _userService = userService;
            _tokenValidationService = tokenValidationService;
        }

        [HttpDelete]
        [Route(Routes.DELETE_TOKEN)]
        public IHttpActionResult DeleteToken(
            [FromUri(Name = "token")] string token)
        {
            var tokenId = new TokenId(token);
            _userService.DeleteToken(tokenId);
            return Ok();
        }
    }
}
