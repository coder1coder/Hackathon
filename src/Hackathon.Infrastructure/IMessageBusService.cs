using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Infrastructure;

public interface IMessageBusService
{
    /// <summary>
    /// Опубликовать сообщение в шину
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">тип сообщения</typeparam>
    Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Попытаться опубликовать сообщение в шину
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">тип сообщения</typeparam>
    Task<bool> TryPublish<T>(T message, CancellationToken cancellationToken = default) where T : class;
}
