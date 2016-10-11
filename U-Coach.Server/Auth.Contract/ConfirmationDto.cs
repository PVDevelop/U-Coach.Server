using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public class ConfirmationDto
    {
        public string Key { get; set; }

        public ConfirmationDto(string key)
        {
            key.NullOrEmptyValidate(nameof(key));

            this.Key = key;
        }
    }
}
