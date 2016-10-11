using System;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public class KeyGeneratorService : IKeyGeneratorService
    {
        public string GenerateConfirmationKey()
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateTokenKey()
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateUserId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
