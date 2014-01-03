using Brainshare.Infrastructure.Services;
using Microsoft.AspNet.SignalR;

namespace BrainShare
{
    public class RabbitMQ
    {
        public static void Start()
        {
           var  ozService =  GlobalHost.DependencyResolver.Resolve<OzIsbnService>();
            ozService.Run();
        }
    }
}