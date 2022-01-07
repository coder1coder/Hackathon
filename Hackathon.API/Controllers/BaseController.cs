using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BaseController: ControllerBase
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
    }
}