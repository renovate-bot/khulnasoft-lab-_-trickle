using Trickle.Directory.Domain.Aggregates.Trickle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Change = Trickle.Directory.Infrastructure.Persistence.Queries.Entities.Change;

namespace Trickle.Directory.Infrastructure.Persistence.Commands.EntityTypeConfigurations;

internal class TrickleChangeTypeConfiguration : IEntityTypeConfiguration<TrickleChange>
{
    public virtual void Configure(EntityTypeBuilder<TrickleChange> builder)
    {
        builder.Property<long>(nameof(Change.TrickleId));
        builder.Property(c => c.Before)
            .HasColumnType("jsonb");
        builder.Property(c => c.After)
            .HasColumnType("jsonb");
    }
}
