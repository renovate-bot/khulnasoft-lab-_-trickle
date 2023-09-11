using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Trickle;

public class TrickleViewUrl : Entity
{
    protected TrickleViewUrl() { }

    public short SegmentNumber { get; private init; }
    public short Primariness { get; private init; }
    public Uri Url { get; private init; } = default!;

    internal static TrickleViewUrl Create(short segmentNumber, short primariness, Uri url)
    {
        if (segmentNumber < 1)
        {
            throw new ArgumentException(
                "The segment number must be greater than or equal to 1.",
                nameof(segmentNumber));
        }

        if (primariness < 1)
        {
            throw new ArgumentException(
                "The primariness must be greater than or equal to 1.",
                nameof(segmentNumber));
        }

        return new TrickleViewUrl { SegmentNumber = segmentNumber, Primariness = primariness, Url = url };
    }
}
