using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Auth.WebClient;
using PVDevelop.UCoach.Server.Core.Domain;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public class SportsmanConfirmationService
    {
        private readonly IUsersClient _users;
        private readonly IMapper _mapper;
        private readonly ISportsmanConfirmationRepository _userRepository;
        private readonly ISportsmanConfirmationProducer _userConfirmationProducer;

        public SportsmanConfirmationService(
            IUsersClient users,
            ISportsmanConfirmationRepository userRepository,
            IMapper mapper,
            ISportsmanConfirmationProducer userConfirmationProducer)
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

        public void CreateUser(CreateSportsmanConfirmationParams userParams)
        {
            var webUserParams = _mapper.Map<Auth.WebDto.CreateUserParams>(userParams);
            var authId = _users.Create(webUserParams);

            var confirmation = SportsmanConfirmationFactory.CreateSportsmanConfirmation(authId, userParams.ConfirmationKey);
            _userRepository.Insert(confirmation);

            var producerParams = _mapper.Map<ProduceConfirmationKeyParams>(userParams);
            _userConfirmationProducer.Produce(producerParams);
        }
    }
}
