using System;
using PVDevelop.UCoach.Server.Configuration;

namespace PVDevelop.UCoach.Server.WebApi
{
    public class ActionResultBuilderFactory :
        IActionResultBuilderFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public ActionResultBuilderFactory(IConnectionStringProvider connectionStringProvider)
        {
            if(connectionStringProvider == null)
            {
                throw new ArgumentNullException(nameof(connectionStringProvider));
            }
            _connectionStringProvider = connectionStringProvider;
        }

        public IActionResultBuilder CreateActionResultBuilder()
        {
            return new ActionResultBuilder(_connectionStringProvider.ConnectionString);
        }
    }
}
