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
#warning UserConfirmationStatus. Enum разве не int по умолчанию?
    public enum UserStatus : int 
    {
#warning Unspecified как-то слишком обще, давай что-то типа New. И вообще, он вроде нигде не используется.
        Unspecified = 0,
#warning Unconfirmed
        Unconfirm = 1,
#warning Confirmed
        Confirm = 2
    }
}
