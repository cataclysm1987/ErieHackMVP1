using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ErieHackMVP1.Startup))]
namespace ErieHackMVP1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
