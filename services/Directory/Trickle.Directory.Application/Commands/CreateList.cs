using Trickle.Directory.Domain.Aggregates.Trickle;
using Trickle.Directory.Domain.Aggregates.Languages;
using Trickle.Directory.Domain.Aggregates.Licenses;
using Trickle.Directory.Domain.Aggregates.Maintainers;
using Trickle.Directory.Domain.Aggregates.Syntaxes;
using Trickle.Directory.Domain.Aggregates.Tags;
using Trickle.Directory.Infrastructure.Persistence.Commands.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Application.Commands;

public static class CreateList
{
    public record Command : IRequest<Response>
    {
        /// <summary>
        ///     The unique name in title case.
        /// </summary>
        /// <example>EasyList</example>
        public string Name { get; init; } = default!;

        /// <summary>
        ///     The brief description in English (preferably quoted from the project).
        /// </summary>
        /// <example>EasyList is the primary filter list that removes most adverts from international web pages, including unwanted frames, images, and objects. It is the most popular list used by many ad blockers and forms the basis of over a dozen combination and supplementary filter lists.</example>
        public string? Description { get; init; }

        /// <summary>
        ///     The identifier of the License under which this Trickle is released.
        /// </summary>
        /// <example>4</example>
        public long? LicenseId { get; init; }

        /// <summary>
        ///     The identifiers of the Syntaxes implemented by this Trickle.
        /// </summary>
        /// <example>[ 3 ]</example>
        public IEnumerable<long> SyntaxIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Languages targeted by this Trickle.
        /// </summary>
        /// <example>[ 37 ]</example>
        public IEnumerable<long> LanguageIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Tags applied to this Trickle.
        /// </summary>
        /// <example>[ 2 ]</example>
        public IEnumerable<long> TagIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The view URLs.
        /// </summary>
        public IEnumerable<TrickleViewUrl> ViewUrls { get; init; } = new HashSet<TrickleViewUrl>();

        /// <summary>
        ///     The URL of the home page.
        /// </summary>
        /// <example>https://easylist.to/</example>
        public Uri? HomeUrl { get; init; }

        /// <summary>
        ///     The URL of the Tor / Onion page.
        /// </summary>
        /// <example>null</example>
        public Uri? OnionUrl { get; init; }

        /// <summary>
        ///     The URL of the policy/guidelines for the types of rules this Trickle includes.
        /// </summary>
        /// <example>null</example>
        public Uri? PolicyUrl { get; init; }

        /// <summary>
        ///     The URL of the submission/contact form for adding rules to this Trickle.
        /// </summary>
        /// <example>null</example>
        public Uri? SubmissionUrl { get; init; }

        /// <summary>
        ///     The URL of the GitHub Issues page.
        /// </summary>
        /// <example>https://github.com/easylist/easylist/issues</example>
        public Uri? IssuesUrl { get; init; }

        /// <summary>
        ///     The URL of the forum page.
        /// </summary>
        /// <example>https://forums.lanik.us/viewforum.php?f=23</example>
        public Uri? ForumUrl { get; init; }

        /// <summary>
        ///     The URL of the chat room.
        /// </summary>
        /// <example>null</example>
        public Uri? ChatUrl { get; init; }

        /// <summary>
        ///     The email address at which the project can be contacted.
        /// </summary>
        /// <example>easylist@protonmail.com</example>
        public string? EmailAddress { get; init; }

        /// <summary>
        ///     The URL at which donations to the project can be made.
        /// </summary>
        /// <example>null</example>
        public Uri? DonateUrl { get; init; }

        /// <summary>
        ///     The identifiers of the Maintainers of this Trickle.
        /// </summary>
        /// <example>[ 7 ]</example>
        public IEnumerable<long> MaintainerIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle from which this Trickle was forked.
        /// </summary>
        /// <example>[]</example>
        public IEnumerable<long> UpstreamTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle that have been forked from this Trickle.
        /// </summary>
        /// <example>[ 166, 565 ]</example>
        public IEnumerable<long> ForkTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle that include this Trickle.
        /// </summary>
        /// <example>[]</example>
        public IEnumerable<long> IncludedInTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle that this Trickle includes.
        /// </summary>
        /// <example>[ 11, 13, 168 ]</example>
        public IEnumerable<long> IncludesTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle that this Trickle depends upon.
        /// </summary>
        /// <example>[]</example>
        public IEnumerable<long> DependencyTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The identifiers of the Trickle dependent upon this Trickle.
        /// </summary>
        /// <example>[]</example>
        public IEnumerable<long> DependentTrickleIds { get; init; } = new HashSet<long>();

        /// <summary>
        ///     The reason that the Trickle is being added to Trickle.
        /// </summary>
        /// <example>Adding EasyList because I did not see it on Trickle.com yet.</example>
        public string? CreateReason { get; init; }
    }

    public record TrickleViewUrl
    {
        /// <summary>
        ///     The segment number of the URL for the Trickle (for multi-part lists).
        /// </summary>
        /// <example>1</example>
        public short SegmentNumber { get; init; }

        /// <summary>
        ///     How primary the URL is for the Trickle segment (1 is original, 2+ is a mirror; unique per SegmentNumber)
        /// </summary>
        /// <example>1</example>
        public short Primariness { get; init; }

        /// <summary>
        ///     The view URL.
        /// </summary>
        /// <example>https://easylist.to/easylist/easylist.txt</example>
        public Uri Url { get; init; } = default!;
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator(IValidator<long?> entityIdValidator, IValidator<Uri?> urlValidator)
        {
            RuleFor(c => c.LicenseId)
                .SetValidator(entityIdValidator);
            RuleForEach(c => c.SyntaxIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.SyntaxIds));
            RuleForEach(c => c.LanguageIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.LanguageIds));
            RuleForEach(c => c.TagIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.TagIds));
            RuleForEach(c => c.ViewUrls.Select(u => u.Url))
                .SetValidator(urlValidator)
                .OverridePropertyName(nameof(Command.ViewUrls) + "." + nameof(TrickleViewUrl.Url));
            RuleForEach(c => c.ViewUrls.Select(u => u.SegmentNumber))
                .GreaterThan((short)0)
                .OverridePropertyName(nameof(Command.ViewUrls) + "." + nameof(TrickleViewUrl.SegmentNumber));
            RuleForEach(c => c.ViewUrls.Select(u => u.Primariness))
                .GreaterThan((short)0)
                .OverridePropertyName(nameof(Command.ViewUrls) + "." + nameof(TrickleViewUrl.Primariness));
            RuleFor(c => c.HomeUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.OnionUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.PolicyUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.SubmissionUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.IssuesUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.ForumUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.ChatUrl)
                .SetValidator(urlValidator);
            RuleFor(c => c.DonateUrl)
                .SetValidator(urlValidator);
            RuleForEach(c => c.MaintainerIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.MaintainerIds));
            RuleForEach(c => c.UpstreamTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.UpstreamTrickleIds));
            RuleForEach(c => c.ForkTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.ForkTrickleIds));
            RuleForEach(c => c.IncludedInTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.IncludedInTrickleIds));
            RuleForEach(c => c.IncludesTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.IncludesTrickleIds));
            RuleForEach(c => c.DependencyTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.DependencyTrickleIds));
            RuleForEach(c => c.DependentTrickleIds.Select(id => (long?)id))
                .SetValidator(entityIdValidator)
                .OverridePropertyName(nameof(Command.DependentTrickleIds));
        }
    }

    internal class Handler : IRequestHandler<Command, Response>
    {
        private readonly ICommandContext _commandContext;

        public Handler(ICommandContext commandContext)
        {
            _commandContext = commandContext;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            // TODO: push applying domain rule of specified or default license into domain layer?
            var license = request.LicenseId != null
                ? await _commandContext.Licenses
                      .FindAsync(new object[] { request.LicenseId.Value }, cancellationToken)
                  ?? throw new ArgumentException($"LicenseId {request.LicenseId} not found.", nameof(request.LicenseId))
                : await _commandContext.Licenses
                    .WhereIsDefaultForTrickle()
                    .SingleAsync(cancellationToken);

            var syntaxes = request.SyntaxIds.Any()
                ? await _commandContext.Syntaxes
                    .Where(s => request.SyntaxIds.Contains(s.Id))
                    .ToListAsync(cancellationToken)
                : new List<Syntax>();
            if (request.SyntaxIds.Any(sid => !syntaxes.Select(s => s.Id).Contains(sid)))
            {
                throw new ArgumentException("One or more SyntaxIds not found.", nameof(request.SyntaxIds));
            }

            var languages = request.LanguageIds.Any()
                ? await _commandContext.Languages
                    .Where(l => request.LanguageIds.Contains(l.Id))
                    .ToListAsync(cancellationToken)
                : new List<Language>();
            if (request.LanguageIds.Any(lid => !languages.Select(l => l.Id).Contains(lid)))
            {
                throw new ArgumentException("One or more LanguageIds not found.", nameof(request.LanguageIds));
            }

            var tags = request.TagIds.Any()
                ? await _commandContext.Tags
                    .Where(t => request.TagIds.Contains(t.Id))
                    .ToListAsync(cancellationToken)
                : new List<Tag>();
            if (request.TagIds.Any(tig => !tags.Select(t => t.Id).Contains(tig)))
            {
                throw new ArgumentException("One or more TagIds not found.", nameof(request.TagIds));
            }

            var maintainers = request.MaintainerIds.Any()
                ? await _commandContext.Maintainers
                    .Where(m => request.MaintainerIds.Contains(m.Id))
                    .ToListAsync(cancellationToken)
                : new List<Maintainer>();
            if (request.MaintainerIds.Any(mid => !maintainers.Select(m => m.Id).Contains(mid)))
            {
                throw new ArgumentException("One or more MaintainerIds not found.", nameof(request.MaintainerIds));
            }

            var relatedTrickleIds = request.UpstreamTrickleIds
                .Concat(request.ForkTrickleIds)
                .Concat(request.IncludedInTrickleIds)
                .Concat(request.IncludesTrickleIds)
                .Concat(request.DependencyTrickleIds)
                .Concat(request.DependentTrickleIds)
                .ToList();
            var relatedTrickle = relatedTrickleIds.Count > 0
                ? await _commandContext.Trickle
                    .Where(f => relatedTrickleIds.Contains(f.Id))
                    .ToListAsync(cancellationToken)
                : new List<Trickle>();
            if (request.UpstreamTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more UpstreamTrickleIds not found.", nameof(request.UpstreamTrickleIds));
            }
            if (request.ForkTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more ForkTrickleIds not found.", nameof(request.ForkTrickleIds));
            }
            if (request.IncludedInTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more IncludedInTrickleIds not found.", nameof(request.IncludedInTrickleIds));
            }
            if (request.IncludesTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more IncludesTrickleIds not found.", nameof(request.IncludesTrickleIds));
            }
            if (request.DependencyTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more DependencyTrickleIds not found.", nameof(request.DependencyTrickleIds));
            }
            if (request.DependentTrickleIds.Any(fid => !relatedTrickle.Select(f => f.Id).Contains(fid)))
            {
                throw new ArgumentException("One or more DependentTrickleIds not found.", nameof(request.DependentTrickleIds));
            }

            var Trickle = Trickle.CreatePendingApproval(
                request.Name,
                request.Description,
                license,
                syntaxes,
                languages,
                tags,
                request.ViewUrls.Select(u => (u.SegmentNumber, u.Primariness, u.Url)),
                request.HomeUrl,
                request.OnionUrl,
                request.PolicyUrl,
                request.SubmissionUrl,
                request.IssuesUrl,
                request.ForumUrl,
                request.ChatUrl,
                request.EmailAddress,
                request.DonateUrl,
                maintainers,
                relatedTrickle.Where(f => request.UpstreamTrickleIds.Contains(f.Id)),
                relatedTrickle.Where(f => request.ForkTrickleIds.Contains(f.Id)),
                relatedTrickle.Where(f => request.IncludedInTrickleIds.Contains(f.Id)),
                relatedTrickle.Where(f => request.IncludesTrickleIds.Contains(f.Id)),
                relatedTrickle.Where(f => request.DependencyTrickleIds.Contains(f.Id)),
                relatedTrickle.Where(f => request.DependentTrickleIds.Contains(f.Id)),
                request.CreateReason);
            _commandContext.Trickle.Add(Trickle);

            await _commandContext.SaveChangesAsync(cancellationToken);

            return new Response
            {
                ChangeId = Trickle.Changes.Select(c => c.Id).Single()
            };
        }
    }

    public record Response
    {
        /// <summary>
        ///     The identifier of the Change for the new Trickle.
        /// </summary>
        /// <example>99</example>
        public long ChangeId { get; init; }
    }
}
