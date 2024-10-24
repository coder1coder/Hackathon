namespace Hackathon.Common.Models.Teams;

public class CancelRequestParameters
{
    /// <summary>
    /// Идентификатор запроса
    /// </summary>
    public long RequestId { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; }
}
