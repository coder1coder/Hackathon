using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Informing.Abstractions.Constants;
using Hackathon.Informing.Abstractions.Models.Templates;
using Hackathon.Informing.Abstractions.Repositories;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Email;

public class TemplateServiceTests: BaseUnitTest
{
    private readonly ITemplateService _templateService;
    private readonly Mock<ITemplateRepository> _templateRepository;

    public TemplateServiceTests()
    {
        _templateRepository = new Mock<ITemplateRepository>();

        _templateService = new TemplateService(
            NullLogger<TemplateService>.Instance,
            _templateRepository.Object
            );
    }

    [Fact(DisplayName = "Генерация шаблона запроса на подтверждение Email")]
    public async Task Generate_EmailConfirmationRequest_Should_Success()
    {
        //arrange
        const string templateId = Templates.EmailConfirmationRequest;
        var variables = new Dictionary<string, string>
        {
            { Variables.UserName, Guid.NewGuid().ToString() },
            { Variables.ConfirmationCode, Guid.NewGuid().ToString() },
            { Variables.ConfirmationLink, Guid.NewGuid().ToString() }
        };

        _templateRepository.Setup(x =>
                x.GetAsync(templateId))
            .ReturnsAsync(new TemplateModel
            {
                Id = templateId,
                Content = "<!DOCTYPE html><html lang=\"en\">" +
                          "<head><meta charset=\"UTF-8\">" +
                          "<title>Подтверждение Email</title></head><body>\n" +
                          "Уважаемый {{username}}, подтвердите свой Email перейдя по сссылке указанной ниже" +
                          " или указав код подтверждения\nв настройках профиля. <br/>" +
                          "<a href=\"{{confirmationLink}}\">{{confirmationLink}}</a>" +
                          "<br/>\nКод подтверждения: <h3>{{confirmationCode}}</h3></body></html>"
            });

        //act
        var result = await _templateService.GenerateAsync(templateId, variables);
        
        //assert
        Assert.True(result.IsSuccess);
        result.Data.Should().Be("<!DOCTYPE html><html lang=\"en\">" +
                                "<head><meta charset=\"UTF-8\">" +
                                "<title>Подтверждение Email</title></head><body>\n" +
                                $"Уважаемый {variables[Variables.UserName]}, подтвердите свой Email перейдя по сссылке указанной ниже" +
                                " или указав код подтверждения\nв настройках профиля. <br/>" +
                                $"<a href=\"{variables[Variables.ConfirmationLink]}\">{variables[Variables.ConfirmationLink]}</a>" +
                                $"<br/>\nКод подтверждения: <h3>{variables[Variables.ConfirmationCode]}</h3></body></html>");
    }
}
