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

        public void Join(string userId)
        {
            Groups.Add(Context.ConnectionId, userId);
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