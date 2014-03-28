namespace Brainshare.Infrastructure.Platform.Dispatching.Interfaces
{
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }

    public interface IMessageHandler
    {
        // Marker interface
    }

}
