using System.Net.Mail;
using System.Threading.Tasks;
using Hackathon.BL.Email;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
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

    [Fact(DisplayName = "Отправка Email", Skip = "Предназначен только для локальной проверки")]
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
