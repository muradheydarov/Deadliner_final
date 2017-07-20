using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeadLiner.Startup))]
namespace DeadLiner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
