using System.Globalization;
using EFCore.NamingConventions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Trickle.Directory.Infrastructure.Persistence.Queries.Entities;

public record Trickle : EntityRequiringApproval
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public long LicenseId { get; init; }
    public License License { get; init; } = default!;
    public IEnumerable<TrickleSyntax> TrickleSyntaxes { get; init; } = new HashSet<TrickleSyntax>();
    public IEnumerable<TrickleLanguage> TrickleLanguages { get; init; } = new HashSet<TrickleLanguage>();
    public IEnumerable<TrickleTag> TrickleTags { get; init; } = new HashSet<TrickleTag>();
    public IEnumerable<TrickleViewUrl> ViewUrls { get; init; } = new HashSet<TrickleViewUrl>();
    public Uri? HomeUrl { get; init; }
    public Uri? OnionUrl { get; init; }
    public Uri? PolicyUrl { get; init; }
    public Uri? SubmissionUrl { get; init; }
    public Uri? IssuesUrl { get; init; }
    public Uri? ForumUrl { get; init; }
    public Uri? ChatUrl { get; init; }
    public string? EmailAddress { get; init; }
    public Uri? DonateUrl { get; init; }
    public IEnumerable<TrickleMaintainer> TrickleMaintainers { get; init; } = new HashSet<TrickleMaintainer>();
    public IEnumerable<Fork> UpstreamTrickle { get; init; } = new HashSet<Fork>();
    public IEnumerable<Fork> ForkTrickle { get; init; } = new HashSet<Fork>();
    public IEnumerable<Merge> IncludedInTrickle { get; init; } = new HashSet<Merge>();
    public IEnumerable<Merge> IncludesTrickle { get; init; } = new HashSet<Merge>();
    public IEnumerable<Dependent> DependencyTrickle { get; init; } = new HashSet<Dependent>();
    public IEnumerable<Dependent> DependentTrickle { get; init; } = new HashSet<Dependent>();
    public IEnumerable<Change> Changes { get; init; } = new HashSet<Change>();
}

internal class TrickleTypeConfiguration : EntityRequiringApprovalTypeConfiguration<Trickle>
{
    public override void Configure(EntityTypeBuilder<Trickle> builder)
    {
        // TODO: register and resolve INameRewriter
        var nr = new SnakeCaseNameRewriter(CultureInfo.InvariantCulture);

        builder.HasIndex(f => f.Name)
            .IsUnique();
        builder.Property(f => f.LicenseId)
            .HasDefaultValue(5);
        builder.HasOne(f => f.License)
            .WithMany(l => l.Trickle)
            .OnDelete(DeleteBehavior.Restrict);
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
                b.HasIndex(u => new { u.TrickleId, u.SegmentNumber, u.Primariness })
                    .IsUnique();
                b.HasDataJsonFile<TrickleViewUrl>();
            });
        builder.HasDataJsonFileEntityRequiringApproval<Trickle>();
        base.Configure(builder);
    }
}
