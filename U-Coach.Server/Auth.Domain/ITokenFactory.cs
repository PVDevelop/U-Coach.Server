using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public interface ITokenFactory
    {
        Token CreateToken(string userID, string key);
    }
}
