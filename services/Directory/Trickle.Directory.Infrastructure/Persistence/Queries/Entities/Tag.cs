using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Tag : EntityRequiringApproval
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public IEnumerable<TrickleTag> TrickleTags { get; init; } = new HashSet<TrickleTag>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class TagTypeConfiguration : EntityRequiringApprovalTypeConfiguration<Tag>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasIndex(t => t.Name)
            .IsUnique();
        builder.HasDataJsonFileEntityRequiringApproval<Tag>();
        base.Configure(builder);
    }
}
