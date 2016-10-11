using System;
using PVDevelop.UCoach.Server.Auth.Contract;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class CreateUserExecutor : IExecutor
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string Url { get; private set; }

        public string Command
        {
            get
            {
                return "user";
            }
        }

        public string[] ArgumentNames
        {
            get
            {
                return new[] { "login", "password" };
            }
        }

        public string Description
        {
            get
            {
                return "Создать нового пользователя";
            }
        }

        private readonly IUsersClient _usersClient;

        public CreateUserExecutor(IUsersClient usersClient)
        {
            if(usersClient == null)
            {
                throw new ArgumentNullException(nameof(usersClient));
            }

            _usersClient = usersClient;
        }

        public void Setup(string[] arguments)
        {
            Login = arguments[0];
            Password = arguments[1];
            Url = arguments[2];
        }

        public void Execute()
        {
            _usersClient.Create( new UserDto(Login, Password, Url));
        }

        public string GetSuccessString()
        {
            return string.Format("Пользователь {0} создан", Login);
        }
    }
}
