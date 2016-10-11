using System;

namespace PVDevelop.UCoach.Server.Role.FacebookContract
{
    public interface IFacebookClient
    {
        FacebookTokenDto GetFacebookToken(
            string code,
            string redirectUri);

        FacebookProfileDto GetFacebookProfile(string token);

        Uri GetAuthorizationUri(string redirectUri);
    }
}
