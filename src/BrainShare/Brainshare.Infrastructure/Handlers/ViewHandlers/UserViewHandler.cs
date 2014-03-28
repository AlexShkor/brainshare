using Brainshare.Infrastructure.Aggregates.User.Events;
using Brainshare.Infrastructure.Databases;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Platform.Dispatching;
using Brainshare.Infrastructure.Platform.Dispatching.Attributes;
using Brainshare.Infrastructure.Platform.Dispatching.Interfaces;
using Uniform;

namespace Brainshare.Infrastructure.Handlers.ViewHandlers
{
    [Priority(PriorityStages.ViewHandling)]
    public class UserViewHandler : IMessageHandler
    {
        private readonly ViewDatabase _db;
        private readonly IDocumentCollection<User> _users;

        public UserViewHandler(ViewDatabase db)
        {
            _db = db;
            _users = db.Users;
        }

        public void Handle(UserCreated e)
        {
            CreateUser(e);
        }

        public void Handle(UserReCreated e)
        {
            CreateUser(e);
        }

        private void CreateUser(UserCreated e)
        {
            var user = new User
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FacebookId = e.FacebookId,
                Email = e.Email.ToLowerInvariant(),
                Password = e.PasswordHash,
                Salt = e.PasswordSalt,
                Created = e.CreationDate,
            };
            _users.Save(user);
        }

        public void Handle(PasswordChanged e)
        {
            _users.Update(e.Id, u =>
            {
                u.Password = e.PasswordHash;
                u.Salt = e.PasswordSalt;
            });
        }

        public void Handle(UserDeleted e)
        {
            _users.Delete(e.Id);
        }

    }
}