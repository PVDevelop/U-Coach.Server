using PVDevelop.UCoach.Server.Auth.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
#warning логика агрегата
    public class UserValidator : IUserValidator
    {
        public void ValidateLogin(string login)
        {
#warning в регулярку
            if (String.IsNullOrWhiteSpace(login))
            {
                throw new ValidateLoginException();
            }

#warning если здесь будет почта, то переделать на регулярку для почты
            if (!System.Text.RegularExpressions.Regex.IsMatch(login, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                throw new ValidateLoginException();
            }
        }

        public void ValidatePassword(string password)
        {
#warning в регулярку
            if (String.IsNullOrWhiteSpace(password))
            {
                throw new ValidatePasswordException();
            }

#warning не понятно нифига, что за выражение, нужно где-то в комментах подробно описать, желательно с примером
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{7,15}$", RegexOptions.IgnoreCase))
            {
                throw new ValidatePasswordException();
            }
        }
    }
}
