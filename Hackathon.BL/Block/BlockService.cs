using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Block;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Block;
using Hackathon.Common.Models.User;
using System;
using System.Threading.Tasks;

namespace Hackathon.BL.Block;

/// <summary>
/// Сервис для работы с блокировкой
/// </summary>
public class BlockService : IBlockService
{
    private readonly IUserRepository _userRepository;
    private readonly IBlockRepository _blockRepository;

    public BlockService(
        IUserRepository userRepository, IBlockRepository blockRepository)
    {
        _userRepository = userRepository;
        _blockRepository = blockRepository;
    }

    public async Task<Result<long>> CreateAsync(BlockCreateParameters blockCreateParameters)
    {
        var administrator = await _userRepository.GetAsync(blockCreateParameters.AdministratorId);

        if (administrator?.Role != UserRole.Administrator)
            return Result<long>.NotValid(string.Format("Пользователь с Id: {0} не сущуствует или не обладает нужными правами", blockCreateParameters.AdministratorId));

        if (blockCreateParameters.Type == BlockType.Temporary
            && !blockCreateParameters.ActionDate.HasValue 
            && !blockCreateParameters.ActionHours.HasValue)
            return Result<long>.NotValid("Временная блокировка должна содержать дату, до которой действует или часы, в течение которых действует");

        if (blockCreateParameters.Type == BlockType.Permanent
            && (blockCreateParameters.ActionDate.HasValue || blockCreateParameters.ActionHours.HasValue))
            return Result<long>.NotValid("Постоянная блокировка не должна содержать дату, до которой действует или часы, в течение которых действует");

        var user = await _userRepository.GetAsync(blockCreateParameters.UserId);

        if (user is null)
            return Result<long>.NotValid("Пользователь, на которого назначается блокировка не существует");

        if (user.Role == UserRole.Administrator)
            return Result<long>.NotValid("Вы не можете назначить блокировку на администратора");

        if (user.IsBlocking(currentDateTime: DateTime.UtcNow))
            return Result<long>.NotValid("Пользователь уже заблокирован");

        var block = new BlockModel
        {
            AdministratorId = administrator.Id,
            UserId = user.Id,
            CreatedDate = DateTime.UtcNow,
            ActionDate = blockCreateParameters.ActionDate,
            ActionHours = blockCreateParameters.ActionHours,
            Reason = blockCreateParameters.Reason,
            Type = blockCreateParameters.Type,
        };

        var newBlockId = await _blockRepository.CreateAsync(block);

        return Result<long>.FromValue(newBlockId);
    }

    public async Task<bool> CheckBlocking(long userId)
    {
        var user = await _userRepository.GetAsync(userId);

        if (user.IsBlocking(currentDateTime: DateTime.UtcNow))
            return false;

        return true;
    }
}
