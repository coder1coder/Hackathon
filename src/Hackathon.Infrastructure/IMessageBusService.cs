using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Infrastructure;

public interface IMessageBusService
{
    /// <summary>
    /// Попытаться опубликовать сообщение в шину
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">тип сообщения</typeparam>
    Task<bool> TryPublish<T>(T message, CancellationToken cancellationToken = default) where T : class;
}
