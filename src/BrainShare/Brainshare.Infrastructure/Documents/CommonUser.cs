using System;
using System.Collections.Generic;
using Brainshare.Infrastructure.Documents.Data;

namespace Brainshare.Infrastructure.Documents
{
    public class CommonUser
    {
        public string Id { get; set; }
        public Type OriginalType { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public bool IsShell { get; set; }
        public bool IsFacebookAccount { get; set; }
        public string FullName  { get; set; }
        public string AvatarUrl { get; set; }
        public List<LoginService> LoginServices { get; set; }
    }
}