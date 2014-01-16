using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.Infrastructure
{
    public class VkAttachment
    {
        public VkMediaTypeEnum MediaType { get; set; }
        public string OwnerId { get; set; }
        public string MediaId { get; set; }

        public string GetValue()
        {
            return string.Format("{0}{1}_{2}", MediaType.ToString(), OwnerId, MediaId);
        }
    }
}
