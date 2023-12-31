﻿using Trickle.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Trickle.Directory.Api.Controllers;

public class SyntaxesController : BaseController
{
    private readonly IMediator _mediator;

    public SyntaxesController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the syntaxes of the Trickle.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The syntaxes of the Trickle.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetSyntaxes.SyntaxVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetSyntaxes.Query(), cancellationToken));
    }
}
