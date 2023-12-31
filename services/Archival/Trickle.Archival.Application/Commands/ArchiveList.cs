﻿using System.Runtime.CompilerServices;
using Trickle.Archival.Domain.ListArchives;
using Trickle.Archival.Infrastructure.Clients;
using Trickle.Directory.Api.Contracts;
using Trickle.Directory.Api.Contracts.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Trickle.Archival.Application.Commands;

public static class ArchiveList
{
    public record Command(long ListId) : IRequest;

    internal class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IHttpContentClient _client;
        private readonly IDirectoryApi _directory;
        private readonly ILogger _logger;
        private readonly IListArchiveRepository _repo;

        public Handler(
            IHttpContentClient httpContentClient,
            IDirectoryApi directoryApi,
            ILogger<Handler> logger,
            IListArchiveRepository listArchiveRepository)
        {
            _client = httpContentClient;
            _directory = directoryApi;
            _logger = logger;
            _repo = listArchiveRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Archiving list {ListId}", request.ListId);

            var segmentUrls = (await GetSegmentUrlsAsync(request.ListId, cancellationToken)).ToList();
            if (segmentUrls.Count > 0)
            {
                var list = GetList(request.ListId, segmentUrls, cancellationToken);
                await _repo.AddAsync(list, cancellationToken);
                _repo.Commit();

                _logger.LogInformation(
                    "Archived segment(s) {@SegmentNumbers} of list {ListId}",
                    segmentUrls.Select(s => s.SegmentNumber),
                    request.ListId);
            }
            else
            {
                _logger.LogWarning("List {ListId} has no URLs to archive", request.ListId);
            }

            return Unit.Value;
        }

        private async Task<IEnumerable<ListDetailsVm.ViewUrlVm>> GetSegmentUrlsAsync(
            long listId,
            CancellationToken cancellationToken)
        {
            var listDetails = await _directory.GetListDetailsAsync(listId, cancellationToken);
            return listDetails.ViewUrls?
                       .GroupBy(u => u.SegmentNumber, (_, ue) => ue.OrderBy(u => u.Primariness).First()) ??
                   new List<ListDetailsVm.ViewUrlVm>();
        }

        private ListArchive GetList(
            long listId,
            IEnumerable<ListDetailsVm.ViewUrlVm> segmentUrls,
            CancellationToken cancellationToken)
        {
            var segmentsAsync = GetSegmentsAsync(segmentUrls, cancellationToken);
            return new ListArchive(listId, segmentsAsync);
        }

        private async IAsyncEnumerable<ListArchiveSegment> GetSegmentsAsync(
            IEnumerable<ListDetailsVm.ViewUrlVm> segmentUrls,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var segment in segmentUrls)
            {
                var content = await _client.GetContentAsync(segment.Url, cancellationToken);
                if (content != Stream.Null)
                {
                    yield return new ListArchiveSegment(segment.Url, content);
                }
            }
        }
    }
}
