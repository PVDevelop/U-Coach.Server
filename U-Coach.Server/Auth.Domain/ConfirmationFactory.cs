using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using PVDevelop.UCoach.Server.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public class ConfirmationFactory : IConfirmationFactory
    {
        private readonly IUtcTimeProvider _utcTimeProvider;

        public ConfirmationFactory(IUtcTimeProvider utcTimeProvider)
        {
            if (utcTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(utcTimeProvider));
            }
            _utcTimeProvider = utcTimeProvider;
        }

        public Confirmation CreateConfirmation(string userID)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                throw new LoginNotSetException();
            }

            var confirmation = new Confirmation(userId: userID, key: Guid.NewGuid().ToString(), creationTime: _utcTimeProvider.UtcNow);

            return confirmation;
        }
    }
}
