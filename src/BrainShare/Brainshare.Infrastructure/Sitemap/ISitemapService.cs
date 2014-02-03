using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Paging;

namespace Brainshare.Infrastructure.Sitemap
{
    public interface ISitemapService
    {
        string GetSitemapXml(ControllerContext controllerContext, int? page, int? count);
        IPagedEnumerable<SitemapNode> GetSitemapNodes(ControllerContext context, int? page, int? count);

        void AddNode(params SitemapNode[] nodes);
        void AddNode(IEnumerable<SitemapNode> nodes);
    }
}
