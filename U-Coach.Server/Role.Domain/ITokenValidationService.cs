using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Role.Domain
{
    public interface ITokenValidationService
    {
        /// <summary>
        /// Валидирует токен и в случае невалидности кидает NotAuthorizedException
        /// </summary>
        void Validate(TokenId tokenId);
    }
}
