using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrainShare.Admin.Startup))]
namespace BrainShare.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
