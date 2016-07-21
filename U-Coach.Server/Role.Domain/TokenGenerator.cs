using System;
using System.Text;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class TokenGenerator : ITokenGenerator
    {
        public string Generate(
            User user, 
            AuthSystemToken authToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            var decodedToken = string.Format(
                "{0}.{1}.{2}", 
                user.Id.AuthSystemName,
                user.Id.AuthId, 
                authToken.Token);

            var bytes = Encoding.UTF8.GetBytes(decodedToken);
            return Convert.ToBase64String(bytes);
        }
    }
}
