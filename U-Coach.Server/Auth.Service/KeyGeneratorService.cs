using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Service
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
