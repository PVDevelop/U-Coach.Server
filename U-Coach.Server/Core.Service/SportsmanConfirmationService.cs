﻿using System;
using PVDevelop.UCoach.Server.Mapper;
using PVDevelop.UCoach.Server.Core.Domain;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public class SportsmanConfirmationService : ISportsmanConfirmationService
    {
        private readonly IUsersClient _users;
        private readonly ISportsmanConfirmationRepository _userRepository;
        private readonly ISportsmanConfirmationProducer _userConfirmationProducer;

        public SportsmanConfirmationService(
            IUsersClient users,
            ISportsmanConfirmationRepository userRepository,
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
            if(userConfirmationProducer == null)
            {
                throw new ArgumentNullException("userConfirmationProducer");
            }

            _users = users;
            _userRepository = userRepository;
            _userConfirmationProducer = userConfirmationProducer;
        }

        public void CreateConfirmation(CreateSportsmanConfirmationParams userParams)
        {
            var webUserParams = MapperHelper.Map<CreateSportsmanConfirmationParams, CreateUserDto>(userParams);
            var authId = _users.Create(webUserParams);

            var confirmation = SportsmanConfirmationFactory.CreateSportsmanConfirmation(authId, userParams.ConfirmationKey);
            _userRepository.Insert(confirmation);

            var producerParams = MapperHelper.Map<CreateSportsmanConfirmationParams, ProduceConfirmationKeyParams>(userParams);
            _userConfirmationProducer.Produce(producerParams);
        }
    }
}
