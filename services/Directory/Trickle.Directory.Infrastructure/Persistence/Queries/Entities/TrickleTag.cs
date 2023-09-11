using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record TrickleTag
{
    public long TrickleId { get; init; }
    public Trickle Trickle { get; init; } = default!;
    public long TagId { get; init; }
    public Tag Tag { get; init; } = default!;
}

internal class TrickleTagTypeConfiguration : IEntityTypeConfiguration<TrickleTag>
{
    public virtual void Configure(EntityTypeBuilder<TrickleTag> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(TrickleTag))}s");
        builder.HasKey(flt => new { flt.TrickleId, flt.TagId });
        builder.HasQueryFilter(flt => flt.Trickle.IsApproved && flt.Tag.IsApproved);
        builder.HasDataJsonFile<TrickleTag>();
    }
}
