using System.Collections.Generic;

namespace Brainshare.Infrastructure.Infrastructure.Multitenancy.Features
{
    public interface IComplexFeature : IFeature
    {
        IEnumerable<IFeature> SubFeatures { get; }
    }
}
