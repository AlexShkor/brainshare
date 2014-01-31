using System.Collections.Generic;

namespace BrainShare.Domain.Multitenancy.Features
{
    public class ComplexFeature : Feature
    {
        public IEnumerable<Feature> SubFeatures { get; set; }
    }
}
