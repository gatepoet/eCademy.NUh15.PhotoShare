using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eCademy.NUh15.PhotoShare.Startup))]
namespace eCademy.NUh15.PhotoShare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
