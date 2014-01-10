using System.Collections.Generic;
using BrainShare.Domain.Documents.Data;
using BrainShare.Helpers;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;

namespace BrainShare.ViewModels
{
    public class NotificationSettingsViewModel
    {
        public bool NotifyByEmailIfAnybodyAddedMyWishBook { get; set; }
        public bool DuplicateMessagesToEmail { get; set; }

        public List<Service> Services { get; set; }

        public NotificationSettingsViewModel()
        {
        }

        public NotificationSettingsViewModel(NotificationSettings notificationSettings, IEnumerable<LoginService> loginServices)
        {
            NotifyByEmailIfAnybodyAddedMyWishBook = notificationSettings.NotifyByEmailIfAnybodyAddedMyWishBook;
            DuplicateMessagesToEmail = notificationSettings.DuplicateMessagesToEmail;

            Services = new List<Service>();

            foreach (var loginService in loginServices)
            {
                Services.Add(new Service
                    {
                        IsChecked = loginService.UseForNotifications,
                        LogoUrl = UrlHelper.GetServiceLogoUrl(loginService.LoginType,StringUtility.GetEmailSuffix(loginService.ServiceUserId)),
                        IsEmailConfirmed = loginService.LoginType != LoginServiceTypeEnum.Email || loginService.EmailConfirmed,
                        ServiceId = loginService.ServiceUserId
                    });
            }
        }

        public NotificationSettings GetNotificationSettings()
        {
            return new NotificationSettings
                {
                    DuplicateMessagesToEmail = DuplicateMessagesToEmail,
                    NotifyByEmailIfAnybodyAddedMyWishBook = NotifyByEmailIfAnybodyAddedMyWishBook
                };
        }
    }

    public class Service
    {
        public string LogoUrl { get; set; }
        public string ServiceId { get; set; }
        public bool IsChecked { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}