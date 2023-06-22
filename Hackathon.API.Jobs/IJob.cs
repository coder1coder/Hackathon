using System.Threading.Tasks;

namespace Hackathon.Jobs;

/// <summary>
/// Служба
/// </summary>
public interface IJob
{
    /// <summary>
    /// Основной метод содержащий логику работы службы
    /// </summary>
    /// <returns></returns>
    Task ExecuteAsync();
}
