using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LogBook.Startup))]
namespace LogBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
