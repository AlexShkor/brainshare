using BrainShare.Domain.Documents.Data;

namespace BrainShare.ViewModels
{
    public class SettingsViewModel
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
        public bool DuplicateMessagesToEmail { get; set; }

        public VkGroupsSettings VkGroupsSettings { get; set; }

        public NotificationSettings GetNotificationSettings()
        {
            return new NotificationSettings
                {
                    NotifyByEmailIfAnybodyAddedMyWishBook = NotifyByEmailIfAnybodyAddedMyWishBook,
                    DuplicateMessagesToEmail = DuplicateMessagesToEmail
                };
        }
    }
}