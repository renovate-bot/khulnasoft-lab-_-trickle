using AutoMapper;
using AutoMapper.QueryableExtensions;
using Trickle.Directory.Api.Contracts.Models;
using Trickle.Directory.Infrastructure.Persistence.Queries.Context;
using Trickle.Directory.Infrastructure.Persistence.Queries.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Trickle.Directory.Application.Queries;

public static class GetListDetails
{
    public record Query : IRequest<ListDetailsVm?>
    {
        /// <summary>
        ///     The identifier.
        /// </summary>
        /// <example>301</example>
        public long Id { get; init; }
    }

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(q => q.Id)
                .GreaterThan(0);
        }
    }

    internal class Handler : IRequestHandler<Query, ListDetailsVm?>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public Handler(IQueryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ListDetailsVm?> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            return _context.Trickle
                .ProjectTo<ListDetailsVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(fl => fl.Id == request.Id, cancellationToken);
        }
    }

    internal class ListDetailsVmProfile : Profile
    {
        public ListDetailsVmProfile()
        {
            CreateMap<Trickle, ListDetailsVm>()
                .ForMember(fl => fl.SyntaxIds,
                    o => o.MapFrom(fl =>
                        fl.TrickleSyntaxes.OrderBy(fls => fls.SyntaxId).Select(fls => fls.SyntaxId)))
                .ForMember(fl => fl.LanguageIds,
                    o => o.MapFrom(fl =>
                        fl.TrickleLanguages.OrderBy(fll => fll.LanguageId).Select(fll => fll.LanguageId)))
                .ForMember(fl => fl.TagIds,
                    o => o.MapFrom(fl =>
                        fl.TrickleTags.OrderBy(flt => flt.TagId).Select(flt => flt.TagId)))
                .ForMember(fl => fl.ViewUrls,
                    o => o.MapFrom(fl =>
                        fl.ViewUrls.OrderBy(u => u.SegmentNumber).ThenBy(u => u.Primariness)))
                .ForMember(fl => fl.MaintainerIds,
                    o => o.MapFrom(fl =>
                        fl.TrickleMaintainers.OrderBy(flm => flm.MaintainerId).Select(flm => flm.MaintainerId)))
                .ForMember(fl => fl.UpstreamTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.UpstreamTrickle.OrderBy(f => f.UpstreamTrickleId)
                            .Select(f => f.UpstreamTrickleId)))
                .ForMember(fl => fl.ForkTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.ForkTrickle.OrderBy(f => f.ForkTrickleId).Select(f => f.ForkTrickleId)))
                .ForMember(fl => fl.IncludedInTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.IncludedInTrickle.OrderBy(m => m.IncludedInTrickleId)
                            .Select(m => m.IncludedInTrickleId)))
                .ForMember(fl => fl.IncludesTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.IncludesTrickle.OrderBy(m => m.IncludesTrickleId)
                            .Select(m => m.IncludesTrickleId)))
                .ForMember(fl => fl.DependencyTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.DependencyTrickle.OrderBy(d => d.DependencyTrickleId)
                            .Select(d => d.DependencyTrickleId)))
                .ForMember(fl => fl.DependentTrickleIds,
                    o => o.MapFrom(fl =>
                        fl.DependentTrickle.OrderBy(d => d.DependentTrickleId)
                            .Select(d => d.DependentTrickleId)));

            CreateMap<TrickleViewUrl, ListDetailsVm.ViewUrlVm>();
        }
    }
}
