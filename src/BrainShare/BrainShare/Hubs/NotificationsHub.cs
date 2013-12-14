using System;
using System.Threading.Tasks;
using System.Web;
using BrainShare.Authentication;
using BrainShare.Documents;
using BrainShare.Services;
using Microsoft.AspNet.SignalR;

namespace BrainShare.Hubs
{
    [Authorize]
    public class NotificationsHub: Hub
    {
        private readonly UsersService _users;

        public NotificationsHub(UsersService users)
        {
            _users = users;
        }

        public static IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();
            }
        }

        public static bool IsOnline(string userId)
        {
            return HubContext.Clients.Group(userId) == null;
        }

        public override Task OnConnected()
        {
            return Task.Factory.StartNew(() =>
            {
                var userId = GetUserId();
                if (userId != null)
                {
                    Groups.Add(Context.ConnectionId, userId);
                    _users.SetOnlineStatus(userId, true);
                }
            });
        }

        public override Task OnDisconnected()
        {
            return Task.Factory.StartNew(() =>
            {
                var userId = GetUserId();
                if (userId != null)
                {
                    Groups.Remove(Context.ConnectionId, userId);
                    _users.SetOnlineStatus(userId, false);
                    _users.SetLastVisitedDate(DateTime.UtcNow,userId);
                }
            });
        }

        private string GetUserId()
        {
             return  ((UserIdentity) Context.Request.GetHttpContext().User.Identity).User.Id;    
        }

  

        public static void SendGenericText(string userId, string title, string message)
        {
            HubContext.Clients.Group(userId).genericText(new
            {
                Title = title,
                Message = message
            });

            //HubContext.Clients.User(userId).genericText(new
            //{
            //    Title = title,
            //    Message = message
            //});
        }
    }
}