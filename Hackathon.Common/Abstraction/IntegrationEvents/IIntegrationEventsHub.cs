using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.IntegrationEvents;

public interface IIntegrationEventsHub<in T> where T: IIntegrationEvent
{
    /// <summary>
    /// Публикует сообщение всем
    /// </summary>
    /// <param name="topic">Топик</param>
    /// <param name="message">Сообщение</param>
    /// <returns></returns>
    Task PublishAll(string topic, T message);
}

public interface IIntegrationEventsHub
{
    /// <summary>
    /// Публикует сообщение всем
    /// </summary>
    /// <param name="integrationEvent">Сообщение</param>
    /// <returns></returns>
    Task PublishAll(IIntegrationEvent integrationEvent);
}
