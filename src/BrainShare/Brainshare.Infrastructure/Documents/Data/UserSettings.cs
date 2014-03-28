namespace Brainshare.Infrastructure.Documents.Data
{
    public class UserSettings
    {
        public UserSettings()
        {
            NotificationSettings = new NotificationSettings();
        }

        public NotificationSettings NotificationSettings { get; set; }
    }

    public class NotificationSettings
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
        public bool DuplicateMessagesToEmail { get; set; }
    }
}