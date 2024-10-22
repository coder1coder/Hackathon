using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Google.Apis.Auth;
using Hackathon.BL.Users;
using Hackathon.Common;
using Hackathon.Common.Abstraction.Auth;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Auth;
using Hackathon.Common.Models.Users;
using Hackathon.Configuration.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Auth;

public class AuthService: IAuthService
{
    private readonly IPasswordHashService _passwordHashService;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<SignInModel> _signInModelValidator;
    private readonly AuthenticateSettings _authenticateSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly IUserService _userService;

    public AuthService(
        IPasswordHashService passwordHashService, 
        IUserRepository userRepository, 
        IValidator<SignInModel> signInModelValidator,
        IOptions<AuthenticateSettings> authOptions, 
        ILogger<AuthService> logger, 
        IUserService userService)
    {
        _passwordHashService = passwordHashService;
        _userRepository = userRepository;
        _signInModelValidator = signInModelValidator;
        _logger = logger;
        _userService = userService;
        _authenticateSettings = authOptions?.Value ?? new AuthenticateSettings();
    }

    public async Task<Result<AuthTokenModel>> SignInAsync(SignInModel signInModel)
    {
        var modelValidationResult = await _signInModelValidator.ValidateAsync(signInModel);
        if (!modelValidationResult.IsValid)
            //Нет необходимости указывать причины некорректного ввода
        {
            return Result<AuthTokenModel>.NotValid(UserErrorMessages.IncorrectUserNameOrPassword);
        }

        var userSignInDetails = await _userRepository.GetUserSignInDetailsAsync(signInModel.UserName);

        if (userSignInDetails is null)
        {
            return Result<AuthTokenModel>.NotFound(UserErrorMessages.UserDoesNotExists);
        }

        var verified = await _passwordHashService.VerifyAsync(signInModel.Password, userSignInDetails.PasswordHash);

        return !verified
            ? Result<AuthTokenModel>.NotValid(UserErrorMessages.IncorrectUserNameOrPassword)
            : Result<AuthTokenModel>.FromValue(AuthTokenGenerator.GenerateToken(new GenerateTokenPayload
            {
                UserId = userSignInDetails.UserId,
                UserRole = userSignInDetails.UserRole,
                GoogleAccountId = userSignInDetails.GoogleAccountId
            }, _authenticateSettings.Internal));
    }

    public async Task<Result<AuthTokenModel>> SignInByGoogleAsync(SignInByGoogleModel signInByGoogleModel)
    {
        var payload = await VerifyGoogleToken(signInByGoogleModel.AccessToken);
        
        if (payload is null)
        {
            return Result<AuthTokenModel>.NotValid("Указан некорректный токен авторизации");
        }

        //TODO: расширить модель UserSignInDetails необходимыми данными и использовать GetUserSignInDetailsAsync
        var userModel = await _userRepository.GetByGoogleIdOrEmailAsync(payload.Subject, payload.Email);
        
        var googleAccount = new GoogleAccountModel
        {
            Id = payload.Subject,
            Email = payload.Email,
            FullName = payload.Name,
            ImageUrl = payload.Picture
        };

        if (userModel is null)
        {
            var newUserParameters = new CreateNewUserModel
            {
                Email = googleAccount.Email,
                UserName = googleAccount.Email,
                FullName = googleAccount.FullName,
                Password = null,
                GoogleAccount = googleAccount
            };

            await _userService.CreateAsync(newUserParameters);
        }
        else if (!googleAccount.Equals(userModel.GoogleAccount))
        {
            await _userRepository.UpdateGoogleAccount(googleAccount);
        }

        var userSignInDetails = await _userRepository.GetUserSignInDetailsAsync(userModel?.UserName ?? payload.Email);

        return Result<AuthTokenModel>.FromValue(AuthTokenGenerator.GenerateToken(new GenerateTokenPayload
        {
            UserId = userSignInDetails.UserId,
            UserRole = userSignInDetails.UserRole,
            GoogleAccountId = userSignInDetails.GoogleAccountId
        }, _authenticateSettings.Internal));
    }
    
    private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string tokenId)
    {
        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(tokenId, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _authenticateSettings.External.Google.Audience }
            });
        }
        catch (InvalidJwtException e)
        {
            _logger.LogError(e, "{Source} Ошибка во время валидации токена Google. {Message}",
                nameof(UserService), e.Message);
            return null;
        }
    }
}
