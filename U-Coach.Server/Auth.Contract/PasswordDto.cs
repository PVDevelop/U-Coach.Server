using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public sealed class PasswordDto
    {
        public string Password { get; set; }

        public PasswordDto(string password)
        {
            password.NullValidate(nameof(password));

            Password = password;
        }
    }
}
