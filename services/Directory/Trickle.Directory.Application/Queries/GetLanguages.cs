﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Trickle.Directory.Infrastructure.Persistence.Queries.Context;
using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Application.Queries;

public static class GetLanguages
{
    public record Query : IRequest<List<LanguageVm>>;

    internal class Handler : IRequestHandler<Query, List<LanguageVm>>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public Handler(IQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<LanguageVm>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return _context.Languages
                .Where(l => l.TrickleLanguages.Any())
                .OrderBy(l => l.Iso6391)
                .ProjectTo<LanguageVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }

    internal class LanguageVmProfile : Profile
    {
        public LanguageVmProfile()
        {
            CreateMap<Language, LanguageVm>().ForMember(l => l.TrickleIds,
                o => o.MapFrom(l =>
                    l.TrickleLanguages.OrderBy(fll => fll.TrickleId).Select(fll => fll.TrickleId)));
        }
    }

    public record LanguageVm
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        /// <example>37</example>
        public long Id { get; private init; }

        /// <summary>
        /// The unique ISO 639-1 code.
        /// </summary>
        /// <example>en</example>
        public string Iso6391 { get; private init; } = default!;

        /// <summary>
        /// The unique ISO name.
        /// </summary>
        /// <example>English</example>
        public string Name { get; private init; } = default!;

        /// <summary>
        /// The identifiers of the Trickle targeted by this Language.
        /// </summary>
        /// <example>[ 114, 141 ]</example>
        public IEnumerable<long> TrickleIds { get; private init; } = new HashSet<long>();
    }
}
