using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;

namespace Hackathon.Common.Abstraction.Email;

/// <summary>
/// Сервис по работе с шаблонами
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Сгенерировать строковое представление по шаблону
    /// </summary>
    /// <param name="templateName">Наименование шаблона</param>
    /// <param name="templateParameters">Параметры шаблона</param>
    /// <returns></returns>
    Task<Result<string>> Generate(string templateName, Dictionary<string, string> templateParameters);
}
