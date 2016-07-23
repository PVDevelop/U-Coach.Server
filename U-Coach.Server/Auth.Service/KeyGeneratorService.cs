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

#warning почему TokenKey, вроде просто Token?
        public string GenerateTokenKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
