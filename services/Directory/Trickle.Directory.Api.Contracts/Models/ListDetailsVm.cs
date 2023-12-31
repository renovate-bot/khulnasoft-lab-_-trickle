﻿namespace Trickle.Directory.Api.Contracts.Models;

public record ListDetailsVm
{
    /// <summary>
    ///     The identifier.
    /// </summary>
    /// <example>301</example>
    public long Id { get; init; }

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
    public long LicenseId { get; init; }

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
    public IEnumerable<ViewUrlVm> ViewUrls { get; init; } = new HashSet<ViewUrlVm>();

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

    public record ViewUrlVm
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
}
