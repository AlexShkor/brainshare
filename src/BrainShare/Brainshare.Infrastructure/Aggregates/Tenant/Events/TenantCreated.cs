using System;
using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.Tenant.Events
{
    public class TenantCreated : Event
    {
        public DateTime CreatedDate { get; set; }

        public string SubDomain { get; set; }

        public string EmailDomain { get; set; }

        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string UserPasswordHash { get; set; }

        public string UserPasswordSalt { get; set; }

        public List<string> UserExpertises { get; set; }

        public bool IsPublicEmail { get; set; }
    }
}
