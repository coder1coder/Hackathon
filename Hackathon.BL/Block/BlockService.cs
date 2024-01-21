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
public class BlockService : IBlockingService
{
    private readonly IUserRepository _userRepository;
    private readonly IBlockingRepository _blockRepository;

    public BlockService(
        IUserRepository userRepository, IBlockingRepository blockRepository)
    {
        _userRepository = userRepository;
        _blockRepository = blockRepository;
    }

    public async Task<Result<long>> CreateAsync(BlockingCreateParameters blockCreateParameters)
    {
        var administrator = await _userRepository.GetAsync(blockCreateParameters.AssignmentUserId);

        if (administrator?.Role != UserRole.Administrator)
            return Result<long>.NotValid(string.Format("Пользователь с Id: {0} не сущуствует или не обладает нужными правами", blockCreateParameters.AssignmentUserId));

        if (blockCreateParameters.Type == BlockingType.Temporary
            && !blockCreateParameters.ActionDate.HasValue 
            && !blockCreateParameters.ActionHours.HasValue)
            return Result<long>.NotValid("Временная блокировка должна содержать дату, до которой действует или часы, в течение которых действует");

        if (blockCreateParameters.Type == BlockingType.Permanent
            && (blockCreateParameters.ActionDate.HasValue || blockCreateParameters.ActionHours.HasValue))
            return Result<long>.NotValid("Постоянная блокировка не должна содержать дату, до которой действует или часы, в течение которых действует");

        var user = await _userRepository.GetAsync(blockCreateParameters.TargetUserId);

        if (user is null)
            return Result<long>.NotValid("Пользователь, на которого назначается блокировка не существует");

        if (user.Role == UserRole.Administrator)
            return Result<long>.NotValid("Вы не можете назначить блокировку на администратора");

        if (user.IsBlocking)
            return Result<long>.NotValid("Пользователь уже заблокирован");

        var block = new BlockingModel(
            type: blockCreateParameters.Type,
            reason: blockCreateParameters.Reason,
            assignmentUserId: blockCreateParameters.AssignmentUserId,
            targetUserId: blockCreateParameters.TargetUserId,
            actionDate: blockCreateParameters.ActionDate,
            actionHours: blockCreateParameters.ActionHours);

        var newBlockId = await _blockRepository.CreateAsync(block);

        return Result<long>.FromValue(newBlockId);
    }

    public async Task<bool> CheckBlocking(long userId)
    {
        var user = await _userRepository.GetAsync(userId);

        if (user.IsBlocking)
            return false;

        return true;
    }

    public async Task<Result<bool>> DeleteAsync(long removeUserId, long targetUserId)
    {
        var administrator = await _userRepository.GetAsync(removeUserId);

        if (administrator?.Role != UserRole.Administrator)
            return Result<bool>.NotValid(string.Format("Пользователь с Id: {0} не сущуствует или не обладает нужными правами", removeUserId));

        var user = await _userRepository.GetAsync(targetUserId);

        if (user is null)
            return Result<bool>.NotValid("Пользователь, на которого назначается блокировка не существует");

        if (!user.IsBlocking)
            return Result<bool>.NotValid("Пользователь не заблокирован");

        await _blockRepository.RemoveAsync(user.Block);

        return Result<bool>.FromValue(true);
    }
}
