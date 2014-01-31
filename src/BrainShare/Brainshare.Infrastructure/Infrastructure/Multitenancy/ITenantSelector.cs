using System.Collections.Generic;
using System.Web.Routing;
using BrainShare.Domain.Multitenancy;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy
{
    public interface ITenantSelector
    {
        IEnumerable<ApplicationTenant> Tenants { get; }

        ApplicationTenant Select(RequestContext context);
    }
}
