using Hackathon.Common.Models.Block;
using Hackathon.Common.Models.Event;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Block;

public interface IBlockRepository
{
    /// <summary>
    /// Создание блокировки
    /// </summary>
    /// <param name="blockCreateParameters">Модель создаваемой блокировки</param>
    /// <returns></returns>
    Task<long> CreateAsync(BlockModel block);
}
