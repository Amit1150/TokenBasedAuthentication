using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TBA.API.Startup))]
namespace TBA.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}