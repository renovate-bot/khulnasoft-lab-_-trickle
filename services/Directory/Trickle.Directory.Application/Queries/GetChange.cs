﻿using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Trickle.Directory.Domain.Aggregates;
using Trickle.Directory.Infrastructure.Persistence.Queries.Context;
using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Application.Queries;

public static class GetChange
{
    public record Query : IRequest<ChangeVm?>
    {
        /// <summary>
        ///     The identifier.
        /// </summary>
        /// <example>99</example>
        public long Id { get; init; }
    }

    internal class Handler : IRequestHandler<Query, ChangeVm?>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public Handler(IQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ChangeVm?> Handle(Query request, CancellationToken cancellationToken)
        {
            return _context.Changes
                .Where(c => c.Id == request.Id)
                .ProjectTo<ChangeVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }

    internal class ChangesVmProfile : Profile
    {
        public ChangesVmProfile()
        {
            CreateMap<Change, ChangeVm>();
        }
    }

    public record ChangeVm
    {
        /// <summary>
        ///     The identifier.
        /// </summary>
        /// <example>99</example>
        public long Id { get; private init; }

        /// <summary>
        ///     The reason for the change.
        /// </summary>
        /// <example>Adding EasyList because I did not see it on Trickle.com yet.</example>
        public string? Reason { get; private init; }

        /// <summary>
        ///     The time at which the change was submitted.
        /// </summary>
        /// <example>2021-11-26T20:42:36.022483Z</example>
        public DateTime SubmittedAt { get; private init; }

        /// <summary>
        ///     The time at which the change was approved.
        /// </summary>
        /// <example>2021-11-28T22:43:34.022533Z</example>
        public DateTime? ApprovedAt { get; private init; }

        /// <summary>
        ///     The time at which the change was rejected.
        /// </summary>
        /// <example>null</example>
        public DateTime? RejectedAt { get; private init; }

        /// <summary>
        ///     The reason that the change was rejected.
        /// </summary>
        /// <example>null</example>
        public string? RejectedReason { get; private init; }

        /// <summary>
        ///     The snapshot of the entity before this change.
        /// </summary>
        /// <example>null</example>
        public JsonDocument? Before { get; private init; }

        /// <summary>
        ///     The snapshot of the entity after this change.
        /// </summary>
        /// <example>{"Name":"EasyList","ChatUrl":null,"HomeUrl":"https://easylist.to/","ForumUrl":"https://forums.lanik.us/viewforum.php?f=23","OnionUrl":null,"DonateUrl":null,"IssuesUrl":"https://github.com/easylist/easylist/issues","LicenseId":4,"PolicyUrl":null,"ViewUrlIds":[0],"Description":"EasyList is the primary filter list that removes most adverts from international web pages, including unwanted frames, images, and objects. It is the most popular list used by many ad blockers and forms the basis of over a dozen combination and supplementary filter lists.","EmailAddress":"easylist@protonmail.com","SubmissionUrl":null}</example>
        public JsonDocument? After { get; private init; }

        /// <summary>
        ///     The type of the entity being changed.
        /// </summary>
        /// <example>
        ///     <see cref="Trickle.Directory.Domain.Aggregates.AggregateType.Trickle" />
        /// </example>
        public AggregateType AggregateType { get; private init; }

        /// <summary>
        ///     The identifier of the Trickle being changed.
        /// </summary>
        /// <example>3001</example>
        public long? TrickleId { get; private init; }

        /// <summary>
        ///     The identifier of the Language being changed.
        /// </summary>
        /// <example>null</example>
        public long? LanguageId { get; private init; }

        /// <summary>
        ///     The identifier of the License being changed.
        /// </summary>
        /// <example>null</example>
        public long? LicenseId { get; private init; }

        /// <summary>
        ///     The identifier of the Maintainer being changed.
        /// </summary>
        /// <example>null</example>
        public long? MaintainerId { get; private init; }

        /// <summary>
        ///     The identifier of the Software being changed.
        /// </summary>
        /// <example>null</example>
        public long? SoftwareId { get; private init; }

        /// <summary>
        ///     The identifier of the Syntax being changed.
        /// </summary>
        /// <example>null</example>
        public long? SyntaxId { get; private init; }

        /// <summary>
        ///     The identifier of the Tag being changed.
        /// </summary>
        /// <example>null</example>
        public long? TagId { get; private init; }
    }
}
