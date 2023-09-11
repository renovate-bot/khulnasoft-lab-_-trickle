using Trickle.Directory.Domain.Aggregates;
using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.Directory.Domain.Aggregates.Languages;
using Trickle.Directory.Domain.Aggregates.Licenses;
using Trickle.Directory.Domain.Aggregates.Maintainers;
using Trickle.Directory.Domain.Aggregates.Software;
using Trickle.Directory.Domain.Aggregates.Syntaxes;
using Trickle.Directory.Domain.Aggregates.Tags;
using Trickle.Directory.Infrastructure.Persistence.Commands.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Trickle.Directory.Infrastructure.Persistence.Commands.Context;

internal class CommandDbContext : DbContext, ICommandContext
{
    static CommandDbContext()
    {
#pragma warning disable CS0618
        NpgsqlConnection.GlobalTypeMapper.MapEnum<AggregateType>();
#pragma warning restore CS0618
    }

    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<Trickle> Trickle => Set<Trickle>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<License> Licenses => Set<License>();
    public DbSet<Maintainer> Maintainers => Set<Maintainer>();
    public DbSet<Software> Software => Set<Software>();
    public DbSet<Syntax> Syntaxes => Set<Syntax>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType().Assembly,
            type => type.Namespace == typeof(TrickleTypeConfiguration).Namespace);
        modelBuilder.HasPostgresEnum<AggregateType>();
    }
}
