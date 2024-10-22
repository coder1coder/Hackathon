namespace Hackathon.Configuration;

/// <summary>
/// Параметры ограничения наименований
/// </summary>
public sealed class RestrictedNames
{
    /// <summary>
    /// Запрещенные наименования пользователей
    /// </summary>
    public string[] Users { get; set; } = System.Array.Empty<string>();
}
