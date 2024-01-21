using System;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Email;
using Hackathon.BL.Validation;
using Hackathon.BL.Validation.Users;
using Hackathon.Common;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.FileStorage.BL.Validators;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hackathon.BL.Users;

public class UserService: IUserService
{
    /// <see cref="SignUpModelValidator"/>
    private readonly IValidator<SignUpModel> _signUpModelValidator;
    private readonly IValidator<SignInModel> _signInModelValidator;
    private readonly IValidator<UpdateUserParameters> _updateUserParametersValidator;
    private readonly IValidator<IFileImage> _profileImageValidator;
    private readonly IUserRepository _userRepository;
    private readonly IEmailConfirmationRepository _emailConfirmationRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly int _requestLifetimeMinutes;
    private readonly IMapper _mapper;
    private readonly AuthOptions _authOptions;

    public UserService(
        IOptions<EmailSettings> emailSettings,
        IOptions<AuthOptions> authOptions,
        IValidator<IFileImage> profileImageValidator,
        IValidator<SignUpModel> signUpModelValidator,
        IValidator<SignInModel> signInModelValidator,
        IValidator<UpdateUserParameters> updateUserParametersValidator,
        IUserRepository userRepository,
        IEmailConfirmationRepository emailConfirmationRepository,
        IFileStorageService fileStorageService,
        IFileStorageRepository fileStorageRepository,
        IMapper mapper)
    {
        _authOptions = authOptions?.Value ?? new AuthOptions();
        _signUpModelValidator = signUpModelValidator;
        _signInModelValidator = signInModelValidator;
        _updateUserParametersValidator = updateUserParametersValidator;
        _profileImageValidator = profileImageValidator;
        _userRepository = userRepository;
        _emailConfirmationRepository = emailConfirmationRepository;
        _fileStorageService = fileStorageService;
        _fileStorageRepository = fileStorageRepository;
        _mapper = mapper;
        _requestLifetimeMinutes = emailSettings?.Value?.EmailConfirmationRequestLifetime ?? EmailConfirmationService.EmailConfirmationRequestLifetimeDefault;
    }

    public async Task<long> CreateAsync(SignUpModel signUpModel)
    {
        await _signUpModelValidator.ValidateAndThrowAsync(signUpModel);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password);
        signUpModel.Password = passwordHash;

        return await _userRepository.CreateAsync(signUpModel);
    }

    public async Task<Result<AuthTokenModel>> SignInAsync(SignInModel signInModel)
    {
        var modelValidationResult = await _signInModelValidator.ValidateAsync(signInModel);
        if (!modelValidationResult.IsValid)
            //Нет необходимости указывать причины некорректного ввода
            return Result<AuthTokenModel>.NotValid(UserErrorMessages.IncorrectUserNameOrPassword);

        var userSignInDetails = await _userRepository.GetUserSignInDetailsAsync(signInModel.UserName);

        if (userSignInDetails is null)
            return Result<AuthTokenModel>.NotFound(UserErrorMessages.UserDoesNotExists);

        var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, userSignInDetails.PasswordHash);

        return !verified
            ? Result<AuthTokenModel>.NotValid(UserErrorMessages.IncorrectUserNameOrPassword)
            : Result<AuthTokenModel>.FromValue(AuthTokenGenerator.GenerateToken(userSignInDetails, _authOptions));
    }

    public async Task<AuthTokenModel> SignInByGoogle(SignInByGoogleModel signInByGoogleModel)
    {
        //TODO: расширить модель UserSignInDetails необходимыми данными и использовать GetUserSignInDetailsAsync
        var userModel = await _userRepository.GetByGoogleIdOrEmailAsync(signInByGoogleModel.Id, signInByGoogleModel.Email);

        if (userModel != null)
        {
            userModel.GoogleAccount = _mapper.Map<GoogleAccountModel>(signInByGoogleModel);
            await _userRepository.UpdateGoogleAccount(userModel.GoogleAccount);
        }
        else
        {
            var userId = await _userRepository.CreateAsync(new SignUpModel
            {
                Email = signInByGoogleModel.Email,
                UserName = signInByGoogleModel.FullName,
                FullName = signInByGoogleModel.FullName,
                Password = signInByGoogleModel.Id,
                GoogleAccount = signInByGoogleModel
            });

            userModel = await _userRepository.GetAsync(userId);
        }

        var userSignInDetails = await _userRepository.GetUserSignInDetailsAsync(userModel.UserName);

        return AuthTokenGenerator.GenerateToken(userSignInDetails, _authOptions);
    }

    public async Task<Result<UserModel>> GetAsync(long userId)
    {
        var model = await _userRepository.GetAsync(userId);
        
        if (model is null)
            Result.NotFound(UserValidationErrorMessages.UserDoesNotExists);
        
        EnrichModel(model);

        return Result<UserModel>.FromValue(model);
    }

    public async Task<BaseCollection<UserModel>> GetListAsync(Common.Models.GetListParameters<UserFilter> getListParameters)
    {
        var models = await _userRepository.GetAsync(getListParameters);

        if (models.Items is {Count: > 0})
            models.Items = models.Items.Select(EnrichModel).ToArray();

        return models;
    }

    public async Task<Result<Guid>> UploadProfileImageAsync(long userId, IFormFile file)
    {
        if (file is null)
            return Result<Guid>.NotValid(UserErrorMessages.ProfileFileImageIsNotBeEmpty);

        await using var stream = file.OpenReadStream();

        var image = ProfileFileImage.FromStream(stream, file.FileName);

        await _profileImageValidator.ValidateAndThrowAsync(image, options =>
            options.IncludeRuleSets(FileImageValidatorRuleSets.ProfileImage));

        var user = await _userRepository.GetAsync(userId);

        if (user is null)
            return Result<Guid>.NotFound(UserValidationErrorMessages.UserDoesNotExists);

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
            throw new ValidationException(UserValidationErrorMessages.UserDoesNotExists);

        var model = await _userRepository.GetAsync(updateUserParameters.Id);

        if (model.Email.Address is not null && !model.Email.Address.Equals(updateUserParameters.Email))
        {
            await _emailConfirmationRepository.DeleteAsync(model.Id);
        }

        await _userRepository.UpdateAsync(updateUserParameters);
        return Result.Success;
    }

    private UserModel EnrichModel(UserModel model)
    {
        if (model.Email.ConfirmationRequest is not null)
        {
            if (model.Email.ConfirmationRequest.IsConfirmed)
                model.Email.Status = UserEmailStatus.Confirmed;
            else if (DateTime.UtcNow < model.Email.ConfirmationRequest.CreatedAt.AddMinutes(_requestLifetimeMinutes))
                model.Email.Status = UserEmailStatus.Pending;
            else
                model.Email.Status = UserEmailStatus.NotConfirmed;
        }

        return model;
    }
}
