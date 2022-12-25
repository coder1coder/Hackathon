namespace Hackathon.Common.Models.User;

/// <summary>
/// Параметры отправляемых сообщений
/// </summary>
public class EmailParameters
{
    /// <summary>
    /// Email для отправки
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Тема сообщения
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Тело сообщения
    /// </summary>
    public string Body { get; set; }
}
