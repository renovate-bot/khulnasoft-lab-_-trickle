using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Trickle;

public class TrickleValueObject : ValueObject
{
    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public long LicenseId { get; private init; }
    public IEnumerable<long> SyntaxIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> LanguageIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> TagIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> ViewUrlIds { get; private init; } = new HashSet<long>();
    public Uri? HomeUrl { get; private init; }
    public Uri? OnionUrl { get; private init; }
    public Uri? PolicyUrl { get; private init; }
    public Uri? SubmissionUrl { get; private init; }
    public Uri? IssuesUrl { get; private init; }
    public Uri? ForumUrl { get; private init; }
    public Uri? ChatUrl { get; private init; }
    public string? EmailAddress { get; private init; }
    public Uri? DonateUrl { get; private init; }
    public IEnumerable<long> MaintainerIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> UpstreamTrickleIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> ForkTrickleIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> IncludedInTrickleIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> IncludesTrickleIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> DependencyTrickleIds { get; private init; } = new HashSet<long>();
    public IEnumerable<long> DependentTrickleIds { get; private init; } = new HashSet<long>();

    public static TrickleValueObject FromTrickle(Trickle Trickle)
    {
        return new TrickleValueObject
        {
            Name = Trickle.Name,
            Description = Trickle.Description,
            LicenseId = Trickle.License.Id,
            SyntaxIds = Trickle.Syntaxes.Select(s => s.Id),
            LanguageIds = Trickle.Languages.Select(l => l.Id),
            TagIds = Trickle.Tags.Select(t => t.Id),
            ViewUrlIds = Trickle.ViewUrls.Select(u => u.Id),
            HomeUrl = Trickle.HomeUrl,
            OnionUrl = Trickle.OnionUrl,
            PolicyUrl = Trickle.PolicyUrl,
            SubmissionUrl = Trickle.SubmissionUrl,
            IssuesUrl = Trickle.IssuesUrl,
            ForumUrl = Trickle.ForumUrl,
            ChatUrl = Trickle.ChatUrl,
            EmailAddress = Trickle.EmailAddress,
            DonateUrl = Trickle.DonateUrl,
            MaintainerIds = Trickle.Maintainers.Select(m => m.Id),
            UpstreamTrickleIds = Trickle.UpstreamTrickle.Select(f => f.Id),
            ForkTrickleIds = Trickle.ForkTrickle.Select(f => f.Id),
            IncludedInTrickleIds = Trickle.IncludedInTrickle.Select(f => f.Id),
            IncludesTrickleIds = Trickle.IncludesTrickle.Select(f => f.Id),
            DependencyTrickleIds = Trickle.DependencyTrickle.Select(f => f.Id),
            DependentTrickleIds = Trickle.DependentTrickle.Select(f => f.Id)
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
        yield return LicenseId;
        foreach (var id in SyntaxIds)
        {
            yield return id;
        }

        foreach (var id in LanguageIds)
        {
            yield return id;
        }

        foreach (var id in TagIds)
        {
            yield return id;
        }

        foreach (var id in ViewUrlIds)
        {
            yield return id;
        }

        yield return HomeUrl;
        yield return OnionUrl;
        yield return PolicyUrl;
        yield return SubmissionUrl;
        yield return IssuesUrl;
        yield return ForumUrl;
        yield return ChatUrl;
        yield return EmailAddress;
        yield return DonateUrl;
        foreach (var id in MaintainerIds)
        {
            yield return id;
        }

        foreach (var id in UpstreamTrickleIds)
        {
            yield return id;
        }

        foreach (var id in ForkTrickleIds)
        {
            yield return id;
        }

        foreach (var id in IncludedInTrickleIds)
        {
            yield return id;
        }

        foreach (var id in IncludesTrickleIds)
        {
            yield return id;
        }

        foreach (var id in DependencyTrickleIds)
        {
            yield return id;
        }

        foreach (var id in DependentTrickleIds)
        {
            yield return id;
        }
    }
}
