using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_ecomm.Startup))]
namespace Project_ecomm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
