using System;

namespace PVDevelop.UCoach.Server.Role.Contract
{
    public class UserInfoDto
    {
        public string Id { get; private set; }

        public UserInfoDto(string id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }
    }
}
