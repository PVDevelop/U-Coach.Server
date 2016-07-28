using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Статус жизненого цикла для пользователя.
    /// Создался пользователь (Unspecified) -> выслали подтверждение на почту (Unconfirm) -> подтвердили (Confirm)
    /// </summary>
    public enum UserStatus
    {
        Unspecified = 0,
        Unconfirmed = 1,
        Confirmed = 2
    }
}

