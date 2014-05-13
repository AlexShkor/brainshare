using Brainshare.Infrastructure.Aggregates.Tenant.Events;
using Brainshare.Infrastructure.Platform.Domain;

namespace Brainshare.Infrastructure.Aggregates.Tenant
{
    public sealed class TenantState : AggregateState
    {
        public string Id { get; private set; }

        public bool InviteUserFromAnyEmail { get; private set; }

        public TenantState()
        {
            On((TenantCreated e) => Id = e.Id);
            On((InviteUserFromPublicEmailsAllowed e) => InviteUserFromAnyEmail = true);
            On((InviteUserFromPublicEmailsDisallowed e) => InviteUserFromAnyEmail = false);
        }
    }
}