using BackendTools.Common.Models;
using Hackathon.Common.Models.Block;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Block;

public interface IBlockingService
{
    /// <summary>
    /// Создание блокировки
    /// </summary>
    /// <param name="blockCreateParameters">Модель создаваемой блокировки</param>
    /// <returns></returns>
    Task<Result<long>> CreateAsync(BlockingCreateParameters blockCreateParameters);

    /// <summary>
    /// Проверка заблокирован ли пользователь
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <returns></returns>
    Task<bool> CheckBlocking(long userId);
}
