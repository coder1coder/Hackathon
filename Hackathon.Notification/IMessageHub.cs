using System.Threading.Tasks;

namespace Hackathon.MessageQueue
{
    public interface IMessageHub<in T> where T: IMessage
    {
        Task Publish(string topic, T message);
    }
}