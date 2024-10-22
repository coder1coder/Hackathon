namespace Hackathon.Common.Models.Projects;

public interface IHasProjectIdentity
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
