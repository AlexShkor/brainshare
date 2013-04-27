using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BrainShare.Documents;
using BrainShare.Mongo;
using BrainShare.Services;

namespace BrainShare.Authentication
{
    public class CustomAuthentication : IAuthentication
    {
        private readonly UsersService _users;

        public CustomAuthentication(UsersService users)
        {
            _users = users;
        }

        private const string cookieName = "__AUTH_COOKIE";
        
        public HttpContext HttpContext { get; set; }

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