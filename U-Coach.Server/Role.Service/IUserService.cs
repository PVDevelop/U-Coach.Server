using PVDevelop.UCoach.Server.Role.Contract;

namespace PVDevelop.UCoach.Server.Role.Service
{
    public interface IUserService
    {
        void RegisterFacebookUser(RegisterFacebookUserDto facebookUserDto);
    }
}
