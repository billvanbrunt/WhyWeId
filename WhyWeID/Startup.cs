using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WhyWeID.Startup))]
namespace WhyWeID
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
