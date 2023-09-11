using AutoMapper;
using AutoMapper.QueryableExtensions;
using Trickle.Directory.Infrastructure.Persistence.Queries.Context;
using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Application.Queries;

public static class GetMaintainers
{
    public record Query : IRequest<List<MaintainerVm>>;

    internal class Handler : IRequestHandler<Query, List<MaintainerVm>>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public Handler(IQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<MaintainerVm>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return _context.Maintainers
                .OrderBy(m => m.Id)
                .ProjectTo<MaintainerVm>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }

    internal class MaintainerVmProfile : Profile
    {
        public MaintainerVmProfile()
        {
            CreateMap<Maintainer, MaintainerVm>()
                .ForMember(m => m.TrickleIds,
                    o => o.MapFrom(m =>
                        m.TrickleMaintainers.OrderBy(flm => flm.TrickleId).Select(flm => flm.TrickleId)));
        }
    }

    public record MaintainerVm
    {
        /// <summary>
        /// The identifier.
        /// </summary>
        /// <example>7</example>
        public long Id { get; init; }

        /// <summary>
        /// The unique name.
        /// </summary>
        /// <example>The EasyList Authors</example>
        public string Name { get; init; } = default!;

        /// <summary>
        /// The URL of the home page.
        /// </summary>
        /// <example>https://easylist.to/</example>
        public Uri? Url { get; init; }

        /// <summary>
        /// The email address.
        /// </summary>
        /// <example>null</example>
        public string? EmailAddress { get; init; }

        /// <summary>
        /// The Twitter handle.
        /// </summary>
        /// <example>null</example>
        public string? TwitterHandle { get; init; }

        /// <summary>
        /// The identifiers of the Trickle maintained by this Maintainer.
        /// </summary>
        /// <example>[ 11, 13, 301 ]</example>
        public IEnumerable<long> TrickleIds { get; init; } = new HashSet<long>();
    }
}
