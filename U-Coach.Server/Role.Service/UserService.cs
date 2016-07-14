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

        public void RegisterFacebookUser(FacebookConnectionDto facebookConnection)
        {
            var userId = new UserId(AuthSystemHelper.FACEBOOK_SYSTEM_NAME, facebookConnection.Id);
            var token = new AuthToken(facebookConnection.Token, facebookConnection.TokenExpiration);

            IUser user;
            if (_repository.TryGet(userId, out user))
            {
                // обновляем имеюищегося пользователя
                user.SetToken(token);
                _repository.Update(user);
            }
            else
            {
                // создаем нового пользователя
                user = _factory.CreateUser(userId);
                user.SetToken(token);
                _repository.Insert(user);
            }
        }
    }
}
