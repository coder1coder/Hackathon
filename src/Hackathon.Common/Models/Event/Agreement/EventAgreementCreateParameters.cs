namespace Hackathon.Common.Models.Event.Agreement;

/// <summary>
/// Параметры создания соглашения об участии в мероприятии
/// </summary>
public class EventAgreementCreateParameters
{
    /// <summary>
    /// Правила проведения мероприятия
    /// </summary>
    public string Rules { get; set; }

    /// <summary>
    /// Требуется ли подтверждение соглашения
    /// </summary>
    public bool RequiresConfirmation { get; set; }
}
