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

        public UserApplicationService(IRepository<UserAggregate> repository,CryptographicHelper crypto)
        {
            _repository = repository;
            _crypto = crypto;
        }

        public void Handle(CreateUser c)
        {
            var salt = _crypto.GenerateSalt();
            _repository.Perform(c.Id, user => user.Create(
                c.Id,
                c.FirstName,
                c.LastName,
                _crypto.GetPasswordHash(c.Password,salt),
                salt,
                c.Email,
                c.FacebookId));
        }

        public void Handle(ChangePassword c)
        {
            var salt = _crypto.GenerateSalt();
            var hash = _crypto.GetPasswordHash(c.NewPassword, salt);
            _repository.Perform(c.Id, user => user.ChangePassword(hash,salt,c.IsChangedByAdmin));
        }

        public void Handle(DeleteUser c)
        {
            _repository.Perform(c.Id, user => user.Delete(c));
        }

        public void Handle(UpdateUserDetails c)
        {
            _repository.Perform(c.Id, user => user.UpdateDetails(c));
        }
    }
}