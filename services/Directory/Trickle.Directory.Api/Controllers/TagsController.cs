using Trickle.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Trickle.Directory.Api.Controllers;

public class TagsController : BaseController
{
    private readonly IMediator _mediator;

    public TagsController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the tags of the Trickle.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tags of the Trickle.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetTags.TagVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetTags.Query(), cancellationToken));
    }
}
