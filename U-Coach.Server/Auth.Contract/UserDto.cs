using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public sealed class UserDto
    {
        public string Password { get; set; }
        public string Login { get; set; }

        public UserDto(string login, string password)
        {
            login.NullOrEmptyValidate(nameof(login));
            password.NullOrEmptyValidate(nameof(password));

            Login = login;
            Password = password;
        }
    }
}
