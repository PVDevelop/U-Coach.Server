using System;
using PVDevelop.UCoach.Server.Configuration;
using PVDevelop.UCoach.Server.RestClient;
using PVDevelop.UCoach.Server.Role.Contract;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public class UserService : IUserService
    {
        private static IRestClientFactory GetFacebookGraphClientFactory()
        {
            var connectionStringProvider = new SimpleConnectionStringProvider("https://graph.facebook.com");
            return new RestClientFactory(connectionStringProvider);
        }

        private readonly IUserRepository _repository;
        private readonly IUserFactory _factory;
        private readonly ISettingsProvider<IFacebookOAuthSettings> _settingsProvider;

        public UserService(
            IUserFactory factory,
            IUserRepository repository,
            ISettingsProvider<IFacebookOAuthSettings> settingsProvider)
        {
            if(factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }
            if(settingsProvider == null)
            {
                throw new ArgumentNullException(nameof(settingsProvider));
            }

            _factory = factory;
            _repository = repository;
            _settingsProvider = settingsProvider;
        }

        public OAuthRedirectDto RedirectToFacebookPage()
        {
            var settings = _settingsProvider.Settings;
            var redirectUri = string.Format(
               "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope={2}",
               settings.ClientId,
               settings.UriRedirectToCode,
               "public_profile");
            return new OAuthRedirectDto()
            {
                RedirectUri = redirectUri
            };
        }

        public void ApplyFacebookCode(FacebookCodeDto facebookCodeDto)
        {
            if (facebookCodeDto == null)
            {
                throw new ArgumentNullException("facebookCodeDto");
            }

            var tokenDto = GetFacebookToken(facebookCodeDto.Code);
            var profileDto = GetFacebookProfile(tokenDto.Token);
            RegisterFacebookUser(profileDto);
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

        private FacebookTokenDto GetFacebookToken(string code)
        {
            var settings = _settingsProvider.Settings;
            return
                GetFacebookGraphClientFactory().
                CreateGet("v2.5/oauth/access_token").
                AddParameter("client_id", settings.ClientId).
                AddParameter("redirect_uri", settings.UriRedirectToCode).
                AddParameter("client_secret", settings.ClientSecret).
                AddParameter("code", code).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookTokenDto>();
        }

        private FacebookProfileDto GetFacebookProfile(string token)
        {
            return
                GetFacebookGraphClientFactory().
                CreateGet("me").
                AddParameter("access_token", token).
                Execute().
                CheckGetResult().
                GetJsonContent<FacebookProfileDto>();
        }
    }
}
