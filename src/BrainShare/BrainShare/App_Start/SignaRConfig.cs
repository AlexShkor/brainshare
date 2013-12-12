using System;
using System.Web.Mvc;
using BrainShare;
using BrainShare.Authentication;
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
         //   GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new QueryStringUserIdProvider());
            app.MapSignalR();
        }
    }

    //public class QueryStringUserIdProvider : IUserIdProvider
    //{
    //    public string GetUserId(IRequest request)
    //    {
    //       // return ((UserIdentity)Context.User.Identity).User.Id; "5555"; //request.QueryString["user"] ?? Guid.NewGuid().ToString();
    //    }
    //}
}