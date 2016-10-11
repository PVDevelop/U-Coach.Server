using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public sealed class TokenDto
    {
        /// <summary>
        /// Унакальный ключ доступа в систему
        /// </summary>
        public string Key { get; set; }

        public TokenDto(string key)
        {
            key.NullOrEmptyValidate(nameof(key));

            this.Key = key;
        }
    }
}
