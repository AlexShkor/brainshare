using System;
using System.Threading.Tasks;
using Brainshare.Infrastructure.Authentication;
using Microsoft.AspNet.SignalR;

namespace Brainshare.Infrastructure.Hubs
{
    [Authorize]
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
                var userId = GetUserId();
                if (userId != null)
                {
                    Groups.Add(Context.ConnectionId, userId);
                }
            });
        }


        private string GetUserId()
        {
            try
            {
               return ((UserIdentity) Context.Request.GetHttpContext().User.Identity).Id;
            }
            catch (Exception e)
            {
                return "very-notable-id)";
            }
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