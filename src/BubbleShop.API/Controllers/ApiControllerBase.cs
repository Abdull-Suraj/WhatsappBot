using Microsoft.AspNetCore.Mvc;
using MediatR;
using BubbleShop.Application.Common.Models;

namespace BubbleShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return result.Errors.Count > 1
            ? BadRequest(new { errors = result.Errors })
            : NotFound(new { error = result.Error });
    }

    protected IActionResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(new { errors = result.Errors });
    }
}
