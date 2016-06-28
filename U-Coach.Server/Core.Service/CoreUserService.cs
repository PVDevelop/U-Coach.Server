using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public class CoreUserService
    {
        private readonly IUsersClient _users;
        private readonly IMapper _mapper;
        private readonly ICoreUserRepository _userRepository;
        private readonly ICoreUserConfirmationProducer _userConfirmationProducer;

        public CoreUserService(
            IUsersClient users,
            ICoreUserRepository userRepository,
            IMapper mapper,
            ICoreUserConfirmationProducer userConfirmationProducer)
        {
            if (users == null)
            {
                throw new ArgumentNullException("users");
            }
            if(userRepository == null)
            {
                throw new ArgumentNullException("userRepository");
            }
            if(mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }
            if(userConfirmationProducer == null)
            {
                throw new ArgumentNullException("userConfirmationProducer");
            }

            _users = users;
            _mapper = mapper;
            _userRepository = userRepository;
            _userConfirmationProducer = userConfirmationProducer;
        }

        public void CreateUser(CreateUCoachUserParams userParams)
        {
            var webUserParams = _mapper.Map<Auth.WebDto.CreateUserParams>(userParams);
            var authId = _users.Create(webUserParams);

            var coreUser = CoreUserFactory.CreateUCoachUser(authId, userParams.ConfirmationKey);
            _userRepository.Insert(coreUser);

            var producerParams = _mapper.Map<ProduceConfirmationKeyParams>(userParams);
            _userConfirmationProducer.Produce(producerParams);
        }
    }
}
