using BrainShare.Domain.Documents.Data;

namespace BrainShare.Helpers
{
    public class UrlHelper
    {
        private const string BaseImagesPath = "/Images/socialIcons32/";
        private const string DefaultServiceLogo = "/Images/" + "email_32.png";

        public static string GetServiceLogoUrl(LoginServiceTypeEnum serviceType,string emailSuffix = null)
        {
            switch (serviceType)
            {
                case LoginServiceTypeEnum.Facebook:
                    return BaseImagesPath + "facebook_32.png";
                case LoginServiceTypeEnum.Vk:
                    return BaseImagesPath + "vk1_32.png";
                case LoginServiceTypeEnum.Email:
                    return GetEmailServiceLogoUrl(emailSuffix);
            }

            return DefaultServiceLogo;
        }

        public static string GetEmailServiceLogoUrl(string emailSuffix)
        {
            switch (emailSuffix)
            {
                case "gmail":
                    return BaseImagesPath + "google_32.png";
            }
            return DefaultServiceLogo;
        }
    }
}