using Trickle.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Trickle.Directory.Api.Controllers;

public class MaintainersController : BaseController
{
    private readonly IMediator _mediator;

    public MaintainersController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the maintainers of the Trickle.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The maintainers of the Trickle.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetMaintainers.MaintainerVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetMaintainers.Query(), cancellationToken));
    }
}
