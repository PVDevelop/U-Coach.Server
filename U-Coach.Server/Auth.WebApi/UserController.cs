using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Service;

namespace PVDevelop.UCoach.Server.Auth.WebApi
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException("userService");
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
    }
}
