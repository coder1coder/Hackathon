namespace Hackathon.Informing.Abstractions.Models.Templates;

/// <summary>
/// Шаблон
/// </summary>
public class TemplateModel
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
