using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PAM.MVC.Startup))]
namespace PAM.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
