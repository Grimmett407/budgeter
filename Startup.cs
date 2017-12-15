using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(budgeter.Startup))]
namespace budgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
