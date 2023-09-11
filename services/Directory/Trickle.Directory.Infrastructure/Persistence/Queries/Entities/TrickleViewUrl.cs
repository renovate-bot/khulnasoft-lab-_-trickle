namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record TrickleViewUrl
{
    public long Id { get; init; }
    public long TrickleId { get; init; }
    public Trickle Trickle { get; init; } = default!;
    public short SegmentNumber { get; init; }
    public short Primariness { get; init; }
    public Uri Url { get; init; } = default!;
}
