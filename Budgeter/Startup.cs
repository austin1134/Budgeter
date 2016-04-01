using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CF_Budgeter.Startup))]
namespace CF_Budgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
