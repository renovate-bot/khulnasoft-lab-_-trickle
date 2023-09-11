using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Maintainers;

public class Maintainer : Entity
{
    protected Maintainer() { }

    public string Name { get; private init; } = default!;
    public Uri? Url { get; private init; }
    public string? EmailAddress { get; private init; }
    public string? TwitterHandle { get; private init; }
    public virtual IEnumerable<Trickle> Trickle { get; private init; } = new HashSet<Trickle>();
}
