using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;

namespace Hackathon.Informing.Abstractions.Services;

/// <summary>
/// Сервис по работе с шаблонами
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Сгенерировать строковое представление по шаблону
    /// </summary>
    /// <param name="templateId">Наименование шаблона</param>
    /// <param name="templateParameters">Параметры шаблона</param>
    /// <returns></returns>
    Task<Result<string>> GenerateAsync(string templateId, Dictionary<string, string> templateParameters);
}
