using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record TrickleMaintainer
{
    public long TrickleId { get; init; }
    public Trickle Trickle { get; init; } = default!;
    public long MaintainerId { get; init; }
    public Maintainer Maintainer { get; init; } = default!;
}

internal class TrickleMaintainerTypeConfiguration : IEntityTypeConfiguration<TrickleMaintainer>
{
    public virtual void Configure(EntityTypeBuilder<TrickleMaintainer> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(TrickleMaintainer))}s");
        builder.HasKey(flm => new { flm.TrickleId, flm.MaintainerId });
        builder.HasQueryFilter(flm => flm.Trickle.IsApproved && flm.Maintainer.IsApproved);
        builder.HasDataJsonFile<TrickleMaintainer>();
    }
}
