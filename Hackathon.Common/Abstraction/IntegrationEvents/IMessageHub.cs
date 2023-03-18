using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.IntegrationEvents;

public interface IMessageHub<in T> where T: IIntegrationEvent
{
    /// <summary>
    /// Публикует сообщение
    /// </summary>
    /// <param name="topic">Топик</param>
    /// <param name="message">Сообщение</param>
    /// <returns></returns>
    Task Publish(string topic, T message);
}