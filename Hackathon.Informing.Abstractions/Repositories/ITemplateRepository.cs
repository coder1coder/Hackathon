using System.Threading.Tasks;
using Hackathon.Informing.Abstractions.Models.Templates;

namespace Hackathon.Informing.Abstractions.Repositories;

/// <summary>
/// Шаблон
/// </summary>
public interface ITemplateRepository
{
    /// <summary>
    /// Получить шаблон
    /// </summary>
    /// <param name="templateId">Идентификатор шаблона</param>
    /// <returns></returns>
    Task<TemplateModel> GetAsync(string templateId);
}
