using System.Collections.Generic;
using System.Web.Routing;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy
{
    public interface ITenantSelector
    {
        IEnumerable<IApplicationTenant> Tenants { get; }

        IApplicationTenant Select(RequestContext context);
    }
}
