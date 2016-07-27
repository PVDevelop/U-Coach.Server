using System;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public class UserId
    {
        /// <summary>
        /// Имя системы, в которой аутентифицируется пользователь
        /// </summary>
        public Guid Id { get; set; }

        public UserId(Guid id)
        {
            if(id == default(Guid))
            {
                throw new ArgumentException("Is empty", nameof(id));
            }

            Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var userId = obj as UserId;
            if (userId == null)
            {
                return false;
            }

            return Id == userId.Id;
        }
    }
}
