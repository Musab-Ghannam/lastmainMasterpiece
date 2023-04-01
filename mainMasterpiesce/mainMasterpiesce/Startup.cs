using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(mainMasterpiesce.Startup))]
namespace mainMasterpiesce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}



