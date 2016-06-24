using PVDevelop.UCoach.Server.Auth.Service;
using System;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class LogonExecutor : IExecutor
    {
        public string Login { get; private set; }

        public string Password { get; private set; }

        public string[] ArgumentsNames
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
                return "logon";
            }
        }

        public string Description
        {
            get
            {
                return "Залогиниться";
            }
        }

        private readonly IUserService _userService;

        public LogonExecutor(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        public void Execute()
        {
            var userParams = new LogonUserParams()
            {
                Login = Login,
                Password = Password
            };

            _userService.Logon(userParams);
        }

        public void Setup(string[] arguments)
        {
            Login = arguments[0];
            Password = arguments[1];
        }

        public string GetSuccessString()
        {
            return string.Format("Пользователь {0} залогинился", Login);
        }
    }
}
