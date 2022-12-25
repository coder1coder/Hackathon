using System;
using System.Diagnostics.Tracing;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Abstraction.User;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Email;

public class EmailService: IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSenderSettings _emailSenderSettings;
    private readonly SmtpClient _smtpClient;

    public EmailService(
        IOptions<AppSettings> appSettings,
        ILogger<EmailService> logger,
        SmtpClient smtpClient)
    {
        _logger = logger;
        _smtpClient = smtpClient;

        _emailSenderSettings = appSettings?.Value?.EmailSender ?? new EmailSenderSettings();
    }

    public async Task<Result> SendAsync(EmailParameters parameters)
    {
        try
        {
            await _smtpClient.SendMailAsync(new MailMessage
            {
                From = new MailAddress(_emailSenderSettings.Sender, _emailSenderSettings.Sender),
                To = { parameters.Email },
                Subject = parameters.Subject,
                SubjectEncoding = Encoding.UTF8,
                Body = parameters.Body,
                BodyEncoding = Encoding.UTF8,
                Sender = new MailAddress(_emailSenderSettings.Sender, _emailSenderSettings.Sender),
                IsBodyHtml = true
            });

            return Result.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время отправки Email");
            return Result.Internal("Ошибка во время отправки Email");
        }
    }
}