﻿using Microsoft.AspNet.SignalR;

namespace Brainshare.Infrastructure.Hubs
{
    public class ThreadHub: Hub
    {
        public static IHubContext HubContext
        {
            get
            {
                return GlobalHost.ConnectionManager.GetHubContext<ThreadHub>();
            }
        }

        public void Join(string threadId)
        {
            Groups.Add(Context.ConnectionId, threadId);
        }
    }
}