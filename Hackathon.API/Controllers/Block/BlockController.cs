using Hackathon.API.Module;
using Hackathon.Common.Abstraction.Block;
using Hackathon.Common.Models.Block;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.Block;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers.Block;

[SwaggerTag("Блокировки")]
[Authorize(Policy = nameof(UserRole.Administrator))]
public class BlockController : BaseController
{
    private readonly IBlockService _blockService;

    public BlockController(IBlockService blockService)
    {
        _blockService = blockService;
    }

    /// <summary>
    /// Создание новой блокировки
    /// </summary>
    /// <param name="createBlockRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseCreateResponse), (int)HttpStatusCode.OK)]
    public Task<IActionResult> GetList([FromBody] CreateBlockRequest createBlockRequest)
        => GetResult(() => _blockService.CreateAsync(new BlockCreateParameters
        {
            UserId = createBlockRequest.UserId,
            ActionDate = createBlockRequest.ActionDate,
            ActionHours = createBlockRequest.ActionHours,
            Reason = createBlockRequest.Reason,
            Type = createBlockRequest.Type,
            AdministratorId = AuthorizedUserId
        }));
}
