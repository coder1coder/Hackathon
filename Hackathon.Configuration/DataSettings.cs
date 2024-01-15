namespace Hackathon.Configuration;

/// <summary>
/// Настройки источников данных
/// </summary>
public class DataSettings
{
    /// <summary>
    /// Признак применения миграций при старте приложения
    /// </summary>
    public bool ApplyMigrationsAtStart { get; set; }
    
    /// <summary>
    /// Параметры администратора по умолчанию
    /// </summary>
    public AdministratorDefaults AdministratorDefaults { get; set; }
}
