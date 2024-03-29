﻿using System;
using System.Web.Mvc;
using BrainShare;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace BrainShare
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
           // GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new QueryStringUserIdProvider());
            app.MapSignalR();
        }
    }

    public class QueryStringUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.GetHttpContext().User.Identity.Name;
        }
    }
}