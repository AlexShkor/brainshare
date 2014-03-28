using Brainshare.Infrastructure.Platform.Domain.Messages;

namespace Brainshare.Infrastructure.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        string Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}