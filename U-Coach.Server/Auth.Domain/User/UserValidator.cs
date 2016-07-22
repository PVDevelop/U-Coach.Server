using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    public class UserValidator : IUserValidator
    {
        public void ValidateLogin(string login)
        {
            if (String.IsNullOrWhiteSpace(login))
            {
                throw new ValidateLoginException();
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(login, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                throw new ValidateLoginException();
            }
        }

        public void ValidatePassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new ValidatePasswordException();
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{7,15}$", RegexOptions.IgnoreCase))
            {
                throw new ValidatePasswordException();
            }
        }
    }
}
