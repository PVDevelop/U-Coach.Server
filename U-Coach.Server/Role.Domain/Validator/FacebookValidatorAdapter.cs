using System;
using PVDevelop.UCoach.Server.Role.FacebookContract;

namespace PVDevelop.UCoach.Server.Role.Domain.Validator
{
    public class FacebookValidatorAdapter : IAuthTokenValidator
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookValidatorAdapter(IFacebookClient facebookClient)
        {
            if(facebookClient == null)
            {
                throw new ArgumentNullException(nameof(facebookClient));
            }
            _facebookClient = facebookClient;
        }

        public void Validate(AuthSystemToken authSystemToken)
        {
            // получаем профиль и если не падаем, то валидация прошла успешно!
            _facebookClient.GetFacebookProfile(authSystemToken.Token);
        }
    }
}
