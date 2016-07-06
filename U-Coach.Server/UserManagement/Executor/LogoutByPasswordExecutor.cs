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

        private readonly IUsersClient _usersClient;

        public LogoutByPasswordExecutor(IUsersClient usersClient)
        {
            if (usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }

            _usersClient = usersClient;
        }

        public void Execute()
        {
            var userParams = new LogoutByPasswordUserDto()
            {
                Login = Login,
                Password = Password
            };

            _usersClient.LogoutByPassword(userParams);
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
