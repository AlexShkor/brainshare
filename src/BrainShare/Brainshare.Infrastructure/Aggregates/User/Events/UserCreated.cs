using System;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class UserCreated : Event
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public string FacebookId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}