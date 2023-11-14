using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace Hackathon.Informing.BL.Services;

public class TemplateService: ITemplateService
{
    private const string TemplateParameterPrefix = "{{";
    private const string TemplateParameterPostfix = "}}";

    private readonly ILogger<TemplateService> _logger;
    private readonly ITemplateRepository _templateRepository;

    public TemplateService(ILogger<TemplateService> logger, ITemplateRepository templateRepository)
    {
        _logger = logger;
        _templateRepository = templateRepository;
    }

    public async Task<Result<string>> Generate(string templateId, Dictionary<string, string> templateParameters)
    {
        var template = await _templateRepository.GetAsync(templateId);

        if (template is null)
        {
            _logger.LogError("{Initiator}. Не удалось найти шаблон '{TemplateName}'",
                nameof(TemplateService), templateId);
            return Result<string>.NotFound($"Не удалось найти шаблон '{templateId}'");
        }

        if (templateParameters is {Count: > 0})
            foreach (var (key, value) in templateParameters)
                template.Content = template.Content.Replace(
                    $"{TemplateParameterPrefix}{key}{TemplateParameterPostfix}",
                    value,
                    StringComparison.OrdinalIgnoreCase);

        return Result<string>.FromValue(template.Content);
    }
}
