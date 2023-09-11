using System.Globalization;
using EFCore.NamingConventions.Internal;
using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trickle = Trickle.Directory.Domain.Aggregates.Trickle.Trickle;

namespace Trickle.Directory.Infrastructure.Persistence.Commands.EntityTypeConfigurations;

internal class TrickleTypeConfiguration : IEntityTypeConfiguration<Trickle>
{
    public virtual void Configure(EntityTypeBuilder<Trickle> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.HasMany(f => f.Syntaxes)
            .WithMany(s => s.Trickle)
            .UsingEntity(
                nameof(TrickleSyntax),
                e =>
                {
                    e.ToTable($"{nr.RewriteName(nameof(TrickleSyntax))}es");
                    e.Property<long>(nameof(TrickleSyntax.TrickleId));
                    e.Property<long>(nameof(TrickleSyntax.SyntaxId));
                    e.HasKey(nameof(TrickleSyntax.TrickleId), nameof(TrickleSyntax.SyntaxId));
                });
        builder.HasMany(f => f.Languages)
            .WithMany(l => l.Trickle)
            .UsingEntity(
                nameof(TrickleLanguage),
                e =>
                {
                    e.ToTable($"{nr.RewriteName(nameof(TrickleLanguage))}s");
                    e.Property<long>(nameof(TrickleLanguage.TrickleId));
                    e.Property<long>(nameof(TrickleLanguage.LanguageId));
                    e.HasKey(nameof(TrickleLanguage.TrickleId), nameof(TrickleLanguage.LanguageId));
                });
        builder.HasMany(f => f.Tags)
            .WithMany(t => t.Trickle)
            .UsingEntity(
                nameof(TrickleTag),
                e =>
                {
                    e.ToTable($"{nr.RewriteName(nameof(TrickleTag))}s");
                    e.Property<long>(nameof(TrickleTag.TrickleId));
                    e.Property<long>(nameof(TrickleTag.TagId));
                    e.HasKey(nameof(TrickleTag.TrickleId), nameof(TrickleTag.TagId));
                });
        builder.OwnsMany(
            f => f.ViewUrls,
            b =>
            {
                b.ToTable($"{nr.RewriteName(nameof(TrickleViewUrl))}s");
                b.Property(u => u.Id)
                    .UseHiLo($"EntityFrameworkHiLoSequence-{nameof(TrickleViewUrl)}");
                b.Property(u => u.SegmentNumber)
                    .HasDefaultValue(1);
                b.Property(u => u.Primariness)
                    .HasDefaultValue(1);
                b.HasIndex(
                        nameof(TrickleViewUrl.TrickleId),
                        nameof(TrickleViewUrl.SegmentNumber),
                        nameof(TrickleViewUrl.Primariness))
                    .IsUnique();
            });
        builder.Navigation(f => f.ViewUrls)
            .AutoInclude();
        builder.HasMany(f => f.Maintainers)
            .WithMany(m => m.Trickle)
            .UsingEntity(
                nameof(TrickleMaintainer),
                e =>
                {
                    e.ToTable($"{nr.RewriteName(nameof(TrickleMaintainer))}s");
                    e.Property<long>(nameof(TrickleMaintainer.TrickleId));
                    e.Property<long>(nameof(TrickleMaintainer.MaintainerId));
                    e.HasKey(nameof(TrickleMaintainer.TrickleId), nameof(TrickleMaintainer.MaintainerId));
                });
        builder.HasMany(f => f.UpstreamTrickle)
            .WithMany(f => f.ForkTrickle)
            .UsingEntity<Dictionary<string, object>>(
                nameof(Fork),
                rj => rj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Fork.UpstreamTrickleId)),
                lj => lj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Fork.ForkTrickleId)),
                e => e.ToTable($"{nr.RewriteName(nameof(Fork))}s"));
        builder.HasMany(f => f.IncludedInTrickle)
            .WithMany(f => f.IncludesTrickle)
            .UsingEntity<Dictionary<string, object>>(
                nameof(Merge),
                rj => rj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Merge.IncludedInTrickleId)),
                lj => lj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Merge.IncludesTrickleId)),
                e => e.ToTable($"{nr.RewriteName(nameof(Merge))}s"));
        builder.HasMany(f => f.DependencyTrickle)
            .WithMany(f => f.DependentTrickle)
            .UsingEntity<Dictionary<string, object>>(
                nameof(Dependent),
                rj => rj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Dependent.DependencyTrickleId)),
                lj => lj
                    .HasOne<Trickle>()
                    .WithMany()
                    .HasForeignKey(nameof(Dependent.DependentTrickleId)),
                e => e.ToTable($"{nr.RewriteName(nameof(Dependent))}s"));
        builder.HasMany(f => f.Changes)
            .WithOne()
            .HasForeignKey(nameof(Change.TrickleId));
        builder.Navigation(f => f.Changes)
            .AutoInclude();
    }
}
