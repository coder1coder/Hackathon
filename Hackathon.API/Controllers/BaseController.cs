using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseController: ControllerBase
    {
        protected long UserId
        {
            get {
                var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!string.IsNullOrWhiteSpace(nameIdentifier) && long.TryParse(nameIdentifier, out var userId))
                    return userId;

                return 0;
            }
        }

        protected static async Task<IActionResult> GetResult<T>(Func<Task<Result<T>>> action, HttpStatusCode successStatusCode = HttpStatusCode.OK)
        {
            var result = await action();
            return new ObjectResult(result.IsSuccess ? result.Data : result.ToProblemDetails())
            {
                StatusCode = result.IsSuccess ? (int) successStatusCode : (int) result.Errors.Type
            };
        }

        protected async Task<IActionResult> GetResult(Func<Task<Result>> action, HttpStatusCode successStatusCode = HttpStatusCode.OK)
        {
            var result = await action();

            if (result.IsSuccess)
                return Ok();

            return new ObjectResult(result.ToProblemDetails())
            {
                StatusCode = result.IsSuccess ? (int) successStatusCode : (int) result.Errors.Type
            };
        }
    }
}
