using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Jobs;

public interface IJob
{
    Task Execute();
}