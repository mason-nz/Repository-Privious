using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Startup))]

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
