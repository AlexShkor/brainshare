using System.Collections.Generic;
using BrainShare.Domain.Documents.Data;
using BrainShare.Helpers;
using BrainShare.Utils.Utilities;

namespace BrainShare.ViewModels.ProfileSettings
{
    public class PrivacySettingsViewModel
    {
        public List<LogoImage> ExistingAccounts { get; set; }
        public List<LogoImage> SupportedServices { get; set; }

        public PrivacySettingsViewModel(IEnumerable<LoginService> logins, PrivacySettings privacySettings)
        {
            ExistingAccounts = new List<LogoImage>();
            SupportedServices = new List<LogoImage>();

            foreach (var loginService in logins)
            {
                ExistingAccounts.Add(new LogoImage
                    {
                        LogoUrl = UrlHelper.GetServiceLogoUrl(loginService.LoginType, StringUtility.GetEmailSuffix(loginService.ServiceUserId)),
                    });
            }

            SupportedServices.Add(new LogoImage
                {
                    LogoUrl = UrlHelper.GetServiceLogoUrl(LoginServiceTypeEnum.Facebook)
                });
            SupportedServices.Add(new LogoImage
            {
                LogoUrl = UrlHelper.GetServiceLogoUrl(LoginServiceTypeEnum.Vk),
                LogoLink = "/VkLogin/AddVkAccount"
            });
            SupportedServices.Add(new LogoImage
            {
                LogoUrl = UrlHelper.GetServiceLogoUrl(LoginServiceTypeEnum.Email),
                LogoLink = "/User/LinkAccount"
            });
        }
    }

    public class LogoImage
    {
        public string LogoUrl { get; set; }
        public string LogoLink { get; set; }
    }
}