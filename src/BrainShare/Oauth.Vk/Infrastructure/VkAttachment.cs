namespace Oauth.Vk.Infrastructure
{
    public class VkAttachment
    {
        public string MediaType { get; set; }
        public string OwnerId { get; set; }
        public string MediaId { get; set; }

        public string GetValue()
        {
            return string.Format("{0}{1}_{2}", MediaType, OwnerId, MediaId);
        }
    }
}
