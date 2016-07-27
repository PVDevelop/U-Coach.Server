using System;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class UserIdDto
    {
        public Guid Id { get; private set; }

        public UserIdDto(Guid id)
        {
            if(id == default(Guid))
            {
                throw new ArgumentException("Is empty", nameof(id));
            }

            Id = id;
        }
    }
}
