using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CreditTask2.Startup))]
namespace CreditTask2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
