using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using BrainShare.Utils.Extensions;
using BrainShare.Utils.Utilities;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy
{
    public class DefaultTenantSelector : ITenantSelector
    {

        public DefaultTenantSelector(IEnumerable<IApplicationTenant> tenants)
        {

            Ensure.Argument.NotNull(tenants, "tenants");

            this.Tenants = tenants;

        }


        public IEnumerable<IApplicationTenant> Tenants { get; private set; }


        public IApplicationTenant Select(RequestContext context)
        {

            Ensure.Argument.NotNull(context, "context");


            string baseurl = context.HttpContext.BaseUrl().TrimEnd('/');


            var valid = from tenant in this.Tenants

                        from path in tenant.UrlPaths

                        where path.Trim().TrimEnd('/').Equals(baseurl, StringComparison.OrdinalIgnoreCase)

                        select tenant;


            if (!valid.Any())

                throw new TenantNotFoundException();

            return valid.First();

        }

    }
}
