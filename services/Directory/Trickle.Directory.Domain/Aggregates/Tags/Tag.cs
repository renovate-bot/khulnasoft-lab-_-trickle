using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Tags;

public class Tag : Entity
{
    protected Tag() { }

    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public virtual IEnumerable<Trickle> Trickle { get; private init; } = new HashSet<Trickle>();
}
