﻿using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record SoftwareSyntax
{
    public long SoftwareId { get; init; }
    public Software Software { get; init; } = default!;
    public long SyntaxId { get; init; }
    public Syntax Syntax { get; init; } = default!;
}

internal class SoftwareSyntaxTypeConfiguration : IEntityTypeConfiguration<SoftwareSyntax>
{
    public virtual void Configure(EntityTypeBuilder<SoftwareSyntax> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.ToTable($"{nr.RewriteName(nameof(SoftwareSyntax))}es");
        builder.HasKey(ss => new { ss.SoftwareId, ss.SyntaxId });
        builder.HasQueryFilter(ss => ss.Software.IsApproved && ss.Syntax.IsApproved);
        builder.HasDataJsonFile<SoftwareSyntax>();
    }
}
