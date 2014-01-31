using System.Collections.Generic;
using BrainShare.Domain.Multitenancy.Features;

namespace BrainShare.Domain.Multitenancy
{
    public class ApplicationTenant
    {

        public string ApplicationName { get; set; }


        public FeatureRegistry EnabledFeatures { get; set; }


        public IEnumerable<string> UrlPaths { get; set; }

    }
}
