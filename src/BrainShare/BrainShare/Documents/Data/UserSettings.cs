namespace BrainShare.Documents.Data
{
    public class UserSettings
    {
        public CommonSettings CommonSettings { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
        public CommonSettings PrivacySettings { get; set; }
    }

    public class CommonSettings
    {

    }

    public class NotificationSettings
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
    }

    public class PrivacySettings
    {
        public bool IsProfilePublic { get; set; }
    }

}