namespace Hackathon.Abstraction.Jobs;

public interface IJob
{
    Task Execute();
}