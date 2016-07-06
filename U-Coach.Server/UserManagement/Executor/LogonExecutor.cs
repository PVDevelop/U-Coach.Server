using PVDevelop.UCoach.Server.Auth.Service;
using System;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class LogonExecutor : IExecutor
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

        private readonly IUsersClient _usersClient;

        public LogonExecutor(IUsersClient usersClient)
        {
            if (usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }

            _usersClient = usersClient;
        }

        public void Execute()
        {
            var userParams = new LogonUserDto()
            {
                Login = Login,
                Password = Password
            };

            _usersClient.Logon(userParams);
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
