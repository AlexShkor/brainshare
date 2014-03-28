using System;
using Brainshare.Infrastructure.Aggregates.User.Commands;
using Brainshare.Infrastructure.Aggregates.User.Events;
using Brainshare.Infrastructure.Platform.Domain;
using Brainshare.Infrastructure.Platform.Extensions;

namespace Brainshare.Infrastructure.Aggregates.User
{
    public class UserAggregate : Aggregate<UserState>
    {
        public void Create(
            string id, 
            string firstName, 
            string lastName, 
            string passwordHash, 
            string passwordSalt, 
            string email,
            string facebookId)
        {
            if (State.Id.HasValue())
            {
                if (State.UserWasDeleted)
                {
                    Apply(new UserReCreated
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        CreationDate = DateTime.Now
                    });
                }
                else
                {
                    throw new InvalidOperationException("User with same ID already exist.");
                }
            }
            else
            {
                Apply(new UserCreated
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreationDate = DateTime.Now,
                    FacebookId = facebookId
                });
            }
        }

        public void ChangePassword(string passwordHash, string passwordSalt, bool isChangedByAdmin)
        {

            Apply(new PasswordChanged
            {
                Id = State.Id,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                WasChangedByAdmin = isChangedByAdmin,
            });
        }

        public void Delete(DeleteUser c)
        {
            Apply(new UserDeleted
            {
                Id = c.Id,
                DeletedByUserId = c.DeletedByUserId,
            });
        }

        public void UpdateDetails(UpdateUserDetails c)
        {
            Apply(new UserDetailsUpdated
            {
                Id = c.Id,
                UserName = c.UserName
            });
        }
    }
}