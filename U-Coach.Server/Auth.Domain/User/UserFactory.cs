using System;
using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
#warning грохаем фабрику и переносим логику в агрегат
    public class UserFactory : IUserFactory
    {
        private readonly IUtcTimeProvider _utcTimeProvider;
        private readonly IUserValidator _userValidator;

        public UserFactory(
            IUtcTimeProvider utcTimeProvider,
            IUserValidator userValidator)
        {
            if(utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }
            if (userValidator == null)
            {
                throw new ArgumentNullException(nameof(userValidator));
            }

            _utcTimeProvider = utcTimeProvider;
            _userValidator = userValidator;
        }

        public User CreateUser(string login, string password)
        {
            _userValidator.ValidateLogin(login);
            _userValidator.ValidatePassword(password);

            var user = new User()
            {
                Login = login,
                CreationTime = _utcTimeProvider.UtcNow,
                Status = UserStatus.Unconfirm
            };

            user.SetPassword(password);

            return user;
        }
    }
}
