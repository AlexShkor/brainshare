using Brainshare.Infrastructure.Aggregates.Tenant.Commands;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;

namespace Brainshare.Infrastructure.Aggregates.Tenant
{
    public class TenantApplicationService : IMessageHandler
    {
        private readonly IRepository<TenantAggregate> _repository;

        public TenantApplicationService(IRepository<TenantAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateTenant c)
        {
            _repository.Perform(c.Id, user => user.Create(c));
        }        
        
        public void Handle(AllowInviteUserFromPublicEmails c)
        {
            _repository.Perform(c.Id, user => user.AllowInviteUserFromPublicEmails(c));
        }

        public void Handle(DisallowInviteUsersFromPublicEmails c)
        {
            _repository.Perform(c.Id, user => user.DisallowInviteUsersFromPublicEmails(c));
        }
    }
}