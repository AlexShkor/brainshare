using System.Collections.Generic;
using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Aggregates.User.Events
{
    public class ExpertsConnectionCreated : Event
    {
        public List<string> Users { get; set; }



    }
}