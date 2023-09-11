using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record TrickleLanguage
{
    public long TrickleId { get; init; }
    public Trickle Trickle { get; init; } = default!;
    public long LanguageId { get; init; }
    public Language Language { get; init; } = default!;
}

internal class TrickleLanguageTypeConfiguration : IEntityTypeConfiguration<TrickleLanguage>
{
    public virtual void Configure(EntityTypeBuilder<TrickleLanguage> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(TrickleLanguage))}s");
        builder.HasKey(fll => new { fll.TrickleId, fll.LanguageId });
        builder.HasQueryFilter(fll => fll.Trickle.IsApproved && fll.Language.IsApproved);
        builder.HasDataJsonFile<TrickleLanguage>();
    }
}
