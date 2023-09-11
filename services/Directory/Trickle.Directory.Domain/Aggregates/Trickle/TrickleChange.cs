using Trickle.Directory.Domain.Changes;

namespace Trickle.Directory.Domain.Aggregates.Trickle;

public class TrickleChange : Change, IChangeAggregate<TrickleValueObject>
{
    protected TrickleChange() { }

    public TrickleValueObject? Before { get; private init; }
    public TrickleValueObject? After { get; private init; }

    public static TrickleChange Create(Trickle Trickle, string? reason)
    {
        return new TrickleChange { After = TrickleValueObject.FromTrickle(Trickle), Reason = reason };
    }

    public static TrickleChange Update(Trickle before, Trickle after, string? reason)
    {
        return new TrickleChange
        {
            Before = TrickleValueObject.FromTrickle(before),
            After = TrickleValueObject.FromTrickle(after),
            Reason = reason
        };
    }

    public static TrickleChange Delete(Trickle Trickle, string? reason)
    {
        return new TrickleChange { Before = TrickleValueObject.FromTrickle(Trickle), Reason = reason };
    }
}
