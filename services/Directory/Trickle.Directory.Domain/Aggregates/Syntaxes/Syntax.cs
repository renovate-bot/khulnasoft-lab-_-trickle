using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Syntaxes;

public class Syntax : Entity
{
    protected Syntax() { }

    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public Uri? Url { get; private init; }
    public virtual IEnumerable<Trickle> Trickle { get; private init; } = new HashSet<Trickle>();
    public virtual IEnumerable<Software.Software> Software { get; private init; } = new HashSet<Software.Software>();
}
