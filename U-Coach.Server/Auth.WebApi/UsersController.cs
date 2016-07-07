using System;
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
        public string CreateUser([FromBody] CreateUserDto createUserDto)
        {
            return _userService.Create(createUserDto);
        }

        [HttpPost]
        [Route(Routes.LOGON_USER)]
        public string LogonUser(string login, [FromBody] string password)
        {
            var dto = new LogonUserDto()
            {
                Login = login,
                Password = password
            };

            return _userService.Logon(dto);
        }

        [HttpPost]
        [Route(Routes.VALIDATE_USER_TOKEN)]
        public void ValidateToken(string login, [FromBody] string token)
        {
            var dto = new ValidateTokenDto()
            {
                Login = login,
                Token = token
            };

            _userService.ValidateToken(dto);
        }
    }
}
