using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SocialNetwork.PresentationLayer.App_Start.StartupSignalR))]

namespace SocialNetwork.PresentationLayer.App_Start
{
    public class StartupSignalR
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
