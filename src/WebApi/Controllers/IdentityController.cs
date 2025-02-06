using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Contracts;
using WebApi.RequestModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public async Task<IActionResult> AuthenticateRequest([FromBody] string username,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was cancelled by client");

        await _identityService.AuthenticateRequestAsync(username);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticateRequestModel model,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return StatusCode(StatusCodes.Status499ClientClosedRequest, "Request was cancelled by client");

        var token = await _identityService.AuthenticateAsync(model.Username, model.Code)!;

        return token;
    }
}