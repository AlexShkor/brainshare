using System.Collections.Generic;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy.Features
{
    public interface IFeatureRegistry
    {
        IEnumerable<IFeature> Features { get; }

        bool IsEnabled(IEnumerable<string> featurePath);
    }
}
