﻿using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BrainShare.DependencyResolution;
using Brainshare.Infrastructure;
using Brainshare.Infrastructure.Sitemap;
using Microsoft.AspNet.SignalR;
using StructureMap;

namespace BrainShare
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            SitemapService.Register();

            IContainer container = IoC.Initialize();
            ContainerConfigure.Configure(container);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);

            GlobalHost.DependencyResolver = new StructureMapResolver(container);
           

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RabbitMQ.Start();
     
        }

        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    if (Request.Url.Host.Equals("brainshare.apphb.com", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        Response.RedirectPermanent("http://brainshare.me");
        //    }
        //}
    }
}