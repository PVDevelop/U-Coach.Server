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
        public string Url4Confirmation { get; set; }

        public UserDto(string login, string password, string url4Confirmation)
        {
            login.NullOrEmptyValidate(nameof(login));
            password.NullOrEmptyValidate(nameof(password));
            url4Confirmation.NullOrEmptyValidate(nameof(url4Confirmation));

            Login = login;
            Password = password;
            Url4Confirmation = url4Confirmation;
        }
    }
}
