using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace Brainshare.Infrastructure
{
    public class Post
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string Slug { get; set; }
    }

    public class SitemapActionResult : ActionResult
    {
        private List<Post> _posts;

        public SitemapActionResult(List<Post> posts)
        {
            this._posts = posts;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", "http://site.com");

                writer.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));

                writer.WriteElementString("changefreq", "daily");
                writer.WriteElementString("priority", "1.0");
                writer.WriteEndElement();

                foreach (Post post in this._posts)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", string.Format("http://site.com/{0}", post.Slug));

                    writer.WriteElementString("lastmod", post.UpdatedOn.ToString("yyyy-MM-dd"));

                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.5");
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.Flush();
                writer.Close();
            }
        }
    }
}