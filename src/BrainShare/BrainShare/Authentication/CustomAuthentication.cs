using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BrainShare.Documents;
using BrainShare.Mongo;

namespace BrainShare.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        public CustomAuthentication(MongoDocumentsDatabase database)
        {
            this.Database = database;
        }

        private const string cookieName = "__AUTH_COOKIE";
        
        public HttpContext HttpContext { get; set; }

        public MongoDocumentsDatabase Database { get; set; }

        public User Login(string userName, string password, bool isPersistent)
        {
            throw new NotImplementedException();
        }

        public User Login(string login)
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public IPrincipal CurrentUser { get; private set; }
    }
}