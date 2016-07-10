using System;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserFactory _factory;

        public UserService(
            IUserFactory factory,
            IUserRepository repository)
        {
            if(factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _factory = factory;
            _repository = repository;
        }

        public void RegisterFacebookUser(FacebookProfileDto facebookUserDto)
        {
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, facebookUserDto.Id);
            if (!_repository.Contains(userId))
            {
                var user = _factory.CreateUser(userId);
                _repository.Insert(user);
            }
        }
    }
}
