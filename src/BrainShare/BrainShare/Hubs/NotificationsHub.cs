using System.Threading.Tasks;
using BrainShare.Authentication;
using Microsoft.AspNet.SignalR;

namespace BrainShare.Hubs
{
    public class NotificationsHub: Hub
    {
        public static IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            }
        }

        public override Task OnConnected()
        {
            return Task.Factory.StartNew(() =>
                {
                    if (Context.User != null)
                    {
                        Groups.Add(Context.ConnectionId, ((UserIdentity) Context.User.Identity).User.Id);
                    }
                });
        }

        public override Task OnDisconnected()
        {
            return Task.Factory.StartNew(() =>
            {
                if (Context.User != null)
                {
                    Groups.Remove(Context.ConnectionId, ((UserIdentity) Context.User.Identity).User.Id);
                }
            });
        }

        public static void SendGenericText(string userId, string title, string message)
        {
            HubContext.Clients.Group(userId).genericText(new
            {
                Title = title,
                Message = message
            });
        }
    }
}