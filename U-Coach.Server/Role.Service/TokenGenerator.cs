using System;
using System.Text;
using PVDevelop.UCoach.Server.Role.Domain;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public class TokenGenerator : ITokenGenerator
    {
        public string Generate(User user, AuthTokenParams tokenParams)
        {
            if(tokenParams == null)
            {
                throw new ArgumentNullException(nameof(tokenParams));
            }

            var decodedToken = string.Format(
                "{0}.{1}.{2}", 
                tokenParams.AuthSystemName, 
                tokenParams.AuthUserId, 
                tokenParams.Token);

            var bytes = Encoding.UTF8.GetBytes(decodedToken);
            return Convert.ToBase64String(bytes);
        }
    }
}
