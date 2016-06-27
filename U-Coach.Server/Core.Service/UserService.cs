using System;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public class UserService
    {
        private readonly IUsersClient _users;
        private readonly IMapper _mapper;

        public UserService(
            IUsersClient users,
            IMapper mapper)
        {
            if (users == null)
            {
                throw new ArgumentNullException("users");
            }
            if(mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            _users = users;
            _mapper = mapper;
        }

        public void CreateUser(CreateUserParams userParams)
        {
            var webUserParams = _mapper.Map<Auth.WebDto.CreateUserParams>(userParams);
            _users.Create(webUserParams);
        }
    }
}
