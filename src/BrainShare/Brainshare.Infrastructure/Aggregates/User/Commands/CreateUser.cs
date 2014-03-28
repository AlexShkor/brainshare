using System;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Commands
{
    public class CreateUser : Command
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FacebookId { get; set; }
    }
}