using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Syntax : EntityRequiringApproval
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public Uri? Url { get; init; }
    public IEnumerable<TrickleSyntax> TrickleSyntaxes { get; init; } = new HashSet<TrickleSyntax>();
    public IEnumerable<SoftwareSyntax> SoftwareSyntaxes { get; init; } = new HashSet<SoftwareSyntax>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class SyntaxTypeConfiguration : EntityRequiringApprovalTypeConfiguration<Syntax>
{
    public override void Configure(EntityTypeBuilder<Syntax> builder)
    {
        builder.HasIndex(s => s.Name)
            .IsUnique();
        builder.HasDataJsonFileEntityRequiringApproval<Syntax>();
        base.Configure(builder);
    }
}
