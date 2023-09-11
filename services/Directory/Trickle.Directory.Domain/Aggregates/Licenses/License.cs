using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Licenses;

public class License : Entity
{
    protected License() { }

    public string Name { get; private init; } = default!;
    public Uri? Url { get; private init; }
    public bool PermitsModification { get; private init; }
    public bool PermitsDistribution { get; private init; }
    public bool PermitsCommercialUse { get; private init; }
    public virtual IEnumerable<Trickle> Trickle { get; private init; } = new HashSet<Trickle>();
}
