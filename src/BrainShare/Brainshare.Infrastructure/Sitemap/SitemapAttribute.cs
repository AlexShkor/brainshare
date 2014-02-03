using System;
using System.Web.Mvc;

namespace Brainshare.Infrastructure.Sitemap
{
    public class SitemapAttribute : ActionFilterAttribute
    {
        public SitemapFrequency Frequency { get; set; }
        public double Priority { get; set; }
        public DateTime LastModified { get; set; }

        public SitemapAttribute()
        {
            Frequency = SitemapFrequency.Daily;
            Priority = 0.5;
        }
    }
}
