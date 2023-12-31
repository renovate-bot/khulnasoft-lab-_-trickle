﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Maintainer : EntityRequiringApproval
{
    public string Name { get; init; } = default!;
    public Uri? Url { get; init; }
    public string? EmailAddress { get; init; }
    public string? TwitterHandle { get; init; }
    public IEnumerable<TrickleMaintainer> TrickleMaintainers { get; init; } = new HashSet<TrickleMaintainer>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class MaintainerTypeConfiguration : EntityRequiringApprovalTypeConfiguration<Maintainer>
{
    public override void Configure(EntityTypeBuilder<Maintainer> builder)
    {
        builder.HasIndex(m => m.Name)
            .IsUnique();
        builder.HasDataJsonFileEntityRequiringApproval<Maintainer>();
        base.Configure(builder);
    }
}
