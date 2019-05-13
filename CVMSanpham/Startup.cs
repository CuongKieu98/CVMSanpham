using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CVMSanpham.Startup))]
namespace CVMSanpham
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
