using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    /// <summary>
    /// Идентификатор пользователя системы аутентифиакции
    /// </summary>
    public class AuthUserId
    {
        /// <summary>
        /// Имя системы, в которой аутентифицируется пользователь
        /// </summary>
        public string AuthSystemName { get; private set; }

        /// <summary>
        /// Идентификатор пользователя в системе, в которой аутентифицируется пользователь
        /// </summary>
        public string AuthId { get; private set; }

        public AuthUserId(string authSystemName, string authUserId)
        {
            if (authSystemName == null)
            {
                throw new ArgumentNullException(nameof(authSystemName));
            }
            if (authUserId == null)
            {
                throw new ArgumentNullException(nameof(authUserId));
            }

            AuthSystemName = authSystemName;
            AuthId = authUserId;
        }

        public override int GetHashCode()
        {
            return
                AuthSystemName.GetHashCode() ^ AuthId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            var userId = obj as AuthUserId;
            if(userId == null)
            {
                return false;
            }

            return
                AuthSystemName == userId.AuthSystemName &&
                AuthId == userId.AuthId;
        }
    }
}
