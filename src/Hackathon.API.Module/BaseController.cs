using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.API.Module.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Module;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController: ControllerBase
{
    protected long AuthorizedUserId
    {
        get {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrWhiteSpace(nameIdentifier) && long.TryParse(nameIdentifier, out var userId))
            {
                return userId;
            }

            return 0;
        }
    }
    
    protected static Task<IActionResult> GetResult<TResult>(Func<Task<Result<TResult>>> action, HttpStatusCode successStatusCode = HttpStatusCode.OK)
        => GetResult(action, result => result, successStatusCode);

    protected static async Task<IActionResult> GetResult<TResult, TResponse>(
        Func<Task<Result<TResult>>> action, 
        Func<TResult, TResponse> responseMapper,
        HttpStatusCode successStatusCode = HttpStatusCode.OK)
    {
        var result = await action();
        
        return new ObjectResult(result.IsSuccess 
            ? responseMapper.Invoke(result.Data)
            : result.ToProblemDetails())
        {
            StatusCode = result.IsSuccess ? (int) successStatusCode : (int) result.Errors.Type
        };
    }

    protected async Task<IActionResult> GetResult(Func<Task<Result>> action, HttpStatusCode successStatusCode = HttpStatusCode.OK)
    {
        var result = await action();

        if (result.IsSuccess)
        {
            return Ok();
        }

        return new ObjectResult(result.ToProblemDetails())
        {
            StatusCode = result.IsSuccess ? (int) successStatusCode : (int) result.Errors.Type
        };
    }
}
