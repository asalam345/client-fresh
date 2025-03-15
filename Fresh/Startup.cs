using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fresh.Startup))]
namespace Fresh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
