using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(live.courses.Startup))]

namespace live.courses
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //for chat
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["adv_coursesEntities"].ConnectionString;
            GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            //
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
