namespace BrainShare.Documents.Data
{
    public class UserSettings
    {
        public UserSettings()
        {
            CommonSettings = new CommonSettings();
            NotificationSettings = new NotificationSettings();
            PrivacySettings = new PrivacySettings();
        }

        public CommonSettings CommonSettings { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
        public PrivacySettings PrivacySettings { get; set; }
    }

    public class CommonSettings
    {

    }

    public class NotificationSettings
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
        public bool DuplicateMessagesToEmail { get; set; }
    }

    public class PrivacySettings
    {
        public bool IsProfilePublic { get; set; }
    }

}