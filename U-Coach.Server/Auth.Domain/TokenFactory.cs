using PVDevelop.UCoach.Server.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public sealed class TokenFactory: 
        ITokenFactory
    {
        private readonly IUtcTimeProvider _utcTimeProvider;

        public TokenFactory(IUtcTimeProvider utcTimeProvider)
        {
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }
            _utcTimeProvider = utcTimeProvider;
        }

        public Token CreateToken(string userID, string key)
        {
            return new Token()
            {
                UserId = userID,
                ExpiryDate = _utcTimeProvider.UtcNow.Date.AddDays(60).AddHours(2),
                Key = key
            };
        }
    }
}
