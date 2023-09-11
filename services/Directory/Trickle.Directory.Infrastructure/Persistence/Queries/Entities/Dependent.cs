using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Dependent
{
    public long DependencyTrickleId { get; init; }
    public Trickle DependencyTrickle { get; init; } = default!;
    public long DependentTrickleId { get; init; }
    public Trickle DependentTrickle { get; init; } = default!;
}

internal class DependentTypeConfiguration : IEntityTypeConfiguration<Dependent>
{
    public virtual void Configure(EntityTypeBuilder<Dependent> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(Dependent))}s");
        builder.HasKey(d => new { d.DependencyTrickleId, d.DependentTrickleId });
        builder.HasOne(d => d.DependencyTrickle)
            .WithMany(fl => fl.DependentTrickle)
            .HasForeignKey(d => d.DependencyTrickleId)
            .HasConstraintName("fk_dependents_filter_lists_dependency_filter_list_id");
        builder.HasOne(d => d.DependentTrickle)
            .WithMany(fl => fl.DependencyTrickle)
            .HasForeignKey(d => d.DependentTrickleId)
            .HasConstraintName("fk_dependents_filter_lists_dependent_filter_list_id");
        builder.HasQueryFilter(d => d.DependencyTrickle.IsApproved && d.DependentTrickle.IsApproved);
        builder.HasDataJsonFile<Dependent>();
    }
}
