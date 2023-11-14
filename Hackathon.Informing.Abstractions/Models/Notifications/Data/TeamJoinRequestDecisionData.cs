namespace Hackathon.Informing.Abstractions.Models.Notifications.Data;

public record TeamJoinRequestDecisionData
{
    /// <summary>
    /// Результат решения по запросу
    /// </summary>
    public bool IsApproved { get; set; }
    
    /// <summary>
    /// Комментарий
    /// <remarks>Устанавливается в случае отрицательного решения</remarks>
    /// </summary>
    public string Comment { get; set; }
    
    /// <summary>
    /// Идентификатор команд
    /// </summary>
    public long TeamId { get; set; }
    
    /// <summary>
    /// Наименование команды
    /// </summary>
    public string TeamName { get; set; }
}
