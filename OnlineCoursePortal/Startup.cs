using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnlineCoursePortal.Startup))]
namespace OnlineCoursePortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
