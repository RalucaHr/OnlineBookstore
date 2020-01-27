using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DTA.Web.Startup))]
namespace DTA.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
