namespace Brainshare.Infrastructure.Platform.Dispatching.Interfaces
{
    public interface IMessageHandlerInterceptor
    {
        void Intercept(DispatcherInvocationContext context);
    }
}
