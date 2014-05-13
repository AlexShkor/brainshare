using Brainshare.Infrastructure.Aggregates.Tenant.Commands;
using Brainshare.Infrastructure.Aggregates.Tenant.Events;
using Brainshare.Infrastructure.Platform.Domain;

namespace Brainshare.Infrastructure.Aggregates.Tenant
{
    public class TenantAggregate : Aggregate<TenantState>
    {
        public void Create(CreateTenant c)
        {
            Apply(new TenantCreated()
            {
                Id = c.Id,
                CreatedDate = c.CreatedDate,
                SubDomain = c.SubDomain,
                EmailDomain = c.EmailDomain,
                UserId = c.UserId,
                UserEmail = c.UserEmail,
                UserExpertises = c.UserExpertises,
                UserFirstName = c.UserFirstName,
                UserLastName = c.UserLastName,
                UserPasswordHash = c.UserPasswordHash,
                UserPasswordSalt = c.UserPasswordSalt,
                IsPublicEmail = c.IsPublicEmail
            });
        }

        public void AllowInviteUserFromPublicEmails(AllowInviteUserFromPublicEmails c)
        {
            if (!State.InviteUserFromAnyEmail)
            {
                Apply(new InviteUserFromPublicEmailsAllowed()
                {
                    Id = c.Id
                });   
            }
        }

        public void DisallowInviteUsersFromPublicEmails(DisallowInviteUsersFromPublicEmails c)
        {
            if (State.InviteUserFromAnyEmail)
            {
                Apply(new InviteUserFromPublicEmailsDisallowed()
                {
                    Id = c.Id
                });   
            }
        }
    }
}