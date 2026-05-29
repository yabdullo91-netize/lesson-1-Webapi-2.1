using Microsoft.AspNetCore.Mvc;
using Application.Common;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if (result.Value == null) return NotFound();
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }
}
