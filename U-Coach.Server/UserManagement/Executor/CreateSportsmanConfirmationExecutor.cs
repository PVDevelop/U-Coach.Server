using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PVDevelop.UCoach.Server.Core.Service;

namespace PVDevelop.UCoach.Server.UserManagement.Executor
{
    public class CreateSportsmanConfirmationExecutor : IExecutor
    {
        private readonly ISportsmanConfirmationService _confirmationService;
        private readonly CreateSportsmanConfirmationParams _confirmationParams = new CreateSportsmanConfirmationParams();

        public CreateSportsmanConfirmationExecutor(ISportsmanConfirmationService confirmationService)
        {
            if(confirmationService == null)
            {
                throw new ArgumentNullException("confirmationService");
            }

            _confirmationService = confirmationService;
        }

        public string[] ArgumentNames
        {
            get
            {
                return new[]
                {
                    "login",
                    "password",
                    "confirmation_key",
                    "email"
                };
            }
        }

        public string Command
        {
            get
            {
                return "confirmation";
            }
        }

        public string Description
        {
            get
            {
                return "Отправить подтверждение о создании нового пользователя на почту";
            }
        }

        public void Execute()
        {
            _confirmationService.CreateConfirmation(_confirmationParams);
        }

        public string GetSuccessString()
        {
            return "Подтверждение отправлено";
        }

        public void Setup(string[] arguments)
        {
            _confirmationParams.Login = arguments[0];
            _confirmationParams.Password = arguments[1];
            _confirmationParams.ConfirmationKey = arguments[2];
            _confirmationParams.Address = arguments[3];
        }
    }
}
