using Hackathon.Common.Abstraction.Block;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hackathon.API.Module;

public class BlockingHandler : AuthorizationHandler<BlockingRequirement>
{
    private readonly IBlockService _blockService;

    public BlockingHandler(IBlockService blockService)
    {
        _blockService = blockService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BlockingRequirement requirement)
    {
        var nameIdentifier = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrWhiteSpace(nameIdentifier) && long.TryParse(nameIdentifier, out var userId))
        {
            var result = await _blockService.CheckBlocking(userId);

            if (!result)
                context.Fail();
        }

        context.Succeed(requirement);
    }
}
