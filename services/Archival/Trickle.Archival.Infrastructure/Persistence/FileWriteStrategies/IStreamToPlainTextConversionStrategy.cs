using Trickle.Archival.Domain.ListArchives;

namespace Trickle.Archival.Infrastructure.Persistence.FileWriteStrategies;

internal interface IStreamToPlainTextConversionStrategy
{
    Stream Convert(ListArchiveSegment listArchiveSegment, CancellationToken cancellationToken);
}
