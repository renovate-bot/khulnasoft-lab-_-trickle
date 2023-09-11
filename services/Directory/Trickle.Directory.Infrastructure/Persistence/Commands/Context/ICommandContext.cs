using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.Directory.Domain.Aggregates.Languages;
using Trickle.Directory.Domain.Aggregates.Licenses;
using Trickle.Directory.Domain.Aggregates.Maintainers;
using Trickle.Directory.Domain.Aggregates.Software;
using Trickle.Directory.Domain.Aggregates.Syntaxes;
using Trickle.Directory.Domain.Aggregates.Tags;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Infrastructure.Persistence.Commands.Context;

public interface ICommandContext
{
    DbSet<Trickle> Trickle { get; }
    DbSet<Language> Languages { get; }
    DbSet<License> Licenses { get; }
    DbSet<Maintainer> Maintainers { get; }
    DbSet<Software> Software { get; }
    DbSet<Syntax> Syntaxes { get; }
    DbSet<Tag> Tags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
