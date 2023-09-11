using Trickle.Archival.Domain.ListArchives;

namespace Trickle.Archival.Infrastructure.Persistence.FileWriteStrategies;

internal class PlainText : IStreamToPlainTextConversionStrategy
{
    public Stream Convert(ListArchiveSegment listArchiveSegment, CancellationToken cancellationToken)
    {
        return listArchiveSegment.Content;
    }
}
