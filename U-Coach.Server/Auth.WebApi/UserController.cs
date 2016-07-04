using System;
using System.Web.Http;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Service;

namespace PVDevelop.UCoach.Server.Auth.WebApi
{
    [RoutePrefix("user")]
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
        public void CreateUser(CreateUserDto createUserDto)
        {
            _userService.Create(createUserDto);
        }
    }
}
