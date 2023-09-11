using Trickle.Directory.Api.Contracts.Models;
using Trickle.Directory.Application.Commands;
using Trickle.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Trickle.Directory.Api.Controllers;

public class ListsController : BaseController
{
    private readonly IMediator _mediator;

    public ListsController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the Trickle.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Trickle.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ListVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetLists.Query(), cancellationToken));
    }

#if DEBUG

    /// <summary>
    ///     ALPHA: Creates the Trickle pending moderator approval.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    [HttpPost]
    [ProducesResponseType(typeof(CreateList.Response), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Create(CreateList.Command command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken);
        return AcceptedAtAction(
            nameof(ChangesController.GetDetails),
            nameof(ChangesController).Replace("Controller", string.Empty),
            new { id = response.ChangeId });
    }

#endif

    /// <summary>
    ///     Gets the details of the Trickle.
    /// </summary>
    /// <param name="id">The identifier of the Trickle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The details of the Trickle.</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(ListDetailsVm), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<IActionResult> GetDetails(long id, CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetListDetails.Query { Id = id }, cancellationToken), id);
    }
}
