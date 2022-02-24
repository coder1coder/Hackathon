using System.Threading.Tasks;

namespace Hackathon.Notification
{
    public interface IMessageHub<in T> where T: IIntegrationEvent
    {
        /// <summary>
        /// Публикует сообщение в шине
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Publish(string topic, T message);
    }
}