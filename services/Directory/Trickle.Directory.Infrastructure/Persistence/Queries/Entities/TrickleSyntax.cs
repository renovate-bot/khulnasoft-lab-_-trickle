using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record TrickleSyntax
{
    public long TrickleId { get; init; }
    public Trickle Trickle { get; init; } = default!;
    public long SyntaxId { get; init; }
    public Syntax Syntax { get; init; } = default!;
}

internal class TrickleSyntaxTypeConfiguration : IEntityTypeConfiguration<TrickleSyntax>
{
    public virtual void Configure(EntityTypeBuilder<TrickleSyntax> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(TrickleSyntax))}es");
        builder.HasKey(fls => new { fls.TrickleId, fls.SyntaxId });
        builder.HasQueryFilter(fls => fls.Trickle.IsApproved && fls.Syntax.IsApproved);
        builder.HasDataJsonFile<TrickleSyntax>();
    }
}
