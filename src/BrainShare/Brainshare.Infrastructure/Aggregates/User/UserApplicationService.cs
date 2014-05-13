using Brainshare.Infrastructure.Aggregates.User.Commands;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;

namespace Brainshare.Infrastructure.Aggregates.User
{
    public class UserApplicationService : IMessageHandler
    {
        private readonly IRepository<UserAggregate> _repository;
        private readonly CryptographicHelper _crypto;

        public UserApplicationService(IRepository<UserAggregate> repository, CryptographicHelper crypto)
        {
            _repository = repository;
            _crypto = crypto;
        }

        public void Handle(RegisterUser c)
        {
            _repository.Perform(c.Id, user => user.Create(c));
        }

        public void Handle(ChangePassword c)
        {
            _repository.Perform(c.Id, user => user.ChangePassword(c));
        }

        public void Handle(MarkUserAsDeleted c)
        {
            _repository.Perform(c.Id, user => user.MarkUserAsDeleted(c));
        }

        public void Handle(DeleteUser c)
        {
            _repository.Perform(c.Id, user => user.Delete(c));
        }

        public void Handle(UpdateUserDetails c)
        {
            _repository.Perform(c.Id, user => user.UpdateUserDetails(c));
        }

        public void Handle(UpdateResetPasswordCode c)
        {
            _repository.Perform(c.Id, user => user.UpdateResetPasswordCode(c.Code));
        }

        public void Handle(ChangeUserAvatar c)
        {
            _repository.Perform(c.Id, user => user.ChangeUserAvatar(c));
        }

        public void Handle(SendEmailInvitions c)
        {
            _repository.Perform(c.Id, user => user.SendEmailInvitions(c));
        }

        public void Handle(AddUserRole c)
        {
            _repository.Perform(c.Id, user => user.AddRole(c));
        }

        public void Handle(RemoveUserRole c)
        {
            _repository.Perform(c.Id, user => user.RemoveRole(c));
        }        
        
        public void Handle(ActivateUser c)
        {
            _repository.Perform(c.Id, user => user.Activate(c));
        }

        public void Handle(DeactivateUser c)
        {
            _repository.Perform(c.Id, user => user.Deactivate(c));
        }

        public void Handle(UnfeaturedUser c)
        {
            _repository.Perform(c.Id, user => user.Unfeatured(c));
        }

        public void Handle(CreateSystemUser c)
        {
            _repository.Perform(c.Id, user => user.CreateSystemUser(c));
        }
    }
}