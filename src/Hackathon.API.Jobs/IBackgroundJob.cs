using System.Threading.Tasks;

namespace Hackathon.Jobs;

public interface IBackgroundJob
{
    /// <summary>
    /// Метод выполняющий работу джобы
    /// </summary>
    /// <returns></returns>
    Task DoWork();
}
