using System.Collections.Generic;

namespace BrainShare.Domain.Multitenancy.Features
{
    public class FeatureRegistry
    {
        public IEnumerable<Feature> Features { get; set; }

        public bool IsEnabled(IEnumerable<string> featurePath)
        {
            return false;
        }
    }
}
