﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Language : EntityRequiringApproval
{
    public string Iso6391 { get; init; } = default!;
    public string Name { get; init; } = default!;
    public IEnumerable<TrickleLanguage> TrickleLanguages { get; init; } = new HashSet<TrickleLanguage>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class LanguageTypeConfiguration : EntityRequiringApprovalTypeConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.Property(l => l.Iso6391)
            .IsFixedLength()
            .HasMaxLength(2);
        builder.HasIndex(l => l.Iso6391)
            .IsUnique();
        builder.HasIndex(l => l.Name)
            .IsUnique();
        builder.HasDataJsonFileEntityRequiringApproval<Language>();
        base.Configure(builder);
    }
}
