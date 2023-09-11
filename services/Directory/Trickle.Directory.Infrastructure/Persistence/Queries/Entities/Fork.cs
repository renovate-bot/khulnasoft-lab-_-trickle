using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Fork
{
    public long UpstreamTrickleId { get; init; }
    public Trickle UpstreamTrickle { get; init; } = default!;
    public long ForkTrickleId { get; init; }
    public Trickle ForkTrickle { get; init; } = default!;
}

internal class ForkTypeConfiguration : IEntityTypeConfiguration<Fork>
{
    public virtual void Configure(EntityTypeBuilder<Fork> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(Fork))}s");
        builder.HasKey(f => new { f.UpstreamTrickleId, f.ForkTrickleId });
        builder.HasOne(f => f.UpstreamTrickle)
            .WithMany(fl => fl.ForkTrickle)
            .HasForeignKey(f => f.UpstreamTrickleId)
            .HasConstraintName("fk_forks_filter_lists_upstream_filter_list_id");
        builder.HasOne(f => f.ForkTrickle)
            .WithMany(fl => fl.UpstreamTrickle)
            .HasForeignKey(f => f.ForkTrickleId)
            .HasConstraintName("fk_forks_filter_lists_fork_filter_list_id");
        builder.HasQueryFilter(f => f.UpstreamTrickle.IsApproved && f.ForkTrickle.IsApproved);
        builder.HasDataJsonFile<Fork>();
    }
}
