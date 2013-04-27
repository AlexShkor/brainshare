using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrainShare.Documents;
using BrainShare.MongoDB;

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
    }
}