namespace Hackathon.Informing.DAL.Entities;

/// <summary>
/// Шаблон информирования
/// </summary>
public class TemplateEntity
{
    /// <summary>
    /// Идентификатор шаблона
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Содержимое шаблона
    /// </summary>
    public string Content { get; set; }
}
