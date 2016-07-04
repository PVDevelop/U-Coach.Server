using PVDevelop.UCoach.Server.Auth.Service;
using System;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class LogoutByPasswordExecutor : IExecutor
    {
        public string Login { get; private set; }

        public string Password { get; private set; }

        public string[] ArgumentNames
        {
            get
            {
                return new[] { "login", "password" };
            }
        }

        public string Command
        {
            get
            {
                return "plogout";
            }
        }

        public string Description
        {
            get
            {
                return "Разлогиниться по паролю";
            }
        }

        private readonly IUserService _userService;

        public LogoutByPasswordExecutor(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        public void Execute()
        {
            var userParams = new LogoutByPasswordUserDto()
            {
                Login = Login,
                Password = Password
            };

            _userService.LogoutByPassword(userParams);
        }

        public string GetSuccessString()
        {
            return string.Format("Пользователь {0} разлогинился", Login);
        }

        public void Setup(string[] arguments)
        {
            Login = arguments[0];
            Password = arguments[1];
        }
    }
}
