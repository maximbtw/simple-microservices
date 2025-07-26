using Microsoft.AspNetCore.Mvc;
using PizzeriaApi.Contracts;

namespace PizzeriaApi.WebApi.Controllers;

public class ApiController : ControllerBase
{
    protected async Task<IActionResult> InvokeAsync<TRequest, TResponse>(
        TRequest request,
        Func<TRequest, Task<TResponse>> func)
        where TResponse : ApiResponseBase
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        TResponse response = await func.Invoke(request);

        return HandleResponse(response);
    }

    protected async Task<IActionResult> InvokeAsync<TResponse>(Func<Task<TResponse>> func)
        where TResponse : ApiResponseBase
    {
        TResponse response = await func.Invoke();

        return HandleResponse(response);
    }

    private IActionResult HandleResponse<TResponse>(TResponse response) where TResponse : ApiResponseBase
    {
        if (response.Ok)
        {
            return Ok(response);
        }

        if (response.Error is null)
        {
            return BadRequest(response);
        }
        
        if (response.Error.Code is ErrorCode.ServerError)
        {
            return StatusCode(500);
        }

        if (response.Error.Code is ErrorCode.NotFound)
        {
            return NotFound(response);
        }

        if (response.Error.Code is ErrorCode.AccessDenied)
        {
            return StatusCode(403);
        }

        if (response.Error.Code is ErrorCode.NotAuthorized)
        {
            return Unauthorized(response);
        }
        
        if (response.Error.Code is ErrorCode.Conflict)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Conflict",
                Detail = response.Error.Message,
                Type = response.Error.Type,
                Status = 409
            });
        }
        
        return BadRequest(response);
    }
}