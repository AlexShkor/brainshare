using BrainShare.Domain.Documents.Data;

namespace BrainShare.Helpers
{
    public class UrlHelper
    {
        private const string BaseImagesPath = "/Images/socialIcons32/";
        public static string GetServiceLogoUrl(LoginServiceTypeEnum serviceType)
        {
            switch (serviceType)
            {
                case LoginServiceTypeEnum.Facebook:
                    return BaseImagesPath + "facebook_32.png";
            }

            return BaseImagesPath + "facebook_32.png";
        }
    }
}