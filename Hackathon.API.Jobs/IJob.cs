using System.Threading.Tasks;

namespace Hackathon.Jobs;

public interface IJob
{
    Task ExecuteAsync();
}
