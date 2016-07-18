using System;
using PVDevelop.UCoach.Server.Auth.Domain;
using PVDevelop.UCoach.Server.Auth.Contract;
using PVDevelop.UCoach.Server.Mapper;

namespace PVDevelop.UCoach.Server.Auth.Service
{
    public class ConfirmationService : IConfirmationService
    {
        private readonly IConfirmationFactory _confirmationFactory;
        private readonly IConfirmationRepository _confirmationRepository;
        private readonly IConfirmationProducer _confirmationProducer;

        public ConfirmationService(
            IConfirmationFactory confirmationFactory,
            IConfirmationRepository confirmationRepository,
            IConfirmationProducer confirmationProducer)
        {
            if (confirmationFactory == null)
            {
                throw new ArgumentNullException(nameof(confirmationFactory));
            }
            if(confirmationRepository == null)
            {
                throw new ArgumentNullException(nameof(confirmationRepository));
            }
            if(confirmationProducer == null)
            {
                throw new ArgumentNullException(nameof(confirmationProducer));
            }

            _confirmationFactory = confirmationFactory;
            _confirmationRepository = confirmationRepository;
            _confirmationProducer = confirmationProducer;
        }

        public void CreateConfirmation(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var confirmation = _confirmationFactory.CreateConfirmation(userId);
            _confirmationRepository.Obtain(confirmation);

            var confirmationParams = MapperHelper.Map<Confirmation, ConfirmationKeyParams>(confirmation); //?
            _confirmationProducer.Produce(confirmationParams);
        }

        public bool Confirm(string userId, string key)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var confirmation = _confirmationRepository.FindByConfirmation(userId, key);

            if (confirmation != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
