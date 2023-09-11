using Trickle.Directory.Domain.Aggregates.Languages;
using Trickle.Directory.Domain.Aggregates.Licenses;
using Trickle.Directory.Domain.Aggregates.Maintainers;
using Trickle.Directory.Domain.Aggregates.Syntaxes;
using Trickle.Directory.Domain.Aggregates.Tags;
using Trickle.Directory.Domain.Changes;
using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Directory.Domain.Aggregates.Trickle;

public class Trickle : Entity, IRequireChangeApproval<TrickleChange>
{
    private readonly ICollection<TrickleChange> _changes = new HashSet<TrickleChange>();
    private readonly ICollection<Trickle> _dependencyTrickle = new HashSet<Trickle>();
    private readonly ICollection<Trickle> _dependentTrickle = new HashSet<Trickle>();
    private readonly ICollection<Trickle> _forkTrickle = new HashSet<Trickle>();
    private readonly ICollection<Trickle> _includedInTrickle = new HashSet<Trickle>();
    private readonly ICollection<Trickle> _includesTrickle = new HashSet<Trickle>();
    private readonly ICollection<Language> _languages = new HashSet<Language>();
    private readonly ICollection<Maintainer> _maintainers = new HashSet<Maintainer>();
    private readonly ICollection<Syntax> _syntaxes = new HashSet<Syntax>();
    private readonly ICollection<Tag> _tags = new HashSet<Tag>();
    private readonly ICollection<Trickle> _upstreamTrickle = new HashSet<Trickle>();
    private readonly ICollection<TrickleViewUrl> _viewUrls = new HashSet<TrickleViewUrl>();

    protected Trickle() { }

    public string Name { get; private init; } = default!;
    public string? Description { get; private init; }
    public virtual License License { get; private init; } = default!;

    public virtual IEnumerable<Syntax> Syntaxes
    {
        get => _syntaxes;
        private init => _syntaxes = new HashSet<Syntax>(value);
    }

    public virtual IEnumerable<Language> Languages
    {
        get => _languages;
        private init => _languages = new HashSet<Language>(value);
    }

    public virtual IEnumerable<Tag> Tags
    {
        get => _tags;
        private init => _tags = new HashSet<Tag>(value);
    }

    public virtual IEnumerable<TrickleViewUrl> ViewUrls
    {
        get => _viewUrls;
        private init => _viewUrls = new HashSet<TrickleViewUrl>(value);
    }

    public Uri? HomeUrl { get; private init; }
    public Uri? OnionUrl { get; private init; }
    public Uri? PolicyUrl { get; private init; }
    public Uri? SubmissionUrl { get; private init; }
    public Uri? IssuesUrl { get; private init; }
    public Uri? ForumUrl { get; private init; }
    public Uri? ChatUrl { get; private init; }
    public string? EmailAddress { get; private init; }
    public Uri? DonateUrl { get; private init; }

    public virtual IEnumerable<Maintainer> Maintainers
    {
        get => _maintainers;
        private init => _maintainers = new HashSet<Maintainer>(value);
    }

    public virtual IEnumerable<Trickle> UpstreamTrickle
    {
        get => _upstreamTrickle;
        private init => _upstreamTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<Trickle> ForkTrickle
    {
        get => _forkTrickle;
        private init => _forkTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<Trickle> IncludedInTrickle
    {
        get => _includedInTrickle;
        private init => _includedInTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<Trickle> IncludesTrickle
    {
        get => _includesTrickle;
        private init => _includesTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<Trickle> DependencyTrickle
    {
        get => _dependencyTrickle;
        private init => _dependencyTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<Trickle> DependentTrickle
    {
        get => _dependentTrickle;
        private init => _dependentTrickle = new HashSet<Trickle>(value);
    }

    public virtual IEnumerable<TrickleChange> Changes => _changes;
    public bool IsApproved { get; private init; }

    public static Trickle CreatePendingApproval(
        string name,
        string? description,
        License license,
        IEnumerable<Syntax> syntaxes,
        IEnumerable<Language> languages,
        IEnumerable<Tag> tags,
        IEnumerable<(short SegmentNumber, short Primariness, Uri Url)> viewUrls,
        Uri? homeUrl,
        Uri? onionUrl,
        Uri? policyUrl,
        Uri? submissionUrl,
        Uri? issuesUrl,
        Uri? forumUrl,
        Uri? chatUrl,
        string? emailAddress,
        Uri? donateUrl,
        IEnumerable<Maintainer> maintainers,
        IEnumerable<Trickle> upstreamTrickle,
        IEnumerable<Trickle> forkTrickle,
        IEnumerable<Trickle> includedInTrickle,
        IEnumerable<Trickle> includesTrickle,
        IEnumerable<Trickle> dependencyTrickle,
        IEnumerable<Trickle> dependentTrickle,
        string? createReason)
    {
        var urls = viewUrls.DistinctBy(u => new { u.SegmentNumber, u.Primariness, u.Url })
            .Select(u => TrickleViewUrl.Create(u.SegmentNumber, u.Primariness, u.Url))
            .ToList();
        if (urls.Count == 0)
        {
            throw new ArgumentException("At lest one view URL is required.", nameof(viewUrls));
        }

        if (urls.GroupBy(u => new { u.SegmentNumber, u.Primariness }).Any(g => g.Count() > 1))
        {
            throw new ArgumentException("The segment number and primariness pair must be unique for each view URL.", nameof(viewUrls));
        }

        var upstreamTrickleSet = new HashSet<Trickle>(upstreamTrickle);
        var forkTrickleSet = new HashSet<Trickle>(forkTrickle);
        if (upstreamTrickleSet.Intersect(forkTrickleSet).Any())
        {
            throw new ArgumentException("A Trickle cannot be both an Upstream and a Fork of the same Trickle.");
        }

        var includedInTrickleSet = new HashSet<Trickle>(includedInTrickle);
        var includesTrickleSet = new HashSet<Trickle>(includesTrickle);
        if (includedInTrickleSet.Intersect(includesTrickleSet).Any())
        {
            throw new ArgumentException("A Trickle cannot be both included in and including of the same Trickle.");
        }

        var dependencyTrickleSet = new HashSet<Trickle>(dependencyTrickle);
        var dependentTrickleSet = new HashSet<Trickle>(dependentTrickle);
        if (dependencyTrickleSet.Intersect(dependentTrickleSet).Any())
        {
            throw new ArgumentException("A Trickle cannot be both a dependency of and dependent upon the same Trickle.");
        }

        var list = new Trickle
        {
            Name = name,
            Description = description,
            License = license,
            Syntaxes = syntaxes,
            Languages = languages,
            Tags = tags,
            ViewUrls = urls,
            HomeUrl = homeUrl,
            OnionUrl = onionUrl,
            PolicyUrl = policyUrl,
            SubmissionUrl = submissionUrl,
            IssuesUrl = issuesUrl,
            ForumUrl = forumUrl,
            ChatUrl = chatUrl,
            EmailAddress = emailAddress,
            DonateUrl = donateUrl,
            Maintainers = maintainers,
            UpstreamTrickle = upstreamTrickleSet,
            ForkTrickle = forkTrickleSet,
            IncludedInTrickle = includedInTrickleSet,
            IncludesTrickle = includesTrickleSet,
            DependencyTrickle = dependencyTrickleSet,
            DependentTrickle = dependentTrickleSet,
            IsApproved = false
        };
        list._changes.Add(TrickleChange.Create(list, createReason));
        return list;
    }
}
