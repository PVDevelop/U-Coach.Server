using System;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
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

        public OAuthRedirectDto RedirectToFacebookPage()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://www.facebook.com");
            var factory = new RestClientFactory(connectionStringProvider);
            var content =
                factory.
                CreatePost("dialog/oauth").
                AddParameter("client_id", "1034374253265538").
                AddParameter("redirect_uri", "http://localhost:7788/Auth/FacebookCode").
                AddParameter("scope", "public_profile").
                Execute().
                CheckPostResult().
                GetContent();

            return new OAuthRedirectDto() { Content = content };
        }

        public void RegisterFacebookUser(RegisterFacebookUserDto facebookUserDto)
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
