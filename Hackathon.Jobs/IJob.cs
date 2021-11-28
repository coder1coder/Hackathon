using System.Threading.Tasks;

namespace Hackathon.Jobs
{
    public interface IJob
    {
        string Name { get; }
        string CronInterval { get; }
        Task Execute();
    }
}