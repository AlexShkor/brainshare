using System.Collections.Generic;
using System.Web.Mvc;
using Brainshare.Infrastructure.Infrastructure.Multitenancy.Features;
using StructureMap;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy
{
    public interface IApplicationTenant
    {

        string ApplicationName { get; }


        IFeatureRegistry EnabledFeatures { get; }


        IEnumerable<string> UrlPaths { get; }


        IContainer DependencyContainer { get; }


        IViewEngine ViewEngine { get; }

    }
}
