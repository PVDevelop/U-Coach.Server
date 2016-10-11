using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public interface IUCoachAuthClient
    {
        TokenDto GetToken(string login, string password);
    }
}
