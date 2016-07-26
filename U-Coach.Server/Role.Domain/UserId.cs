using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public class UserId
    {
        public const string SPLITTER = "%40";

        public static UserId Parse(string str)
        {
            var subStrings = str.Split(new[] { SPLITTER }, StringSplitOptions.None);
            if(subStrings.Length != 2)
            {
                throw new ArgumentException("Does not meet id format", nameof(str));
            }
            return new UserId(subStrings[0], subStrings[1]);
        }

        /// <summary>
        /// Имя системы, в которой аутентифицируется пользователь
        /// </summary>
        public string AuthSystemName { get; set; }

        /// <summary>
        /// Идентификатор пользователя в системе, в которой аутентифицируется пользователь
        /// </summary>
        public string AuthId { get; set; }

        public UserId(string authSystemName, string authId)
        {
            if (authSystemName == null)
            {
                throw new ArgumentNullException(nameof(authSystemName));
            }
            if (authId == null)
            {
                throw new ArgumentNullException(nameof(authId));
            }

            AuthSystemName = authSystemName;
            AuthId = authId;
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

            var userId = obj as UserId;
            if(userId == null)
            {
                return false;
            }

            return
                AuthSystemName == userId.AuthSystemName &&
                AuthId == userId.AuthId;
        }

        public override string ToString()
        {
            return string.Join(SPLITTER, AuthSystemName, AuthId);
        }
    }
}
