using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommercialWeb.Startup))]
namespace CommercialWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
