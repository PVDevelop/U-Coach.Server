using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Auth.Contract
{
    public class ConfirmUrlDTO
    {
        public string Url { get; set; }

        public ConfirmUrlDTO(string url)
        {
            Url = url;
        }
    }
}
