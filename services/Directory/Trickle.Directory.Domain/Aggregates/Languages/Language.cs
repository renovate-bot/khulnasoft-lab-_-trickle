using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Languages;

public class Language : Entity
{
    protected Language() { }

    public string Iso6391 { get; private init; } = default!;
    public string Name { get; private init; } = default!;
    public virtual IEnumerable<Trickle> Trickle { get; private init; } = new HashSet<Trickle>();
}
