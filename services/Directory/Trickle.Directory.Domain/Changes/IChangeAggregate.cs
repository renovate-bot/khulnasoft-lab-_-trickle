using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Changes;

public interface IChangeAggregate<out TAggregateValueObject> where TAggregateValueObject : ValueObject
{
    TAggregateValueObject? Before { get; }
    TAggregateValueObject? After { get; }
}
