using System.Threading.Tasks;

namespace Hackathon.Notification
{
    public interface IMessageHub<in T> where T: IIntegrationEvent
    {
        Task Publish(string topic, T message);
    }
}