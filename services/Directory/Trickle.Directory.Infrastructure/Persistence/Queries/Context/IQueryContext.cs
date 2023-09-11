using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Context;

public interface IQueryContext
{
    IQueryable<Change> Changes { get; }
    IQueryable<Trickle> Trickle { get; }
    IQueryable<Language> Languages { get; }
    IQueryable<License> Licenses { get; }
    IQueryable<Maintainer> Maintainers { get; }
    IQueryable<Software> Software { get; }
    IQueryable<Syntax> Syntaxes { get; }
    IQueryable<Tag> Tags { get; }
}
