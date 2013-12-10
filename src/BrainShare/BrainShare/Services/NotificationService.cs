using System;
using BrainShare.Documents;

namespace BrainShare.Services
{
    public class NotificationService
    {
        private readonly UsersService _users;

        public NotificationService(UsersService users)
        {
            _users = users;
        }

        public void Send(string userId, string message)
        {
            var user = _users.GetById(userId);
            var notification = new Notification(message);
            user.Notifications.Add(notification);
            _users.Save(user);
        }

        public void SendAllUsers(string message)
        {
            throw new NotImplementedException();
        }
    }
}