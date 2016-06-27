using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Auth.WebClient;

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

        public void CreateUser(CreateUCoachUserParams userParams)
        {
            var webUserParams = _mapper.Map<Auth.WebDto.CreateUserParams>(userParams);
            _users.Create(webUserParams);
        }
    }
}
