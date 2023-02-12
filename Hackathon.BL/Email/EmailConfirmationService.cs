using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Abstraction.User;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Email;

public class EmailConfirmationService: IEmailConfirmationService
{
    public const int EmailConfirmationRequestLifetimeDefault = 5;

    private const string EmailConfirmationTitle = "Hackathon: подтвердите свой Email";
    private const string EmailAlreadyConfirmed = "Email пользователя уже подтвержден";
    private const string ConfirmationCodeIsWrong = "Код подтверждение указан неверно";
    private const string EmailConfirmationRequestWasNotFound = "Запрос на подтверждение Email пользователя не найден";

    private readonly IEmailConfirmationRepository _emailConfirmationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;

    private readonly int _requestLifetimeMinutes;
    private readonly ILogger<EmailConfirmationService> _logger;

    public EmailConfirmationService(
        IEmailConfirmationRepository emailConfirmationRepository,
        IUserRepository userRepository,
        IOptions<AppSettings> appSettings,
        ILogger<EmailConfirmationService> logger,
        IEmailService emailService,
        ITemplateService templateService)
    {
        _emailConfirmationRepository = emailConfirmationRepository;
        _userRepository = userRepository;
        _logger = logger;
        _emailService = emailService;
        _templateService = templateService;
        _requestLifetimeMinutes = appSettings?.Value?.EmailConfirmationRequestLifetime ?? EmailConfirmationRequestLifetimeDefault;
    }

    public async Task<Result> Confirm(long userId, string code)
    {
        _logger.LogDebug("{Actor}: Call confirm email for user with Id: {UserId}",
            nameof(EmailConfirmationService),
            userId);

        var userModel = await _userRepository.GetAsync(userId);

        if (userModel is null)
            return Result.NotValid(string.Format(EmailConfirmationErrorPatterns.UserWithIdNotFound, userId));

        var request = await _emailConfirmationRepository.GetByUserIdAsync(userId);

        if (request is null || userModel.Email.Address != request.Email)
            return Result.NotValid(EmailConfirmationRequestWasNotFound);

        if (request.IsConfirmed)
            return Result.NotValid(EmailAlreadyConfirmed);

        if (request.Code != code)
            return Result.NotValid(ConfirmationCodeIsWrong);

        request.IsConfirmed = true;

        await _emailConfirmationRepository.UpdateAsync(request);

        _logger.LogDebug("{Actor}: Email was confirmed for user with Id: {UserId}",
            nameof(EmailConfirmationService),
            userId);

        return Result.Success;
    }

    public async Task<Result> CreateRequest(long userId)
    {
        _logger.LogDebug("{Actor}: Call create email confirmation request for user with Id: {UserId}",
            nameof(EmailConfirmationService),
            userId);

        var userModel = await _userRepository.GetAsync(userId);

        if (userModel is null)
            return Result.NotValid(string.Format(EmailConfirmationErrorPatterns.UserWithIdNotFound, userId));

        if (userModel.Email is null)
            return Result.NotValid(EmailConfirmationErrorPatterns.UsersEmailIsNotListed);

        var requestModel = await _emailConfirmationRepository.GetByUserIdAsync(userId);

        if (requestModel is not null && requestModel.Email == userModel.Email?.Address)
        {
            if (requestModel.IsConfirmed)
                return Result.NotValid("Email пользователя уже подтвержден");

            if (DateTime.UtcNow < requestModel.CreatedAt.AddMinutes(_requestLifetimeMinutes))
                return Result.NotValid("Запрос на подтверждение Email уже был отправлен");
        }

        var confirmationCode = GenerateConfirmationCode();
        var emailSendResult = await SendConfirmationEmail(userModel, confirmationCode);

        if (!emailSendResult.IsSuccess)
            return Result.Success;

        var parameters = new EmailConfirmationRequestParameters
        {
            UserId = userModel.Id,
            Email = userModel.Email?.Address,
            Code = confirmationCode
        };

        if (requestModel is null)
        {
            await _emailConfirmationRepository.AddAsync(parameters);
            _logger.LogDebug("{Actor}: Email confirmation request was created for user with Id: {UserId}",
                nameof(EmailConfirmationService),
                userId);
        }
        else
        {
            await _emailConfirmationRepository.UpdateAsync(parameters);
            _logger.LogDebug("{Actor}: Email confirmation request was updated for user with Id: {UserId}",
                nameof(EmailConfirmationService),
                userId);
        }

        return Result.Success;
    }

    private async Task<Result> SendConfirmationEmail(UserModel userModel, string confirmationCode)
    {
        var generateTemplateResult = await _templateService.Generate("EmailConfirmationRequestTemplate", new Dictionary<string, string>
        {
            {"username", userModel.FullName ?? userModel.UserName},
            {"confirmationCode", confirmationCode}
        });

        if (!generateTemplateResult.IsSuccess)
            return Result.FromErrors(generateTemplateResult.Errors);

        var emailParameters = new EmailParameters
        {
            Email = userModel.Email.Address,
            Subject = EmailConfirmationTitle,
            Body = generateTemplateResult.Data
        };

        return await _emailService.SendAsync(emailParameters);
    }

    private static string GenerateConfirmationCode()
        => Guid.NewGuid().ToString();
}
