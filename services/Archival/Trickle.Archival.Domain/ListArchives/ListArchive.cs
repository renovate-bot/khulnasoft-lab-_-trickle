﻿namespace Trickle.Archival.Domain.ListArchives;

public record ListArchive(long Id, IAsyncEnumerable<ListArchiveSegment> Segments);
