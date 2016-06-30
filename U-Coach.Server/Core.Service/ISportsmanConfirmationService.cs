using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.Core.Service
{
    public interface ISportsmanConfirmationService
    {
        void CreateConfirmation(CreateSportsmanConfirmationParams userParams);
    }
}
