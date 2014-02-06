using System;
using Newtonsoft.Json.Linq;
using Oauth.Vk.Infrastructure.Enums;

namespace Oauth.Vk.Infrastructure.Attachments
{
    public class LinkAttachment : VkExtendedAttachment
    {

        public LinkAttachment(string url,string title,string decription,string imageSrc)
        {
            Url = url;
            Title = title;
            Description = decription;
            ImageSrc = imageSrc;
        }
        private string Url { get; set; }
        private string Title { get; set; }
        private string Description { get; set; }
        private string ImageSrc { get; set; }

        public override string MediaType
        {
            get { return VkMediaType.Link; }        
        }

        public override JObject GetValue()
        {
            var type = new JProperty("type", MediaType);
            var fields = new JObject(
                   new JProperty("url", new JValue(Url)),
                   new JProperty("title", new JValue(Title)),
                   new JProperty("description", new JValue(Description)),
                   new JProperty("image_src", new JValue(ImageSrc))
                );

            return new JObject(type, fields );

            // http://vk.com/pages?oid=-1&p=Описание_поля_attachments
        }


    }
}
