﻿using Horrible_Charades_ASP.Models;
using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;

[assembly: OwinStartupAttribute(typeof(Horrible_Charades_ASP.Startup))]
namespace Horrible_Charades_ASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
