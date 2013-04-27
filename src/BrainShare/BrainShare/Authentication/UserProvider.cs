﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BrainShare.Services;
using MongoDB.Driver;

namespace BrainShare.Authentication
{
    public class UserProvider : IPrincipal
    {

        public UserProvider(string name, UsersService users)
        {
            userIdentity = new UserIdentity();
            userIdentity.Init(name, users);
        }


        private UserIdentity userIdentity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity { get { return userIdentity; } }

        public override string ToString()
        {
            return userIdentity.Name;
        }

    }
}