

using System.Collections.Generic;
using System.Text;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Abstraction.User;
using Hackathon.BL.Email;
using Hackathon.BL.Validation.User;
using Hackathon.Common;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.BL.User;

public class UserService: IUserService
{
    /// <see cref="SignUpModelValidator"/>
    private readonly IValidator<SignUpModel> _signUpModelValidator;
    private readonly IValidator<SignInModel> _signInModelValidator;
    private readonly IValidator<UpdateUserParameters> _updateUserParameters;

    private readonly IUserRepository _userRepository;

    private readonly IEmailConfirmationRepository _emailConfirmationRepository;
    private readonly AppSettings _appSettings;

    private readonly IFileStorageService _fileStorageService;

    private readonly int _requestLifetimeMinutes;

    private readonly IMapper _mapper;

    private readonly AuthOptions _authOptions;

    public UserService(
        IOptions<EmailSettings> emailSettings,
        IOptions<AuthOptions> authOptions,
        IValidator<SignUpModel> signUpModelValidator,
        IValidator<SignInModel> signInModelValidator,
        IValidator<UpdateUserParameters> updateUserParameters,
        IUserRepository userRepository,
        IEmailConfirmationRepository emailConfirmationRepository,
        IFileStorageService fileStorageService,
        IMapper mapper)
    {
        _authOptions = authOptions?.Value ?? new AuthOptions();
        _signUpModelValidator = signUpModelValidator;
        _signInModelValidator = signInModelValidator;
        _updateUserParameters = updateUserParameters;
        _userRepository = userRepository;
        _emailConfirmationRepository = emailConfirmationRepository;
        _fileStorageService = fileStorageService;
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
        await _signInModelValidator.ValidateAndThrowAsync(signInModel);

        var users = await _userRepository.GetAsync(new Common.Models.GetListParameters<UserFilter>
        {
            Filter = new UserFilter
            {
                Username = signInModel.UserName
            },
            Limit = 1
        });

        if (users.Items.Count == 0)
            return Result<AuthTokenModel>.NotFound(UserErrorMessages.UserDoesNotExists);

        var user = users.Items.First();

        var verified = BCrypt.Net.BCrypt.Verify(signInModel.Password, user.PasswordHash);

        return !verified
            ? Result<AuthTokenModel>.NotValid(UserErrorMessages.IncorrectUserNameOrPassword)
            : Result<AuthTokenModel>.FromValue(AuthTokenGenerator.GenerateToken(user, _authOptions));
    }

    public async Task<AuthTokenModel> SignInByGoogle(SignInByGoogleModel signInByGoogleModel)
    {
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

        return AuthTokenGenerator.GenerateToken(userModel, _authOptions);
    }

    public async Task<Result<UserModel>> GetAsync(long userId)
    {
        if (!await _userRepository.ExistsAsync(userId))
            Result.NotFound(UserErrorMessages.UserDoesNotExists);

        var model = await _userRepository.GetAsync(userId);

        EnrichModel(model);

        return Result<UserModel>.FromValue(model);
    }

    public async Task<BaseCollection<UserModel>> GetAsync(Common.Models.GetListParameters<UserFilter> getListParameters)
    {
        var models = await _userRepository.GetAsync(getListParameters);

        if (models.Items is {Count: > 0})
            models.Items = models.Items.Select(EnrichModel).ToArray();

        return models;
    }



    public async Task<Result<Guid>> UploadProfileImageAsync(long userId, string filename, Stream stream)
    {
        var getUserResult = await GetAsync(userId);

        if (!getUserResult.IsSuccess)
            return Result<Guid>.FromErrors(getUserResult.Errors);

        var uploadResult = await _fileStorageService.UploadAsync(stream, Bucket.Avatars, filename, userId);

        if (getUserResult.Data.ProfileImageId is not null) {

            var deletionResult = await _fileStorageService.DeleteAsync(getUserResult.Data.ProfileImageId.Value);
            if (!deletionResult.IsSuccess)
                return Result<Guid>.FromErrors(deletionResult.Errors);
        }

        await _userRepository.UpdateProfileImageAsync(userId, uploadResult.Id);
        return Result<Guid>.FromValue(uploadResult.Id);
    }

    public async Task<Result> UpdateUserAsync(UpdateUserParameters updateUserParameters)
    {
        await _updateUserParameters.ValidateAndThrowAsync(updateUserParameters);

        var isUserExists = await _userRepository.ExistsAsync(updateUserParameters.Id);

        if (!isUserExists)
            throw new ValidationException(UserErrorMessages.UserDoesNotExists);

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
