﻿using Trickle.Directory.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Trickle.Directory.Api.Controllers;

public class LicensesController : BaseController
{
    private readonly IMediator _mediator;

    public LicensesController(IMemoryCache cache, IMediator mediator) : base(cache)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Gets the licenses applied to the Trickle.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The licenses applied to the Trickle.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetLicenses.LicenseVm>), StatusCodes.Status200OK)]
    public Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return CacheGetOrCreateAsync(() => _mediator.Send(new GetLicenses.Query(), cancellationToken));
    }
}
