using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcAuthrorization.Startup))]
namespace MvcAuthrorization
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
