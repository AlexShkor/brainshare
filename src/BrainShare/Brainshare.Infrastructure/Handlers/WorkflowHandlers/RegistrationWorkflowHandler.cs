using System;
using System.Linq;
using Brainshare.Infrastructure;
using Brainshare.Infrastructure.Aggregates.Registration.Events;
using Brainshare.Infrastructure.Aggregates.Tenant.Commands;
using Brainshare.Infrastructure.Aggregates.Tenant.Events;
using Brainshare.Infrastructure.Aggregates.User.Commands;
using Brainshare.Infrastructure.Platform.Dispatching;
using Brainshare.Infrastructure.Platform.Dispatching.Attributes;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace QV.Hummingbird.Infrastructure.Handlers.WorkflowHandlers
{
    [Priority(PriorityStages.ViewHandling_Completed)]
    public class RegistrationWorkflowHandler : IMessageHandler
    {
        private readonly ICommandBus _bus;

        public RegistrationWorkflowHandler(ICommandBus bus)
        {
            _bus = bus;
        }

        public void Handle(RegistrationTenantCreated message)
        {
            _bus.Send(new CreateTenant()
            {
                Id = message.TenantId,
                TenantId = message.TenantId,
                CreatedDate = DateTime.UtcNow,
                SubDomain = message.SubDomain,
                EmailDomain = message.EmailDomain,
                UserId = message.UserId,
                UserEmail = message.UserEmail,
                UserExpertises = message.UserExpertises,
                UserFirstName = message.UserFirstName,
                UserLastName = message.UserLastName,
                UserPasswordHash = message.UserPasswordHash,
                UserPasswordSalt = message.UserPasswordSalt,
                IsPublicEmail = message.IsPublicEmail,
                Metadata = new CommandMetadata()
                {
                    UserId = message.Metadata.UserId
                }
            });

            var systemUserId = GenerateSystemUserId(message.TenantId);
            _bus.Send(new CreateSystemUser()
            {
                Id = systemUserId,
                TenantId = message.TenantId,
                FirstName = AppConstants.SystemUserFirstName,
                LastName = AppConstants.SystemUserLastName,
                Email = AppConstants.SystemUserUniqueId + AppConstants.SystemUserEmailDomain,
                CreatedOn = DateTime.UtcNow,
                Metadata = new CommandMetadata()
                {
                    UserId = message.Metadata.UserId
                }
            });
        }

        public void Handle(RegistrationUserCreated message)
        {
            _bus.Send(new RegisterUser()
            {
                Id = message.UserId,
                FirstName = message.FirstName,
                TenantId = message.TenantId,
                LastName = message.LastName,
                PasswordHash = message.PasswordHash,
                PasswordSalt = message.PasswordSalt,
                Email = message.Email,
                Expertises = message.Expertises,
                Metadata = new CommandMetadata()
                {
                    UserId = message.Metadata.UserId
                }
            });
        }

        public void Handle(TenantCreated e)
        {
            _bus.Send(new RegisterUser()
            {
                Id = e.UserId,
                TenantId = e.TenantId,
                Email = e.UserEmail,
                FirstName = e.UserFirstName,
                LastName = e.UserLastName,
                PasswordHash = e.UserPasswordHash,
                PasswordSalt = e.UserPasswordSalt,
                Expertises = e.UserExpertises,
                Metadata = new CommandMetadata()
                {
                    UserId = e.Metadata.UserId
                }
            });
        }

        private string GenerateSystemUserId(string tenantId)
        {
            return "su_" + tenantId;
        }
    }
}
