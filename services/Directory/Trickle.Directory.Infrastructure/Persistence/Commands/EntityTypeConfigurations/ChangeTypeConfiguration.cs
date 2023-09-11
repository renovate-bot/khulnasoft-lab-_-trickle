using System.Globalization;
using EFCore.NamingConventions.Internal;
using Trickle.Directory.Domain.Aggregates;
using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.Directory.Domain.Changes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Commands.EntityTypeConfigurations;

internal class ChangeTypeConfiguration : IEntityTypeConfiguration<Change>
{
    public virtual void Configure(EntityTypeBuilder<Change> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(Change))}s");
        builder.HasDiscriminator<AggregateType>(nr.RewriteName(nameof(Queries.Entities.Change.AggregateType)))
            .HasValue<TrickleChange>(AggregateType.Trickle);
    }
}
