using Hackathon.Common.Models.Block;
using Hackathon.Common.Models.Event;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Block;

public interface IBlockingRepository
{
    /// <summary>
    /// Создание блокировки
    /// </summary>
    /// <param name="blocking">Модель создаваемой блокировки</param>
    /// <returns></returns>
    Task<long> CreateAsync(BlockingModel blocking);

    /// <summary>
    /// Удаление блокировки
    /// </summary>
    /// <param name="blocking">Модель удаляемой блокировки</param>
    /// <returns></returns>
    Task RemoveAsync(BlockingModel blocking);
}
