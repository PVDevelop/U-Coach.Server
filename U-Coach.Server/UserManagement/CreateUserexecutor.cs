using PVDevelop.UCoach.Server.AuthService;
using System;

namespace PVDevelop.UCoach.Server.UserManagement
{
    public class CreateUserExecutor : IExecutor
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public string Command
        {
            get
            {
                return "create";
            }
        }

        public string[] ArgumentsNames
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

        private readonly IUserService _userService;

        public CreateUserExecutor(IUserService userService)
        {
            if(userService == null)
            {
                throw new ArgumentNullException("userService");
            }

            _userService = userService;
        }

        public void Setup(string[] arguments)
        {
            Login = arguments[0];
            Password = arguments[1];
        }

        public void Execute()
        {
            var userParams = new CreateUserParams()
            {
                Login = Login,
                Password = Password
            };

            _userService.Create(userParams);
        }

        public string GetSuccessString()
        {
            return string.Format("Пользователь {0} создан", Login);
        }
    }
}
