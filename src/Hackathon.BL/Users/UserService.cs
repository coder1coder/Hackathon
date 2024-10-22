using System;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Email;
using Hackathon.BL.Validation;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Users;
using Hackathon.Configuration;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.FileStorage.BL.Services;
using Hackathon.FileStorage.BL.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Users;

public class UserService: IUserService
{
    /// <see cref="SignUpModelValidator"/>
    private readonly IValidator<CreateNewUserModel> _signUpModelValidator;
    private readonly IValidator<UpdateUserParameters> _updateUserParametersValidator;
    private readonly IValidator<IFileImage> _profileImageValidator;
    private readonly IUserRepository _userRepository;
    private readonly IEmailConfirmationRepository _emailConfirmationRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly int _requestLifetimeMinutes;
    private readonly IValidator<UpdatePasswordModel> _updatePasswordModelValidator;
    private readonly IPasswordHashService _passwordHashService;

    public UserService(
        IOptions<EmailSettings> emailSettings,
        IValidator<IFileImage> profileImageValidator,
        IValidator<CreateNewUserModel> signUpModelValidator,
        IValidator<UpdateUserParameters> updateUserParametersValidator,
        IUserRepository userRepository,
        IEmailConfirmationRepository emailConfirmationRepository,
        IFileStorageService fileStorageService,
        IFileStorageRepository fileStorageRepository,
        IValidator<UpdatePasswordModel> updatePasswordModelValidator, 
        IPasswordHashService passwordHashService)
    {
        _signUpModelValidator = signUpModelValidator;
        _updateUserParametersValidator = updateUserParametersValidator;
        _profileImageValidator = profileImageValidator;
        _userRepository = userRepository;
        _emailConfirmationRepository = emailConfirmationRepository;
        _fileStorageService = fileStorageService;
        _fileStorageRepository = fileStorageRepository;
        _updatePasswordModelValidator = updatePasswordModelValidator;
        _passwordHashService = passwordHashService;
        _requestLifetimeMinutes = emailSettings?.Value?.EmailConfirmationRequestLifetime ?? EmailConfirmationService.EmailConfirmationRequestLifetimeDefault;
    }

    public async Task<long> CreateAsync(CreateNewUserModel createNewUserModel)
    {
        await _signUpModelValidator.ValidateAndThrowAsync(createNewUserModel);

        //В случаях с внешними сервисами аутентификации, например Google, пароль может отсутствовать
        if (createNewUserModel.Password is not null)
        {
            createNewUserModel.Password = _passwordHashService.HashPassword(createNewUserModel.Password);
        }

        return await _userRepository.CreateAsync(createNewUserModel);
    }

    public async Task<Result<UserModel>> GetAsync(long userId)
    {
        var model = await _userRepository.GetAsync(userId);
        
        if (model is null)
        {
            Result.NotFound(UserValidationErrorMessages.UserDoesNotExists);
        }

        EnrichModel(model);

        return Result<UserModel>.FromValue(model);
    }

    public async Task<BaseCollection<UserModel>> GetListAsync(Common.Models.GetListParameters<UserFilter> getListParameters)
    {
        var models = await _userRepository.GetAsync(getListParameters);

        if (models.Items is {Count: > 0})
        {
            models.Items = models.Items.Select(EnrichModel).ToArray();
        }

        return models;
    }

    public async Task<Result<Guid>> UploadProfileImageAsync(long userId, IFormFile file)
    {
        if (file is null)
        {
            return Result<Guid>.NotValid(UserErrorMessages.ProfileFileImageIsNotBeEmpty);
        }

        await using var stream = file.OpenReadStream();

        var image = await ImageLoader.LoadFromStreamAsync(stream, file.FileName, fileImage => 
            new ProfileFileImage(fileImage.Width, fileImage.Height, fileImage.Length, fileImage.Extension));

        await _profileImageValidator.ValidateAndThrowAsync(image, options =>
            options.IncludeRuleSets(FileImageValidatorRuleSets.ProfileImage));

        var user = await _userRepository.GetAsync(userId);

        if (user is null)
        {
            return Result<Guid>.NotFound(UserValidationErrorMessages.UserDoesNotExists);
        }

        var uploadResult = await _fileStorageService.UploadAsync(stream, Bucket.Avatars, file.FileName, userId);

        if (user.ProfileImageId.HasValue)
        {
            await _fileStorageRepository.UpdateFlagIsDeleted(user.ProfileImageId.Value, true);
        }

        await _userRepository.UpdateProfileImageAsync(userId, uploadResult.Id);
        return Result<Guid>.FromValue(uploadResult.Id);
    }

    public async Task<Result> UpdateUserAsync(UpdateUserParameters updateUserParameters)
    {
        await _updateUserParametersValidator.ValidateAndThrowAsync(updateUserParameters);

        var isUserExists = await _userRepository.ExistsAsync(updateUserParameters.Id);

        if (!isUserExists)
        {
            throw new ValidationException(UserValidationErrorMessages.UserDoesNotExists);
        }

        var model = await _userRepository.GetAsync(updateUserParameters.Id);

        if (model.Email.Address is not null && !model.Email.Address.Equals(updateUserParameters.Email))
        {
            await _emailConfirmationRepository.DeleteAsync(model.Id);
        }

        await _userRepository.UpdateAsync(updateUserParameters);
        return Result.Success;
    }

    public async Task<Result> UpdatePasswordAsync(long authorizedUserId, UpdatePasswordModel parameters)
    {
        await _updatePasswordModelValidator.ValidateAndThrowAsync(parameters);

        var userPasswordHash = await _userRepository.GetPasswordHashAsync(authorizedUserId);
        if (userPasswordHash is null)
        {
            return Result.NotFound(UserErrorMessages.UserDoesNotExists);
        }

        var isPasswordCorrect = await _passwordHashService.VerifyAsync(parameters.CurrentPassword, userPasswordHash);
        if (!isPasswordCorrect)
        {
            return Result.NotValid(UserErrorMessages.CurrentPasswordIncorrect);
        }

        var newPasswordHash = _passwordHashService.HashPassword(parameters.NewPassword);

        await _userRepository.UpdatePasswordHashAsync(authorizedUserId, newPasswordHash);

        return Result.Success;
    }

    private UserModel EnrichModel(UserModel model)
    {
        if (model.Email.ConfirmationRequest is not null)
        {
            if (model.Email.ConfirmationRequest.IsConfirmed)
            {
                model.Email.Status = UserEmailStatus.Confirmed;
            }
            else if (DateTime.UtcNow < model.Email.ConfirmationRequest.CreatedAt.AddMinutes(_requestLifetimeMinutes))
            {
                model.Email.Status = UserEmailStatus.Pending;
            }
            else
            {
                model.Email.Status = UserEmailStatus.NotConfirmed;
            }
        }

        return model;
    }
}
