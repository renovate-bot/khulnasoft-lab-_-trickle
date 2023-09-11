using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record License : EntityRequiringApproval
{
    public string Name { get; init; } = default!;
    public Uri? Url { get; init; }
    public bool PermitsModification { get; init; }
    public bool PermitsDistribution { get; init; }
    public bool PermitsCommercialUse { get; init; }
    public IEnumerable<Trickle> Trickle { get; init; } = new HashSet<Trickle>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class LicenseTypeConfiguration : EntityRequiringApprovalTypeConfiguration<License>
{
    public override void Configure(EntityTypeBuilder<License> builder)
    {
        builder.HasIndex(l => l.Name)
            .IsUnique();
        builder.Property(l => l.PermitsModification)
            .HasDefaultValue(false);
        builder.Property(l => l.PermitsDistribution)
            .HasDefaultValue(false);
        builder.Property(l => l.PermitsCommercialUse)
            .HasDefaultValue(false);
        builder.HasDataJsonFileEntityRequiringApproval<License>();
        base.Configure(builder);
    }
}
