using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.Email;

public class TemplateService: ITemplateService
{
    private const string TemplateParameterPrefix = "[";
    private const string TemplateParameterPostfix = "]";
    private const string TemplateExtension = "html";

    private readonly ILogger<TemplateService> _logger;

    public TemplateService(ILogger<TemplateService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<string>> Generate(string templateName, Dictionary<string, string> templateParameters)
    {
        var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Templates\\{templateName}.{TemplateExtension}");

        if (!File.Exists(templatePath))
        {
            _logger.LogError("{Initiator}. Не удалось найти шаблон '{TemplateName}'",
                nameof(TemplateService),
                templateName);
            return Result<string>.NotFound($"Не удалось найти шаблон '{templateName}'");
        }

        var template = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);

        if (templateParameters is {Count: > 0})
            foreach (var (key, value) in templateParameters)
                template = template.Replace(
                    $"{TemplateParameterPrefix}{key}{TemplateParameterPostfix}",
                    value,
                    StringComparison.OrdinalIgnoreCase);

        return Result<string>.FromValue(template);
    }
}

public interface ITemplateService
{
    Task<Result<string>> Generate(string templateName, Dictionary<string, string> templateParameters);
}
