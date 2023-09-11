using Microsoft.AspNetCore.Mvc;

namespace Trickle.Archival.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PingController : ControllerBase
{
    /// <summary>
    ///     A sample endpoint.
    /// </summary>
    /// <returns>Pong.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public ActionResult<string> Ping()
    {
        return "pong";
    }
}
