using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Merge
{
    public long IncludedInTrickleId { get; init; }
    public Trickle IncludedInTrickle { get; init; } = default!;
    public long IncludesTrickleId { get; init; }
    public Trickle IncludesTrickle { get; init; } = default!;
}

internal class MergeTypeConfiguration : IEntityTypeConfiguration<Merge>
{
    public virtual void Configure(EntityTypeBuilder<Merge> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(Merge))}s");
        builder.HasKey(m => new { m.IncludedInTrickleId, m.IncludesTrickleId });
        builder.HasOne(m => m.IncludedInTrickle)
            .WithMany(fl => fl.IncludesTrickle)
            .HasForeignKey(m => m.IncludedInTrickleId)
            .HasConstraintName("fk_merges_filter_lists_included_in_filter_list_id");
        builder.HasOne(m => m.IncludesTrickle)
            .WithMany(fl => fl.IncludedInTrickle)
            .HasForeignKey(m => m.IncludesTrickleId)
            .HasConstraintName("fk_merges_filter_lists_includes_filter_list_id");
        builder.HasQueryFilter(m => m.IncludedInTrickle.IsApproved && m.IncludesTrickle.IsApproved);
        builder.HasDataJsonFile<Merge>();
    }
}
