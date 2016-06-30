using System;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Auth.Service;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    /// <summary>
    /// Реализация провайдера клиентов в текущем .Net домене приложения
    /// </summary>
    public class CurrentDomainUsersClient : IUsersClient
    {
        private readonly IUserService _userService;

        public CurrentDomainUsersClient(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException("userService");
            }
            _userService = userService;
        }

        public string Create(CreateUserDto userDto)
        {
            var userParams = MapperHelper.Map<CreateUserDto, CreateUserParams>(userDto);
            return _userService.Create(userParams);
        }
    }
}
