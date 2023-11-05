using System.Net.Mail;
using System.Threading.Tasks;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.Tests.Integration;

public class EmailServiceTests: BaseIntegrationTest
{
    private readonly IEmailService _service;

    public EmailServiceTests(TestWebApplicationFactory factory) : base(factory)
    {
        var emailSettingsMock = new Mock<IOptions<EmailSettings>>();
        emailSettingsMock.Setup(x => x.Value)
            .Returns(new EmailSettings
            {
                EmailSender = new EmailSenderSettings
                {
                    Sender = EmailSettings.EmailSender.Sender
                }
            });

        var smtpClient = factory.Services.GetRequiredService<SmtpClient>();
        _service = new EmailService(emailSettingsMock.Object, NullLogger<EmailService>.Instance, smtpClient);
    }

#pragma warning disable xUnit1004
    [Fact(DisplayName = "Отправка Email", Skip = "Предназначен только для локальной проверки")]
#pragma warning restore xUnit1004
    public async Task SendAsync_Test()
    {
        var result = await _service.SendAsync(new EmailParameters
        {
            Email = EmailSettings.EmailSender.Sender,
            Subject = "subject",
            Body = "<b>OK</b>"
        });

        Assert.True(result.IsSuccess);
    }
}
