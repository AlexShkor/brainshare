using System.Collections.Generic;

namespace Brainshare.Infrastructure.Platform.ViewServices
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            PagingInfo = new PagingInfo();
        }

        public List<T> Items { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
