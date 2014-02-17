using System;
using System.Web;
using System.Web.Mvc;
using Brainshare.Infrastructure.Authentication;

namespace BrainShare.Infrastructure.Authentication
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.Authenticate);
        }

        private void Authenticate(Object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            context.User = auth.CurrentUser;
        }

        public void Dispose()
        {
        }
    }
}