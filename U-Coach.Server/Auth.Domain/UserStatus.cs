using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Domain
{
    /// <summary>
    /// Статус жизненого цикла для пользователя.
    /// Создался пользователь (Unconfirmed) -> подтвердили (Confirmed)
    /// </summary>
    public enum ConfirmationStatus
    {
        Unconfirmed = 0,
        Confirmed = 1
    }
}

