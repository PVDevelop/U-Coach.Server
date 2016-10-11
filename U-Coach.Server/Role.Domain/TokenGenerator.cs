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

            var decodedToken = Guid.NewGuid().ToString();

            var bytes = Encoding.UTF8.GetBytes(decodedToken);
            return Convert.ToBase64String(bytes);
        }
    }
}
