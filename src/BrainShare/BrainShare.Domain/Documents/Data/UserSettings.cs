using System.Collections.Generic;

namespace BrainShare.Domain.Documents.Data
{
    public class UserSettings
    {
        public UserSettings()
        {
            NotificationSettings = new NotificationSettings();
            VkGroupsSettings = new VkGroupsSettings();
        }

        public NotificationSettings NotificationSettings { get; set; }
        public VkGroupsSettings VkGroupsSettings { get; set; }
    }

    public class NotificationSettings
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
        public bool DuplicateMessagesToEmail { get; set; }
    }

    public class VkGroupsSettings
    {
        public VkGroupsSettings()
        {
            Groups = new List<VkGroup>();
            NewGroupTemplate = new NewGroupTemplate ();
        }

        public List<VkGroup> Groups { get; set; }
        public NewGroupTemplate NewGroupTemplate { get; set; }
    }

    public class VkGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StatusName { get; set; }
    }

    public class NewGroupTemplate
    {
        public bool IsVisible { get; set; }
        public string GroupUrl { get; set; }
        public string PostStatus { get; set; }
    }
}